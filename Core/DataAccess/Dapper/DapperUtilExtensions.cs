using DapperExtensions;
using MiniProfiler.Integrations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Dapper
{
    /// <summary>
    /// Uses DapperExtensions, therefore a partial class with other usings.
    /// </summary>
    public static partial class DapperUtil
    {

        /// <summary>
        /// Updates entity in table "Ts", checks if the entity is modified if the entity is tracked by the Get() extension.
        /// </summary>
        /// <typeparam name="T">Type to be updated</typeparam>
        /// <param name="entity">Entity to be updated</param>
        /// <param name="connection">Open IDbConnection</param>
        /// <param name="func">Funtion that returns an object containing the columns to update as property names,
        /// the returned object can be a dynamic object. E.g. if you only want to update property MyProperty of myEntity, you can 
        /// pass: "myEntity => new { MyProperty = "NewValue" }" or even just: "myEntity => new { myEntity.MyProperty }" to just
        /// update the column with name MyProperty
        /// using the original entity without specifying the property/column names.</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none. If it has an attached connection it will be used and assumed open if the connection parameter wasn't provided.</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>true if updated, false if not found or not modified (tracked entities) or null if an SqlException occurred</returns>
        public static bool? UpdatePartial<T, TUpdateColumns>(T entity, Expression<Func<T, TUpdateColumns>> func, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null)
             where T : class where TUpdateColumns : class
        {
            object t = DapperExtensions.DapperExtensions.DefaultMapper;

            IDbConnection cn = connection ?? transaction?.Connection;
            CustomDbProfiler profiler = null;
           
                if (cn != null)
                {
                    profiler = GetProfiler(cn);
                    return cn.UpdatePartial(entity, func, transaction, commandTimeout);
                }
                else
                {
                    using (cn = DbConnectionProfiled(out profiler))
                        return cn.UpdatePartial(entity, func, transaction, commandTimeout);
                }

        }

    }
}
