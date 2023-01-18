using Core.BaseResults;
using Core.DataAccess.Abstract;
using Core.DataAccess.Repositories;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Core.DataAccess.Dapper
{
    public class DpLogDal : RepositoryBase<Log, LogFilter>, ILogDal
    {
    }
}
