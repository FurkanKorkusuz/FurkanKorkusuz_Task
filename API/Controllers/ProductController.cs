using Business.Abstract;
using Core.Entities.Concrete;
using Core.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using Core.BaseResults;

namespace API.Controllers
{
    public class ProductController : Controller
    {
        private IEfProductService _productService;
        public ProductController(IEfProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("get-all")]
        public IActionResult GetAll()
        {
            return Json(_productService.GetAll());
        }

        //[HttpPost("get-for-list")]
        //public IActionResult GetForList(Expression<Func<Product, bool>>[] predicates, Expression<Func<Product, string>> sort, bool desc, int page, int pageSize, int totalRecords)
        //{
        //   List<Product> list = _productService.GetForList(predicates, sort, desc, page, pageSize, out totalRecords);
        //    return Json(new {List = list, Total = totalRecords});
        //}


        [HttpPost("add")]
        public IActionResult Add(Product product)
        {
            return Json(_productService.Add(product));
        }


        [HttpPost("update")]
        public IActionResult Update(Product product)
        {
            _productService.Update(product);
            return Json(new BaseResult());
        }

        [HttpPost("delete")]
        public IActionResult Delete(int id)
        {
            _productService.Delete(id);
            return Json(new BaseResult());
        }
    }
}
