using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Dapper
{
    public class FilterHelper<TFilter>       
    {
        public static string CreateSqlFilter(TFilter filter)
        {
            var typeFilter = typeof(TFilter);
            List<PropertyInfo> filterProperties = TypeProperties(typeFilter);
            List<PropertyInfo> filterkeyProperties = KeyProperties(typeFilter);
            List<PropertyInfo> filtercomputedProperties = ComputedProperties(typeFilter);
            List<PropertyInfo> filterPropertiesExceptKeyAndComputed = filterProperties.Except(filterkeyProperties.Union(filtercomputedProperties)).ToList();


            StringBuilder filterSql = new StringBuilder();
            StringBuilder sbParameterList = new StringBuilder(null);
            for (var i = 0; i < filterPropertiesExceptKeyAndComputed.Count; i++)
            {
                var property = filterPropertiesExceptKeyAndComputed[i];
                if (property.GetValue(filter) == null)
                {
                    continue;
                }
                if (property.PropertyType == typeof(int[]))
                {
                    filterSql.Append($" AND {property.Name.TrimEnd('s')} in @{property.Name}");
                }
                else
                {
                    filterSql.Append($" AND {property.Name} = @{property.Name}");
                }
                sbParameterList.AppendFormat("@{0}", property.Name);
                //if (i < filterPropertiesExceptKeyAndComputed.Count - 1)
                //    sbParameterList.Append(", ");
            }


            if (filterSql.Length > 0)
                filterSql.Remove(0, 4).Insert(0, " WHERE ");
            return filterSql.ToString();
        }


        private static List<PropertyInfo> TypeProperties(Type type)
        {
            return type.GetProperties().ToList();
        }

        private static List<PropertyInfo> ComputedProperties(Type type)
        {
            var computedProperties = type.GetProperties().Where(p => p.GetCustomAttributes(true).Any(a => a is ComputedAttribute)).ToList();

            return computedProperties;
        }
        private static List<PropertyInfo> KeyProperties(Type type)
        {
            var keyProperties = type.GetProperties().Where(p => p.GetCustomAttributes(true).Any(a => a is KeyAttribute)).ToList();

            return keyProperties;
        }
    }
}
