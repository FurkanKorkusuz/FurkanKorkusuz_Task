using Business.Abstract;
using Core.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Core.DataAccess.Dapper;
using API.Controllers;
using Autofac.Core;

namespace API.Controllers
{
    
    [Route("product")]
    public class ProductController  : Controller //: BaseController<Product, ProductFilter, IProductService>
    {
        IProductService _productService;
        IEfProductService _EfproductService;

        public ProductController(
            IProductService productService,
            IEfProductService efProductService
            ) //: base(productService)
        {
            _productService = productService; 
            _EfproductService = efProductService;
        }

        [Route("get-by-filter")]
        [HttpPost]
        public virtual IActionResult GetByFilter(ProductFilter filter)
        {
            return Json(_productService.GetByFilter(filter));
        }

        [Route("get-by-id")]
        [HttpPost]
        public virtual IActionResult GetByFilter(int id)
        {
            return Json(_EfproductService.GetByID(id));
        }

    }
}

        