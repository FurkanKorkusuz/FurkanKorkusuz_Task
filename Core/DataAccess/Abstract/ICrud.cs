using Core.BaseResults;
using Core.DataAccess.Dapper;
using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Abstract
{
    public interface ICrud<TEntity, TFilter>
        where TEntity : class, IEntity
        where TFilter : class, IFilter
    {

        List<TEntity> GetByFilter(TFilter filter, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null);
        ListResult<TEntity> GetList(QueryParameter<TFilter> queryParameter);

        long? Create(TEntity entity, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null);

        bool? Update(TEntity entity, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null);
        bool? UpdatePartial<TUpdateProperties>(TEntity entity, Expression<Func<TEntity, TUpdateProperties >> func, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null) where TUpdateProperties:class ;

        TEntity GetByID(dynamic id, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null);

        bool? Delete(TEntity entity, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null);
    }
}
