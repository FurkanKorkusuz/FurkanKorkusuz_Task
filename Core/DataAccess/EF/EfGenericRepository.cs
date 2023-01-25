using Core.DataAccess.Abstract;
using Core.Entities.Abstract;
using Core.Utilities.Business;
using DapperExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Core.DataAccess.EF
{
    public abstract class EfGenericRepository<T>  : Abstract.IRepository<T>
        where T : class, IEntity
    {
        private readonly Furkan_TaskDBContext _dbContext;
        private readonly DbSet<T> _dbSet;
        private readonly int _listCount = 30;


        protected EfGenericRepository(Furkan_TaskDBContext dbContext)
        {
            this._dbContext = dbContext;
            this._dbSet = _dbContext.Set<T>();
        }

        public T Add(T entity)
        {
             _dbSet.Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }

        public List<T> Find(Expression<Func<T, bool>>[] predicates)
        {

            var query = _dbSet.AsQueryable();
            foreach (var predicate in predicates)
                query = query.Where(predicate);
            return query.ToList();
        }
        public List<T> GetForList(Expression<Func<T, bool>>[] predicates, Expression<Func<T, string>> sort , bool desc, int page, int pageSize, out int totalRecords)
        {
            List<T> result = new List<T>();
            var query = _dbSet.AsQueryable();
            foreach (var predicate in predicates)
                query = query.Where(predicate);

            totalRecords = _dbSet.Count();
            int skipRows = (page - 1) * pageSize;
            if (desc)
                result = query.OrderByDescending(sort).Skip(skipRows).Take(pageSize).ToList();
            else
                result = query.OrderBy(sort).Skip(skipRows).Take(pageSize).ToList();

            return result;
        }

        public List<T> GetAll()
        {
            return _dbSet.AsEnumerable().ToList();
        }

    }


}
