using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Dapper
{

    public class QueryParameter<TFilter>
        where TFilter : IFilter
    {

        public string Sort { get; set; } 

        public bool IncludeRowCount { get; set; } = false;
   
        public TFilter Filter { get; set; }

    }
    public interface IFilter
    {
        public int RowNumber { get; set; }
     
    }

}
