using Core.BaseResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Business
{
    public class BusinessRules
    {
        /// <summary>
        /// Business te birden fazla iş kuralımız olduğunda, bu iş kurallarından herhangi biri için hata oluşursa hatayı döndürsün.
        /// Hiçbirinden hata dönmez ise null dönsün.
        /// Örnek kullanım:
        ///  IResult result= BusinessRules.Run( 
        ///        CheckIfProductNameExists(product.ProductName), // ürün adının sistemde olup olmadığını kontrol ediyor.
        ///        CheckIfCategoryIsEnabled()      // Kategorinin açık olup olmadığını kontrol ediyo
        ///        );
        /// </summary>
        /// <param name="logics">IResult türünde dönüşü olan fonksiyonlar.</param>
        /// <returns>null veya IResult türündeki hatalı result.</returns>
        public static BaseResult Run(params BaseResult[] logics)
        {
            foreach (var result in logics)
            {
                if (!result.Success)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
