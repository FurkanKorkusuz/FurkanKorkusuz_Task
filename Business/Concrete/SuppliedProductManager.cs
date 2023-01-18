using Business.Abstract;
using Core.BaseResults;
using Core.Entities.Concrete;
using Core.Entities.DTOs;
using Core.Utilities.Business;
using System.Collections.Generic;
using DataAccess.Abstract;
using Core.DataAccess.Dapper;
using Core.Aspects.Autofac.Authorization;
using Core.Aspects.Autofac.Caching;
using System.Linq.Expressions;
using System;
using System.Linq;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

namespace Business.Concrete
{
    public class SuppliedProductManager : ISuppliedProductService
    {
        ISuppliedProductDal _suppliedProductDal;
        public SuppliedProductManager(
        ISuppliedProductDal suppliedProductDal
        )
        {
            _suppliedProductDal = suppliedProductDal;
        }

        [SecuredOperation()]
        [CacheAspect()]
        public List<SuppliedProduct> GetByFilter(SuppliedProductFilter filter)
        {
            List<SuppliedProductFilter> suppliedProductFilters= new List<SuppliedProductFilter>();
            Test(filter=>filter.ProductID == filter.ProductID);

            Func<SuppliedProductFilter, bool> expression = filter => filter.ProductID == filter.ProductID;



            return _suppliedProductDal.GetByFilter(filter, expression);
        }

        private void Test(Func<SuppliedProductFilter, bool> expression)
        {
            int a = 3;
            
        }

    }
}            
        