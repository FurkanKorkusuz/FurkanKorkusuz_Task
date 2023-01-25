using Core.DataAccess.Abstract;
using Core.Entities.Concrete;
using DataAccess.Concrete.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EF.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Furkan_TaskDBContext _context;

        public UnitOfWork(Furkan_TaskDBContext context)
        {
            _context = context;
            //ProductRepository = new EfProductDal(_context);
            //CategoryRepository = new EfGenericRepository<Category>(_context);
        }

        public IRepository<Product> ProductRepository { get; }
        //public IRepository<Category> CategoryRepository { get; }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
