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
using Dapper = Dapper;
namespace DataAccess.Concrete.Dapper
{
    public class DpSuppliedProductDal : ISuppliedProductDal
    {
        public List<SuppliedProduct> GetByFilter(SuppliedProductFilter filter, Func<SuppliedProductFilter, bool> expression)
        {
            string filterSql = FilterHelper<SuppliedProductFilter>.CreateSqlFilter(filter).Replace("WHERE", "AND");
            string srtSql = @$"
                SELECT 	op.ProductID,
	            op.CombinationID,
	            SUM( (op.Quantity - op.QtyShipped) - (op.Booked + op.BookedWithTransport + op.BookedWithTurkiye)) AS SuppliedCount
	            FROM OrderProducts op 
	            JOIN Orders o on op.OrderID = o.OrderID
	            JOIN Products p on p.ProductID = op.ProductID
	            WHERE o.Status IN (3, 8, 10, 11) -- Order Status
                AND p.SupplierID = @SupplierID
	            AND op.Date < DATEADD(MINUTE, 30, GETDATE()) -- 30 minute before orders (for booked complate)
	            AND (op.Quantity - op.QtyShipped) > (op.Booked + op.BookedWithTransport + op.BookedWithTurkiye) -- (order - shipped) - (reserved) = quantity to be supplied
                {filterSql}
	            GROUP BY op.ProductID, op.CombinationID
            ";

            return DapperUtil.Query<SuppliedProduct>(srtSql, filter).ToList();
        }

       

    }
}          
        