using Core.Entities.Abstract;
using Core.Utilities.Business;
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
    public abstract class EfGenericRepository<T>  : IEfGenericRepository<T>
        where T : class, IEntity
    {
        private readonly Furkan_TaskDBContext _dbContext;
        private readonly DbSet<T> _dbSet;


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

        public virtual T GetByID(int id)
        {
            return _dbSet.Find(id);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = GetByID(id);
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }

        public virtual List<T> GetList()
        { 
            return _dbSet.AsNoTracking().ToList();
        }

        public virtual List<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

    }


}
