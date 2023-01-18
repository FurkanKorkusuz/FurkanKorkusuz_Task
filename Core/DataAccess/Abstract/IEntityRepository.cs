using Core.BaseResults;
using Core.DataAccess.Dapper;
using Core.Entities.Abstract;
using System.Collections.Generic;
using System.Data;

namespace Core.DataAccess.Abstract
{
    public interface IEntityRepository<T, TFilter> : ICrud<T, TFilter>
        where T : class, IEntity, new()
        where TFilter : class, IFilter, new()
    {
        //bool? UpdatePartial<TUpdateProperties>(T entity, Expression<Func<T, TUpdateProperties>> func, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null) where TUpdateProperties : class;
    }

    public interface IEntityRepositoryWithDto<T, TFilter, TDto> : IEntityRepository<T, TFilter>
        where T : class, IEntity, new()
        where TFilter : class, IFilter, new()
    {
        ListResult<TDto> GetDtoList(QueryParameter<TFilter> queryParameter);
        List<TDto> GetDtoByFilter(TFilter filter, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null);
    }
}
