using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
    public abstract class BaseEntity :  IEntity
    {
        [Dapper.Contrib.Extensions.Key]
        [Key]
        public int ID { get; set; }
    }
}
