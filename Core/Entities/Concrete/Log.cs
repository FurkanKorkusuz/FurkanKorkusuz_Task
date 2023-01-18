using Core.DataAccess.Dapper;
using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
    [Table("Logs")]
    public class Log :IEntity
    {
        [Dapper.Contrib.Extensions.Key]
        public int ID { get; set; }
        public string Detail { get; set; }
        public DateTime Date { get; set; }
        public byte Audit { get; set; } = (byte)LogQualification.Error;
        public int? UserID { get; set; }
    }

    public class LogFilter : IFilter
    {
        public int ID { get; set; }
        public string Detail { get; set; }
        public DateTime Date { get; set; }
        public byte Audit { get; set; }
        public int RowNumber { get; set; }
    }

    public enum LogQualification
    {
        Success = 0,
        Message = 1,
        Warning = 2,
        Error = 3
    }
}
