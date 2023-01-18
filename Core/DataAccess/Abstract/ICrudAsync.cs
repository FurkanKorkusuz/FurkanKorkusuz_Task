using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Abstract
{
    public interface ICrudAsync<T>
    {
        Task<int> CreateAsync(T entity, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<T> ReadAsync(dynamic id, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<bool> UpdateAsync(T entity, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null);

        Task<bool> UpdatePartialAsync<TUpdateColumns>(T entity, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null)
            where TUpdateColumns : class;
    }
}
