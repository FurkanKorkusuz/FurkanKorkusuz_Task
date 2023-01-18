using Core.DataAccess.EF;
using Core.Entities.Concrete;
using Core.Entities.DTOs;
using DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EF
{
    public class EfNewDal : EfGenericRepository<New>, INewDal
    {
        private readonly Furkan_TaskDBContext _dbContext;
        private readonly DbSet<New> _dbSet;

        public EfNewDal(Furkan_TaskDBContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._dbSet = _dbContext.Set<New>();
        }

        public override New GetByID(int id)
        {
            return  _dbSet.Find(id);
        }
    }
}
