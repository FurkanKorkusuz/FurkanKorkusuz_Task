using Core.BaseResults;
using Core.DataAccess.Dapper;
using Core.Entities.Concrete;
using Core.Entities.DTOs;
using System.Collections.Generic;
using Core.Utilities.Business;
using System.Linq.Expressions;
using System.Linq;
using System;

namespace Business.Abstract
{
    public interface IProductService : IEntityServiceWithDto<Product, ProductFilter, ProductViewDto>
    {
        
    }

    public interface IEfProductService 
    {
        Product Save(Product entity);
        Product GetByID(int id);
        void Update(Product entity);
        void Delete(int id);
        IQueryable<Product> All();
        IQueryable<Product> Find(Expression<Func<Product, bool>> predicate);
    }
}

        