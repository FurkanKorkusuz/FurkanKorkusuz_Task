using Core.DataAccess.EF;
using Core.Entities.Concrete;
using Core.Utilities.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EF
{
    public class EfProductDal : EfGenericRepository<Product>, IEfProductDal
    {
        public EfProductDal(Furkan_TaskDBContext dbContext) : base(dbContext)
        {
        }
    }

   public interface IEfProductDal : Core.DataAccess.Abstract.IRepository<Product>
    {

    }
}
