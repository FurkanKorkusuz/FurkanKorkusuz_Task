using Core.DataAccess.Abstract;
using Core.Entities.Concrete;
using Core.Entities.DTOs;
using Core.BaseResults;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.DataAccess.Dapper;
using Core.DataAccess.EF;
using Core.Utilities.Business;

namespace DataAccess.Abstract
{
    public interface INewDal : IEfGenericRepository<New>
    {
        
    }
}


        