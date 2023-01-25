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
using static Slapper.AutoMapper;

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


        public List<Product> GetAll()
        {
            return _productDal.GetAll();
        }

        public void Delete(int id)
        {
            _productDal.Delete(id);
        }

        public List<Product> Find(Expression<Func<Product, bool>>[] predicates)
        {
            return _productDal.Find(predicates);
        }


        public List<Product> GetForList(Expression<Func<Product, bool>>[] predicates, Expression<Func<Product, string>> sort, bool desc, int page, int pageSize, out int totalRecords)
        {
            return _productDal.GetForList(predicates,sort, desc, page, pageSize, out totalRecords);
        }

        public Product GetById(int id)
        {
            return _productDal.GetById(id);
        }

        public Product Add(Product entity)
        {
            return _productDal.Add(entity);
        }

        public void Update(Product entity)
        {
            _productDal.Update(entity);
        }
    }

}
