using Castle.Core;
using Core.Aspects.Autofac.Authorization;
using Core.Aspects.Autofac.Caching;
using Core.BaseResults;
using Core.DataAccess;
using Core.DataAccess.Abstract;
using Core.DataAccess.Dapper;
using Core.Entities;
using Core.Entities.Abstract;
using Core.Entities.Concrete;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Business
{
    public class BaseEntityManager<TEntity,TFilter> : IEntityService<TEntity,TFilter>
        where TEntity : class, IEntity, new()
        where TFilter : class, IFilter, new()


    {
        IEntityRepository<TEntity, TFilter> _dal;

        public BaseEntityManager(IEntityRepository<TEntity, TFilter> dal)
        {
            _dal = dal;
        }

        [CacheRemoveAspect(true)]
        [SecuredOperation()]
        public virtual TEntity Add(TEntity entity)
        {
            PropertyInfo keyColumn = GetIdentityColumn(typeof(TEntity));
            int id = (int)_dal.Create(entity);
            typeof(TEntity).GetProperty(keyColumn.Name).SetValue(entity, id);
            return entity;
        }


        [CacheRemoveAspect(true)]
        [SecuredOperation()]
        public virtual bool Delete(TEntity entity)
        {
           return _dal.Delete(entity) ?? false;
        }



        [SecuredOperation()]
        public virtual TEntity GetByID(int id)
        {
            return _dal.GetByID(id);
        }


        [CacheAspect()]
        [SecuredOperation()]
        public virtual ListResult<TEntity> GetList(QueryParameter<TFilter> queryParameter)
        {
            return _dal.GetList(queryParameter);
        }



        [CacheRemoveAspect(true)]
        [SecuredOperation()]
        public virtual bool Update(TEntity entity)
        {
            return _dal.Update(entity) ?? false;
        }


        [CacheAspect()]
        [SecuredOperation()]
        public List<TEntity> GetByFilter(TFilter filter)
        {
            return _dal.GetByFilter(filter);
        }



        [CacheRemoveAspect(true)]
        [SecuredOperation()]
        public bool UpdatePartial<TUpdateProperties>(TEntity entity, Expression<Func<TEntity, TUpdateProperties>> func) where TUpdateProperties : class
        {
            return _dal.UpdatePartial(entity, func) ?? false;
        }



        private PropertyInfo GetIdentityColumn(Type entityType)
        {
            return entityType.GetProperties().Where(p => p.GetCustomAttributes(true).Any(a => a is KeyAttribute)).FirstOrDefault();
        }
    }


    public class BaseEntityManagerWithDto<TEntity, TFilter, TDto> : BaseEntityManager<TEntity,TFilter>, IEntityServiceWithDto<TEntity, TFilter, TDto>
       where TEntity : class, IEntity, new()
       where TFilter : class, IFilter, new()


    {
        IEntityRepositoryWithDto<TEntity, TFilter, TDto> _dal;

        public BaseEntityManagerWithDto(IEntityRepositoryWithDto<TEntity, TFilter, TDto> dal):base(dal) 
        {
            _dal = dal;

        }


        [CacheAspect()]
        [SecuredOperation()]
        public virtual ListResult<TDto> GetDtoList(QueryParameter<TFilter> queryParameter)
        {
            return _dal.GetDtoList(queryParameter);
        }

        [CacheAspect()]
        [SecuredOperation()]
        public virtual List<TDto> GetDtoByFilter(TFilter filter)
        {
            return _dal.GetDtoByFilter(filter);
        }


        [CacheRemoveAspect(true)]
        [SecuredOperation()]
        public virtual TEntity AddWithDto(TDto entity)
        {
            throw new NotImplementedException();
        }


        [CacheRemoveAspect(true)]
        [SecuredOperation()]
        public virtual bool UpdateWithDto(TDto entity)
        {
            throw new NotImplementedException();
        }


    }
}
