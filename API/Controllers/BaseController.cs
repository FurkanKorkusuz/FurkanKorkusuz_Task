using Core.DataAccess.Dapper;
using Core.Entities.Abstract;
using Core.Utilities.Business;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    [Route("[controller]")]
    public class BaseController<TEntity, TFilter, Service> : Controller
        where TEntity : class, IEntity, new()
        where TFilter : class, IFilter, new()
        where Service : IEntityService<TEntity, TFilter>
    {
        private Service _service;
        public string _name = "";

        public BaseController(Service service)
        {
            _service = service;
            _name = typeof(TEntity).Name;
        }


        [Route("list")]
        [HttpPost]
        public virtual IActionResult GetList(QueryParameter<TFilter> queryParameter)
        {
            return Json(_service.GetList(queryParameter));
        }

        [HttpPost("add")]
        public virtual IActionResult Add(TEntity entity)
        {
            return Json(_service.Add(entity));
        }

        [Route("get-by-filter")]
        [HttpPost]
        public virtual IActionResult GetByFilter(TFilter filter)
        {
            return Json(_service.GetByFilter(filter));
        }

        [Route("get-by-id")]
        [HttpPost]
        public virtual IActionResult GetByID(int id)
        {
            return Json(_service.GetByID(Convert.ToInt32(id)));
        }

        [Route("edit")]
        [HttpPost]
        public virtual IActionResult Update(TEntity entity)
        {
            return Json(_service.Update(entity));
        }

        [Route("remove")]
        [HttpPost]
        public virtual IActionResult Delete(TEntity entity)
        {
            return Json(_service.Delete(entity));
        }

    }
}
