using Core.DataAccess.Abstract;
using Core.DataAccess.Connections;
using Dapper;
using Dapper.Contrib.Extensions;
using DapperExtensions.Mapper;
using MiniProfiler.Integrations;
using StackExchange.Profiling.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Core.DataAccess.Dapper
{
    /// <summary>
    /// Uses Dapper.Contrib.Extensions
    /// </summary>
    public static partial class DapperUtil
    {
        private static string _connectionString;

        private static string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                    _connectionString = SqlConnectionTools.ConnectionString();

                return _connectionString; 
            }
        }

        public static IDbConnection DbConnectionProfiled(out CustomDbProfiler profiler)
        {
            profiler = new CustomDbProfiler();
            IDbConnection connection = ProfiledDbConnectionFactory.New(new SqlServerDbConnectionFactory(ConnectionString), profiler);

            return connection;
        }

        public static IDbConnection DbConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static void Config()
        {
            // https://github.com/tmsmith/Dapper-Extensions/wiki/AutoClassMapper#pluralizedautoclassmapper
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(PluralizedAutoClassMapper<>);
        }

        /// <summary>
        /// Executes a query, returning the data typed as <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of results to return.</typeparam>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="param">The parameters to pass, if any.</param>
        /// <param name="connection">Open connection to query on.</param>
        /// <param name="transaction">The transaction to use, if any.</param>
        /// <param name="buffered">Whether to buffer results in memory.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <param name="commandType">The type of command to execute.</param>
        /// <returns>
        /// A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first column is assumed, otherwise an instance is
        /// created per row, and a direct column-name===member-name mapping is assumed (case insensitive), or null if an SqlException occurred.
        /// </returns>
        public static IEnumerable<T> Query<T>(string sql, object param = null, IDbConnection connection = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            IDbConnection cn = connection ?? transaction?.Connection;


                if (cn != null)
                {
                    return cn.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
                }
                else
                {
                    using (cn = DbConnection())
                        return cn.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
                }

        }

        /// <summary>
        /// Inserts an entity into table "Ts" and returns identity id or number of inserted rows if inserting a list.
        /// </summary>
        /// <typeparam name="T">The type to insert.</typeparam>
        /// <param name="entity">Entity to insert, can be list of entities</param>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>Identity of inserted entity, or number of inserted rows if inserting a list, or null if an SqlException occurred.</returns>
        public static long? Insert<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null)
             where T : class
        {
            IDbConnection cn = connection ?? transaction?.Connection;
            CustomDbProfiler profiler = null;

                if (cn != null)
                {
                    profiler = GetProfiler(cn);
                    return cn.Insert(entity, transaction, commandTimeout);
                }
                else
                {
                    using (cn = DbConnectionProfiled(out profiler))
                        return cn.Insert(entity, transaction, commandTimeout);              
           
                }


        }



        /// <summary>
        /// Returns a single entity by a single id from table "Ts".  
        /// Id must be marked with [Key] attribute.
        /// Entities created from interfaces are tracked/intercepted for changes and used by the Update() extension
        /// for optimal performance. 
        /// </summary>
        /// <typeparam name="T">Interface or type to create and populate</typeparam>
        /// <param name="id">Id of the entity to get, must be marked with [Key] attribute</param>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>Entity of T, or null if an SqlException occurred.</returns>
        public static T Get<T>(object id, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null)
             where T : class
        {
            IDbConnection cn = connection ?? transaction?.Connection;
            CustomDbProfiler profiler = null;

                if (cn != null)
                {
                    profiler = GetProfiler(cn);
                    return cn.Get<T>(id, transaction, commandTimeout);
                }
                else
                {
                    using (cn = DbConnectionProfiled(out profiler))
                        return cn.Get<T>(id, transaction, commandTimeout);
                }

        }


        /// <summary>
        /// Returns a list of entities from table "Ts".
        /// Id of T must be marked with [Key] attribute.
        /// Entities created from interfaces are tracked/intercepted for changes and used by the Update() extension
        /// for optimal performance.
        /// </summary>
        /// <typeparam name="T">Interface or type to create and populate</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>Entity of T, or null if an SqlException occurred.</returns>
        public static IEnumerable<T> GetAll<T>(IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null)
            where T : class
        {
            IDbConnection cn = connection ?? transaction?.Connection;
            CustomDbProfiler profiler = null;

                if (cn != null)
                {
                    profiler = GetProfiler(cn);
                    return cn.GetAll<T>(transaction, commandTimeout);
                }
                else
                {
                    using (cn = DbConnectionProfiled(out profiler))
                        return cn.GetAll<T>(transaction, commandTimeout);
                }
        }



        /// <summary>
        /// Updates entity in table "Ts", checks if the entity is modified if the entity is tracked by the Get() extension.
        /// </summary>
        /// <typeparam name="T">Type to be updated</typeparam>
        /// <param name="entity">Entity to be updated</param>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>true if updated, false if not found or not modified (tracked entities), or null if an SqlException occurred.</returns>
        public static bool? Update<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null)
             where T : class
        {
            IDbConnection cn = connection ?? transaction?.Connection;
            CustomDbProfiler profiler = null;

                if (cn != null)
                {
                    profiler = GetProfiler(cn);
                    return cn.Update(entity, transaction, commandTimeout);
                }
                else
                {
                    using (cn = DbConnectionProfiled(out profiler))
                        return cn.Update(entity, transaction, commandTimeout);
                }


        }



        /// <summary>
        /// Delete entity in table "Ts".
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="entity">Entity to delete</param>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>true if deleted, false if not found, or null if an SqlException occurred.</returns>
        public static bool? Delete<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null)
             where T : class
        {
            IDbConnection cn = connection ?? transaction?.Connection;
            CustomDbProfiler profiler = null;

                if (cn != null)
                {
                    profiler = GetProfiler(cn);
                    return cn.Delete(entity, transaction, commandTimeout);
                }
                else
                {
                    using (cn = DbConnectionProfiled(out profiler))
                        return cn.Delete(entity, transaction, commandTimeout);
                }

        }

  

        /// <summary>
        /// Executes a single-row query, returning the data typed as <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of result to return.</typeparam>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="param">The parameters to pass, if any.</param>
        /// <param name="connection">The open connection to query on.</param>
        /// <param name="transaction">The transaction to use, if any.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <param name="commandType">The type of command to execute.</param>
        /// <returns>
        /// A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first column is assumed, otherwise an instance is
        /// created per row, and a direct column-name===member-name mapping is assumed (case insensitive), or the default value of T (null) if an SqlException occurred.
        /// </returns>
        public static T QueryFirstOrDefault<T>(string sql, object param = null, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            IDbConnection cn = connection ?? transaction?.Connection;
      
                if (cn != null)
                {
                    return cn.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
                }
                else
                {
                    using (cn = DbConnection())
                        return cn.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
                }

        }



        /// <summary>
        /// Execute a command that returns multiple result sets, and access each in turn.
        /// </summary>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="callback">Receives the resulting GridReader as first parameter, while the connection is still open.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="connection">The open connection to query on.</param>
        /// <param name="transaction">The transaction to use for this query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        public static void QueryMultiple(string sql, Action<GridReader> callback, object param = null, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            IDbConnection cn = connection ?? transaction?.Connection;
          
                if (cn != null)
                {
                    callback(cn.QueryMultiple(sql, param, transaction, commandTimeout, commandType));
                }
                else
                {
                    using (cn = DbConnection())
                        callback(cn.QueryMultiple(sql, param, transaction, commandTimeout, commandType));
                }
            

        }

        /// <summary>
        /// Execute parameterized SQL.
        /// </summary>
        /// <param name="sql">The SQL to execute for this query.</param>
        /// <param name="param">The parameters to use for this query.</param>
        /// <param name="connection">The connection to query on.</param>
        /// <param name="transaction">The transaction to use for this query.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
        /// <param name="commandType">Is it a stored proc or a batch?</param>
        /// <returns>The number of rows affected, or -2 if an SqlException occurred.</returns>
        public static int Execute(string sql, object param = null, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            IDbConnection cn = connection ?? transaction?.Connection;

            if (cn != null)
            {
                return cn.Execute(sql, param, transaction, commandTimeout, commandType);
            }
            else
            {
                using (cn = DbConnection())
                    return cn.Execute(sql, param, transaction, commandTimeout, commandType);
            }

        }


        /// <summary>
        /// Upgrades the IDbConnection to a ProfiledDbConnection if it's not, and returns 
        /// the associated CustomDbProfiler. 
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private static CustomDbProfiler GetProfiler(IDbConnection connection)
        {
            CustomDbProfiler profiler = null;
            if (connection is ProfiledDbConnection)
                profiler = ((ProfiledDbConnection)connection).Profiler as CustomDbProfiler;
            else
                connection = new ProfiledDbConnection((DbConnection)connection, profiler);

            return profiler;
        }

        /// <summary>
        /// Returns the commands associated with the profiler, otherwise null.
        /// </summary>
        /// <param name="profiler"></param>
        /// <returns></returns>
        private static string GetCommands(CustomDbProfiler profiler)
        {
            return profiler?.ProfilerContext?.GetCommands();
        }
    
    }
}
