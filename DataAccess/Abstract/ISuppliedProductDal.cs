using Core.DataAccess.Abstract;
using Core.Entities.Concrete;
using Core.Entities.DTOs;
using Core.BaseResults;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.DataAccess.Dapper;

namespace DataAccess.Abstract
{
    public interface ISuppliedProductDal 
    {
        List<SuppliedProduct> GetByFilter(SuppliedProductFilter filter, Func<SuppliedProductFilter, bool> expression);
    }
}


        