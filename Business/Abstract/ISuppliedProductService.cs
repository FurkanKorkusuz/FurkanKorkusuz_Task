using Core.BaseResults;
using Core.DataAccess.Dapper;
using Core.Entities.Concrete;
using Core.Entities.DTOs;
using System.Collections.Generic;
using Core.Utilities.Business;

namespace Business.Abstract
{
    public interface ISuppliedProductService
    {
        List<SuppliedProduct> GetByFilter(SuppliedProductFilter filter);
    }
}

        