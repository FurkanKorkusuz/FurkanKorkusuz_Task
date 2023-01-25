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
        Product Add(Product entity);
        void Delete(int id);
        List<Product> GetAll();
        List<Product> Find(Expression<Func<Product, bool>>[] predicates);
        List<Product> GetForList(Expression<Func<Product, bool>>[] predicates, Expression<Func<Product, string>> sort, bool desc, int page, int pageSize, out int totalRecords);
        Product GetById(int id);
        void Update(Product entity);
    }
}

        