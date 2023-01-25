using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EF.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<Product> ProductRepository { get; }
        //IRepository<Category> CategoryRepository { get; }
        int SaveChanges();
    }
}
