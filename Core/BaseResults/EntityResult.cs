using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BaseResults
{
    public class EntityResult<T>:BaseResult
        where T : class,  new()
    {
        public T Entity { get; set; }
    }
}
