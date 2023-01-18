using Core.BaseResults;
using Core.DataAccess.Dapper;
using Core.Entities.Concrete;
using Core.Entities.DTOs;
using System.Collections.Generic;
using Core.Utilities.Business;
using System.Linq.Expressions;
using System.Linq;
using System;

namespace Business.Abstract
{
    public interface INewService
    {
        EntityResult<List<New>> GetList();
        EntityResult<List<New>> Find(Expression<Func<New, bool>> predicate);
        EntityResult<New> GetByID(int id);
        EntityResult<New> Add(NewAddDto entity);
        BaseResult Update(NewAddDto entity);
        BaseResult Delete(int id);
    }

   
}

        