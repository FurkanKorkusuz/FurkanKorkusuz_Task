using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
    public abstract class BaseEntity<T> : ICloneable, IEntity
    {
        [Dapper.Contrib.Extensions.Key]
        public int ID { get; set; }

        public T ShallowClone()
        {
            // If a field is a reference type, the reference is copied but the referred object is not; therefore, the original object and its clone refer to the same object.
            return (T)MemberwiseClone();
        }

        public object Clone() { return MemberwiseClone(); }
    }
}
