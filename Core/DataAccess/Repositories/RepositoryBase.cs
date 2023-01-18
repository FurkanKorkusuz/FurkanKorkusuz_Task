using Core.DataAccess.Abstract;
using Core.DataAccess.Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Castle.Components.DictionaryAdapter;
using Core.Entities.Abstract;
using Core.BaseResults;
using Castle.DynamicProxy;
using Dapper;
using static Dapper.Contrib.Extensions.SqlMapperExtensions;
using static Slapper.AutoMapper;
using Core.Entities.Concrete;
using Core.Entities.DTOs;
using static Dapper.SqlMapper;
using Core.Utilities.CodeGenerator;
using KeyAttribute = Dapper.Contrib.Extensions.KeyAttribute;

namespace Core.DataAccess.Repositories
{

    public abstract class RepositoryBase<T, TFilter> :IEntityRepository<T,TFilter>
        where T : class, IEntity, new()
        where TFilter : class, IFilter, new()
    {
        public const int RowsPerListAddition = 30;
        //private bool _hasDto = false;

        public virtual string ParseOrderBy(string raw)
        {
            string idColumn = typeof(T).GetProperties().Where(p => p.GetCustomAttributes(true).Any(a => a is KeyAttribute)).FirstOrDefault().Name;
            string[] columns = typeof(T).GetProperties().Select(p=>p.Name).ToArray();

            if (raw == null)
                return $"ORDER BY {idColumn} DESC";

            string direction = "DESC";
            string column = raw;
            if ((raw.EndsWith("_ASC", StringComparison.OrdinalIgnoreCase) || raw.EndsWith("_DESC", StringComparison.OrdinalIgnoreCase)))
            {
                direction = raw.EndsWith("_ASC", StringComparison.OrdinalIgnoreCase) ? "ASC" : "DESC";
                column = raw.Substring(0, raw.Length - (direction == "ASC" ? 4 : 5));
            }

            if (columns.Contains(column))
                return $"ORDER BY {column} {direction}";

            return $"ORDER BY {idColumn} DESC";
        }

        public virtual long? Create(T entity, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null) => DapperUtil.Insert(entity, connection, transaction, commandTimeout);

        public virtual T GetByID(object id, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null) => DapperUtil.Get<T>(id, connection, transaction, commandTimeout);

        public virtual bool? Update(T entity, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null) => DapperUtil.Update(entity, connection, transaction, commandTimeout);

        public bool? UpdatePartial<TUpdateProperties>(T entity, Expression<Func<T, TUpdateProperties>> func, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null) where TUpdateProperties : class => DapperUtil.UpdatePartial(entity, func, connection, transaction, commandTimeout);

        public virtual bool? Delete(T entity, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null) => DapperUtil.Delete(entity, connection, transaction, commandTimeout);

        public virtual IEnumerable<T> GetAll(IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null) => DapperUtil.GetAll<T>(connection, transaction, commandTimeout);



        protected string GetTableName()
        {
            // Check if we've already set our custom table mapper to TableNameMapper.
            if (SqlMapperExtensions.TableNameMapper != null)
                return SqlMapperExtensions.TableNameMapper(typeof(T));

            // If not, we can use Dapper default method "SqlMapperExtensions.GetTableName(Type type)" which is unfortunately private, that's why we have to call it via reflection.
            string getTableName = "GetTableName";
            MethodInfo getTableNameMethod = typeof(SqlMapperExtensions).GetMethod(getTableName, BindingFlags.NonPublic | BindingFlags.Static);

            if (getTableNameMethod == null)
                throw new ArgumentOutOfRangeException($"Method '{getTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.");

            return getTableNameMethod.Invoke(null, new object[] { typeof(T) }) as string;
        }

        public List<T> GetByFilter(TFilter filter, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            string sqlProperties = CreateSqlProperties(typeof(T));
            string name = GetTableName();
            string filterProperties = CreateFilterProperties(filter);
            string sql = @$"SELECT  TOP 1000
                        {sqlProperties}
                        FROM {name}
                        {filterProperties}";

            return DapperUtil.Query<T>(sql, filter).ToList();
        }

        public ListResult<T> GetList(QueryParameter<TFilter> queryParameter)
        {

            StringBuilder sql = new StringBuilder();
            string sqlProperties = CreateSqlProperties(typeof(T));
            string filterSql = CreateFilterProperties(queryParameter.Filter);
            string name = GetTableName();
            string sortSql = ParseOrderBy(queryParameter.Sort);

            if (queryParameter.IncludeRowCount)
                sql.Append($@"SELECT Count FROM (SELECT COUNT(*) Count FROM {name} t {filterSql}) a;");

            sql.Append($@" 
SELECT *
FROM (
    SELECT ROW_NUMBER() OVER ({sortSql}) AS RowNumber, * 
    FROM (
		SELECT	
        {sqlProperties}
		FROM   {name}        
        {filterSql}
	) tmp1
) AS tmp2
WHERE RowNumber BETWEEN @RowNumber AND @RowNumber + {RowsPerListAddition};");

            ListResult<T> result = null;
            DapperUtil.QueryMultiple(sql.ToString(), (GridReader gr) =>
            {
                result = new ListResult<T>()
                {
                    Count = queryParameter.IncludeRowCount ? gr.Read<int>().First() : 0,
                    RowList = gr.Read<T>().ToList()
                };
            }, queryParameter.Filter);

            if (result == null)
                result = new ListResult<T>();

            return result;
        }
        private static List<PropertyInfo> TypeProperties(Type type)
        {
            return type.GetProperties().ToList();
        }

        private static List<PropertyInfo> ComputedProperties(Type type)
        {
            var computedProperties = type.GetProperties().Where(p => p.GetCustomAttributes(true).Any(a => a is ComputedAttribute)).ToList();

            return computedProperties;
        }
        private static List<PropertyInfo> KeyProperties(Type type)
        {
            var keyProperties = type.GetProperties().Where(p => p.GetCustomAttributes(true).Any(a => a is KeyAttribute)).ToList();

            return keyProperties;
        }
        private static string CreateFilterProperties(TFilter filter)
        {
            var typeFilter = typeof(TFilter);
            List<PropertyInfo> filterProperties = TypeProperties(typeFilter);
            List<PropertyInfo> filterkeyProperties = KeyProperties(typeFilter);
            List<PropertyInfo> filtercomputedProperties = ComputedProperties(typeFilter);
            List<PropertyInfo> filterPropertiesExceptKeyAndComputed = filterProperties.Except(filterkeyProperties.Union(filtercomputedProperties)).ToList();


            StringBuilder filterSql = new StringBuilder();
            StringBuilder sbParameterList = new StringBuilder(null);
            for (var i = 0; i < filterPropertiesExceptKeyAndComputed.Count; i++)
            {
                var property = filterPropertiesExceptKeyAndComputed[i];
                if (property.GetValue(filter) == null)
                {
                    continue;
                }
                if (property.PropertyType == typeof(int[]))
                {
                    filterSql.Append($" AND {property.Name.TrimEnd('s')} in @{property.Name}");
                }
                else
                {
                    filterSql.Append($" AND {property.Name} = @{property.Name}");
                }
                sbParameterList.AppendFormat("@{0}", property.Name);
                //if (i < filterPropertiesExceptKeyAndComputed.Count - 1)
                //    sbParameterList.Append(", ");
            }


            if (filterSql.Length > 0)
                filterSql.Remove(0, 4).Insert(0, " WHERE ");
            return filterSql.ToString();
        }

        private string CreateSqlProperties(Type type, bool ExceptKeyAndComputedParameters = false)
        {
            StringBuilder sql = new StringBuilder();
            List<PropertyInfo> allProperties = TypeProperties(type);

            if (ExceptKeyAndComputedParameters)
            {
                List<PropertyInfo> keyProperties = KeyProperties(type);
                List<PropertyInfo> computedProperties = ComputedProperties(type);
                allProperties = allProperties.Except(keyProperties.Union(computedProperties)).ToList();
            }


            for (var i = 0; i < allProperties.Count; i++)
            {
                var property = allProperties[i];
                sql.Append(property.Name);
                if (i < allProperties.Count - 1)
                    sql.AppendLine(", ");
            }
            return sql.ToString();
        }

       
    }

    public abstract class RepositoryBaseWithDto<T, TFilter, TDto>
          where T : class, IEntity
        where TFilter : class, IFilter
    {
        public const int RowsPerListAddition = 30;
        //private bool _hasDto = false;

        public virtual string ParseOrderBy(string raw, string defaultColumn = "ID", params string[] allowedColumns)
        {
            if (raw == null)
                return $"ORDER BY {defaultColumn} DESC";

            if (allowedColumns == null)
                throw new ArgumentNullException(nameof(allowedColumns));
            string direction = "DESC";
            string column = raw;
            if ((raw.EndsWith("_ASC", StringComparison.OrdinalIgnoreCase) || raw.EndsWith("_DESC", StringComparison.OrdinalIgnoreCase)))
            {
                direction = raw.EndsWith("_ASC", StringComparison.OrdinalIgnoreCase) ? "ASC" : "DESC";
                column = raw.Substring(0, raw.Length - (direction == "ASC" ? 4 : 5));
            }

            if (allowedColumns.Contains(column))
                return $"ORDER BY {column} {direction}";

            return $"ORDER BY {defaultColumn} DESC";
        }

        public virtual long? Create(T entity, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null) => DapperUtil.Insert(entity, connection, transaction, commandTimeout);

        public virtual T Read(object id, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null) => DapperUtil.Get<T>(id, connection, transaction, commandTimeout);

        public virtual bool? Update(T entity, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null) => DapperUtil.Update(entity, connection, transaction, commandTimeout);

        public bool? UpdatePartial<TUpdateProperties>(T entity, System.Linq.Expressions.Expression<Func<T, TUpdateProperties>> func, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null)
           where TUpdateProperties : class
        {
            
            string sqlProperties = CreatePartialProperties(func.ReturnType);
            string name = GetTableName();
            string sql = @$"UPDATE {name}
                        SET {sqlProperties}
                        WHERE ID = @ID";
            return DapperUtil.Execute(sql, entity, connection, transaction, commandTimeout ) != -2;
        }
            //=> DapperUtil.UpdatePartial(entity, func, connection, transaction, commandTimeout);

        public virtual bool? Delete(T entity, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null) => DapperUtil.Delete(entity, connection, transaction, commandTimeout);

        public virtual IEnumerable<T> GetAll(IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null) => DapperUtil.GetAll<T>(connection, transaction, commandTimeout);



        protected string GetTableName()
        {
            // Check if we've already set our custom table mapper to TableNameMapper.
            if (SqlMapperExtensions.TableNameMapper != null)
                return SqlMapperExtensions.TableNameMapper(typeof(T));

            // If not, we can use Dapper default method "SqlMapperExtensions.GetTableName(Type type)" which is unfortunately private, that's why we have to call it via reflection.
            string getTableName = "GetTableName";
            MethodInfo getTableNameMethod = typeof(SqlMapperExtensions).GetMethod(getTableName, BindingFlags.NonPublic | BindingFlags.Static);

            if (getTableNameMethod == null)
                throw new ArgumentOutOfRangeException($"Method '{getTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.");

            return getTableNameMethod.Invoke(null, new object[] { typeof(T) }) as string;
        }

        public virtual List<T> GetByFilter(TFilter filter, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            string sqlProperties = CreateSqlProperties(typeof(T));
            string name = GetTableName();
            string filterProperties = CreateFilterProperties(filter);
            string sql = @$"SELECT TOP 1000
                        {sqlProperties}
                        FROM {name}
                        {filterProperties}";

            return DapperUtil.Query<T>(sql, filter).ToList();
        }

        public virtual List<TDto> GetDtoByFilter(TFilter filter, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {

            string sqlProperties = CreateSqlProperties(typeof(TDto));
            string name = GetTableName();
            string filterProperties = CreateFilterProperties(filter);
            string sql = @$"SELECT TOP 1000
                        {sqlProperties}
                        FROM {name}
                        {filterProperties}";

            return DapperUtil.Query<TDto>(sql, filter).ToList();
        }
        public virtual ListResult<T> GetList(QueryParameter<TFilter> queryParameter)
        {

            StringBuilder sql = new StringBuilder();
            string filterSql = CreateFilterProperties(queryParameter.Filter);
            string sqlProperties = CreateSqlProperties(typeof(T));
            string name = GetTableName();
            string sortSql = ParseOrderBy(queryParameter.Sort);


            if (queryParameter.IncludeRowCount)
                sql.Append($@"SELECT Count FROM (SELECT COUNT(*) Count FROM Categories c {filterSql}) a;");

            sql.Append($@" 
SELECT *
FROM (
    SELECT ROW_NUMBER() OVER ({sortSql}) AS RowNumber, * 
    FROM (
		SELECT	
        {sqlProperties}
		FROM   {name}        
        {filterSql}
	) tmp1
) AS tmp2
WHERE RowNumber BETWEEN @RowNumber AND @RowNumber + {RowsPerListAddition};");

            ListResult<T> result = null;
            DapperUtil.QueryMultiple(sql.ToString(), (GridReader gr) =>
            {
                result = new ListResult<T>()
                {
                    Count = queryParameter.IncludeRowCount ? gr.Read<int>().First() : 0,
                    RowList = gr.Read<T>().ToList()
                };
            }, queryParameter.Filter);

            if (result == null)
                result = new ListResult<T>();

            return result;
        }

        public virtual ListResult<TDto> GetDtoList(QueryParameter<TFilter> queryParameter)
        {

            StringBuilder sql = new StringBuilder();
            string name = GetTableName();
            string filterSql = CreateFilterProperties(queryParameter.Filter);
            string sqlProperties = CreateSqlProperties(typeof(TDto));
            string sortSql = ParseOrderBy(queryParameter.Sort);

            if (queryParameter.IncludeRowCount)
                sql.Append($@"SELECT Count FROM (SELECT COUNT(*) Count FROM {name} t {filterSql}) a;");

            sql.Append($@" 
SELECT *
FROM (
    SELECT ROW_NUMBER() OVER ({sortSql}) AS RowNumber, * 
    FROM (
		SELECT	
        {sqlProperties}
		FROM   {name}        
        {filterSql}
	) tmp1
) AS tmp2
WHERE RowNumber BETWEEN @RowNumber AND @RowNumber + {RowsPerListAddition};");

            ListResult<TDto> result = null;
            DapperUtil.QueryMultiple(sql.ToString(), (GridReader gr) =>
            {
                result = new ListResult<TDto>()
                {
                    Count = queryParameter.IncludeRowCount ? gr.Read<int>().First() : 0,
                    RowList = gr.Read<TDto>().ToList()
                };
            }, queryParameter.Filter);

            if (result == null)
                result = new ListResult<TDto>();

            return result;
        }


        public virtual T GetByID(dynamic id, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null) => DapperUtil.Get<T>(id, connection, transaction, commandTimeout);

        private static List<PropertyInfo> TypeProperties(Type type)
        {
            return type.GetProperties().ToList();
        }

        private static List<PropertyInfo> ComputedProperties(Type type)
        {
            var computedProperties = type.GetProperties().Where(p => p.GetCustomAttributes(true).Any(a => a is ComputedAttribute)).ToList();

            return computedProperties;
        }
        private static List<PropertyInfo> KeyProperties(Type type)
        {
            var keyProperties = type.GetProperties().Where(p => p.GetCustomAttributes(true).Any(a => a is KeyAttribute)).ToList();

            return keyProperties;
        }

        protected  string CreateFilterProperties(TFilter filter)
        {
            var typeFilter = typeof(TFilter);
            List<PropertyInfo> filterProperties = TypeProperties(typeFilter);
            List<PropertyInfo> filterkeyProperties = KeyProperties(typeFilter);
            List<PropertyInfo> filtercomputedProperties = ComputedProperties(typeFilter);
            List<PropertyInfo> filterPropertiesExceptKeyAndComputed = filterProperties.Except(filterkeyProperties.Union(filtercomputedProperties)).ToList();


            StringBuilder filterSql = new StringBuilder();
            StringBuilder sbParameterList = new StringBuilder(null);
            for (var i = 0; i < filterPropertiesExceptKeyAndComputed.Count; i++)
            {
                var property = filterPropertiesExceptKeyAndComputed[i];
                if (property.GetValue(filter) == null)
                {
                    continue;
                }
                filterSql.Append($" AND {property.Name} = @{property.Name}");
                sbParameterList.AppendFormat("@{0}", property.Name);
                if (i < filterPropertiesExceptKeyAndComputed.Count - 1)
                    sbParameterList.Append(", ");
            }


            if (filterSql.Length > 0)
                filterSql.Remove(0, 4).Insert(0, " WHERE ");
            return filterSql.ToString();
        }

        protected static string CreatePartialProperties(Type partialType)
        {
            List<PropertyInfo> partialProperties = TypeProperties(partialType);


            StringBuilder partial = new StringBuilder();
            for (var i = 0; i < partialProperties.Count; i++)
            {
                var property = partialProperties[i];
                partial.Append($" {property.Name} = @{property.Name}");

                if (i < partialProperties.Count - 1)
                    partial.AppendLine(", ");
            }
            return partial.ToString();
        }
        private string CreateSqlProperties(Type type, bool ExceptKeyAndComputedParameters = false)
        {
            StringBuilder sql = new StringBuilder();
            List<PropertyInfo> allProperties = TypeProperties(type);

            if (ExceptKeyAndComputedParameters)
            {
                List<PropertyInfo> keyProperties = KeyProperties(type);
                List<PropertyInfo> computedProperties = ComputedProperties(type);
                allProperties = allProperties.Except(keyProperties.Union(computedProperties)).ToList();
            }


            for (var i = 0; i < allProperties.Count; i++)
            {
                var property = allProperties[i];
                sql.AppendLine(property.Name);
                if (i < allProperties.Count - 1)
                    sql.Append(", ");
            }
            return sql.ToString();
        }
    }
}
