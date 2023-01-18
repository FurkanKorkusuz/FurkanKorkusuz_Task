using Business.Abstract;
using Core.BaseResults;
using Core.Entities.Concrete;
using Core.Entities.DTOs;
using Core.Utilities.Business;
using System.Collections.Generic;
using DataAccess.Abstract;
using Core.DataAccess.Dapper;
using Business.BusinessAspects.Autofac;
using DataAccess.Concrete.EF;
using Core.DataAccess.EF;
using System.Linq;
using System.Linq.Expressions;
using System;

namespace Business.Concrete
{
    public class ProductManager : BaseEntityManagerWithDto<Product, ProductFilter, ProductViewDto>, IProductService
    {
        IProductDal _productDal;
        public ProductManager(
            IProductDal productDal
        ) 
            : base(productDal)
        {
            _productDal = productDal;
        }

        //[SecuredOperation("")]
        //public override EntityResult<Product> GetByID(int id)
        //{
        //    return base.GetByID(id);
        //}

    }

    public class EfProductManager : IEfProductService
    {
        private readonly IEfProductDal _productDal;
        public EfProductManager(IEfProductDal productRepository)
        {
            _productDal = productRepository;
        }
        public IQueryable<Product> All()
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Product> Find(Expression<Func<Product, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Product GetByID(int id)
        {
            return _productDal.GetByID(id);
        }

        public Product Save(Product entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Product entity)
        {
            throw new NotImplementedException();
        }
    }

}
