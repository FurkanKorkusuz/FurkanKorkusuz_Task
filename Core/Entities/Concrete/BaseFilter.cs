using Core.DataAccess.Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
    public class BaseFilter : IFilter
    {
        [Computed]
        public int RowNumber { get ; set ; }
    }
}
