using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Interceptors.Autofac
{
    public class AspectInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {

            // class attribute ile aspect olayı gerçekleşecek olan attributler
            var classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute>(true).ToList();

          
                var parameterTypes = method.GetParameters().Select(x => x.ParameterType).Where(x => x == typeof(object));
                // Metodu override ettiğim zaman aynı isimdeki metodlarda AmbiguousMatchException hatası alıyordum.
                // bunun Yerine tüm metodları GetMethods() ile alıp metot ismi ve aynı isimle override ettiğim için metod parametreleriyle aynı parametrelere sahip olan metodu çağırdım. Burada aynı isimle birden fazla metodum olsa bile aynı parametre tipleri olamayacağından benim istediğim metodu yakalamış olurum.
                var myMethod = type.GetMethods()
                    .Where(x => x.Name == method.Name
                    && Enumerable.SequenceEqual(
                        method.GetParameters().Select(x => x.ParameterType).Where(x => x == typeof(object)),
                        x.GetParameters().Select(x => x.ParameterType).Where(x => x == typeof(object))))
                    .SingleOrDefault<MethodInfo>();

                classAttributes.AddRange(myMethod.GetCustomAttributes<MethodInterceptionBaseAttribute>(true));


                // Tüm hata exception larımı yakalamak için bu kodu yazıyorum.                
                //classAttributes.Add(new ExceptionLogAspect());

                // metot attribute ile aspect olayı gerçekleşecek olan attributler
                /* ***********************
                var methodAttributes = type.GetMethod(method.Name).GetCustomAttributes<MethodInterceptionBaseAttribute>(true);     
       
                                // metod olanları da classdakilere ekleyelim (beraber çalıştırmak için )
                                                                                                                                                   // örneğin securty class ile verilmiştir ve o classdaki tüm metodlarda doğrulama vardır ama validate ise tek add metoduna metot Attribute ile eklenmişse ikisini de çalıştırmam gerekli.
                classAttributes.AddRange(methodAttributes);
                         ************************************** */

           


            // Priority int olarak değer alır ve metotda birden fazla aspect varsa hangisinin önce çalışacağını belirtmek için.
            return classAttributes.OrderBy(x => x.Priority).ToArray();
        }
    }
}
