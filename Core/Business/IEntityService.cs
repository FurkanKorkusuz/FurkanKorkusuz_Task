using Core.Aspects.Autofac.Authorization;
using Core.BaseResults;
using Core.DataAccess.Dapper;
using Core.Entities;
using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Core.Utilities.Business
{
    public interface IEntityService<T, TFilter> 
        where T:class, IEntity, new()
        where TFilter : IFilter, new()
    {

        ListResult<T> GetList(QueryParameter<TFilter> queryParameter);
 
        List<T> GetByFilter(TFilter filter);
        T GetByID(int id);

        T Add(T entity);

        bool Update(T entity);
        bool UpdatePartial<TUpdateProperties>(T entity, Expression<Func<T, TUpdateProperties>> func) where TUpdateProperties : class;
        bool Delete(T entity);
    }

    public interface IEntityServiceWithDto<T, TFilter, TDto> : IEntityService<T, TFilter>  
      where T : class, IEntity, new()
      where TFilter : IFilter, new()
    {
        ListResult<TDto> GetDtoList(QueryParameter<TFilter> queryParameter);
        List<TDto> GetDtoByFilter(TFilter filter);
        T AddWithDto(TDto entity);

        bool UpdateWithDto(TDto entity);
    }

}
