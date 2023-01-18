using Core.BaseResults;
using Core.DataAccess.Dapper;
using Core.DataAccess.Repositories;
using Core.Entities.Concrete;
using Core.Entities.DTOs;
using DataAccess.Abstract;
using System.Linq;
using System.Text;
using System.Data;
using static Dapper.SqlMapper;
using Core.DataAccess.Abstract;
using System.Collections.Generic;
using System;

namespace DataAccess.Concrete.Dapper
{
    public class DpProductDal : RepositoryBaseWithDto<Product, ProductFilter, ProductViewDto>, IProductDal
    {

   //     public override List<Product> GetByFilter(ProductFilter filter, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null)
   //     {

			//string filterQuery = base.CreateFilterProperties(filter);
   //     }
    }
}          
        