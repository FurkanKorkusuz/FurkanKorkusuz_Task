using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Abstract
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        void Delete(int id);
        List<T> GetAll();
        List<T> Find(Expression<Func<T, bool>>[] predicates);
        List<T> GetForList(Expression<Func<T, bool>>[] predicates, Expression<Func<T, string>> sort, bool desc, int page, int pageSize, out int totalRecords);
        T GetById(int id);
        void Update(T entity);
    }
}
