using Business.Abstract;
using Core.BaseResults;
using Core.Entities.Concrete;
using Core.Entities.DTOs;
using Core.Utilities.Business;
using System.Collections.Generic;
using DataAccess.Abstract;
using Core.DataAccess.Dapper;
using Core.Aspects.Autofac.Caching;
using System.Linq.Expressions;
using System.Reflection;
using System;
using Core.Aspects.Autofac.Authorization;
using Core.Aspects.Autofac.Performance;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using static Slapper.AutoMapper;
using Core.Aspects.Autofac.Validation;
using Business.ValidationRules.FluentValidation;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Core.Extensions;

namespace Business.Concrete
{
    public class NewManager : INewService
    {
        INewDal _newDal;
        IHttpContextAccessor _httpContextAccessor;
        public NewManager(INewDal newDal)
        {
            _newDal = newDal;
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        [PerformanceAspect(1)]
        public EntityResult<List<New>> GetList()
        {
            int userid = _httpContextAccessor.HttpContext.User.GetUserID();
            return new EntityResult<List<New>> { Entity = _newDal.Find(n=>n.UserID==userid) };
        }

        [PerformanceAspect(1)]
        public EntityResult<New> GetByID(int id)
        {

            int userid = _httpContextAccessor.HttpContext.User.GetUserID();
            New @new = _newDal.GetByID(id);
            if (@new.UserID == userid)
            {
                return new EntityResult<New> { Entity = @new };
            }
            return new EntityResult<New> { Error = "Bu haber baþka bir kullanýcýya ait." };

        }

        [PerformanceAspect(1)]
        public EntityResult<List<New>> Find(Expression<Func<New, bool>> predicate)
        {
            int userid = _httpContextAccessor.HttpContext.User.GetUserID();
            return new EntityResult<List<New>> { Entity = _newDal.Find(predicate).Where(n => n.UserID == userid).ToList() };
        }


        [ValidationAspect(typeof(NewAddDtoValidator))]
        [CacheRemoveAspect(true)]
        public EntityResult<New> Add(NewAddDto entity)
        {
            New @new = new New
            {
                UserID = _httpContextAccessor.HttpContext.User.GetUserID(),
                CreatedDate = DateTime.Now,
                Content = entity.Content,
                Summary= entity.Summary,
                IsActive= entity.IsActive,
                UrlLink= entity.UrlLink,
                
            };
            return new EntityResult<New>
            {
                Entity = _newDal.Add(@new)
            };
        }


        [ValidationAspect(typeof(NewAddDtoValidator))]
        [CacheRemoveAspect(true)]
        public BaseResult Update(NewAddDto entity)
        {
            New @new = GetByID(entity.ID).Entity;
            if (@new.UserID != _httpContextAccessor.HttpContext.User.GetUserID())
            {
                return new BaseResult { Error = "Bu haber baþka bir kullanýcýya ait." };
            }

            @new.IsActive = entity.IsActive;
            @new.Summary = entity.Summary;
            @new.Content = entity.Content;

            _newDal.Update(@new);
            return new BaseResult();
        }

        [CacheRemoveAspect(true)]
        public BaseResult Delete(int id)
        {
            New @new = GetByID(id).Entity;
            if (@new== null)
            {
                return new BaseResult { Error = "Haber bulunamadý." };
            }
            if (@new.UserID != _httpContextAccessor.HttpContext.User.GetUserID())
            {
                return new BaseResult { Error="Bu haber baþka bir kullanýcýya ait."};
            }

            @new.IsDeleted = true;
            @new.DeletedDate =DateTime.Now;

            _newDal.Update(@new);
            return new BaseResult();
        }
    }
}
