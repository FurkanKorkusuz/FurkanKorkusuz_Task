
using Business.Abstract;
using Core.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Core.DataAccess.Dapper;
using API.Controllers;
using Autofac.Core;
using System;
using Core.Entities.DTOs;

namespace API.Controllers
{

    [Route("new")]
    public class NewController : Controller
    {
        INewService _newService;

        public NewController(
            INewService newService
            )
        {
            _newService = newService;

        }

        [Route("list")]
        [HttpPost]
        public  IActionResult GetList()
        {
            return Json(_newService.GetList());
        }

        [HttpPost("add")]
        public  IActionResult Add(NewAddDto entity)
        {
            return Json(_newService.Add(entity));
        }

        [Route("get-by-id")]
        [HttpPost]
        public  IActionResult GetByID(int id)
        {
            return Json(_newService.GetByID(id));
        }

        [Route("edit")]
        [HttpPost]
        public  IActionResult Update(NewAddDto entity)
        {
            return Json(_newService.Update(entity));
        }

        [Route("delete")]
        [HttpPost]
        public  IActionResult Delete(int id)
        {
            return Json(_newService.Delete(id));
        }



    }
}

