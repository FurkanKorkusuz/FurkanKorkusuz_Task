using Castle.DynamicProxy;
using Core.DataAccess.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Interceptors.Autofac;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aspects.Autofac.Performance
{
    public class PerformanceAspect : MethodInterception
    {
        private int _interval; // Proses time
        private Stopwatch _stopwatch; // Counter

        public PerformanceAspect(int interval)
        {
            _interval = interval;
            _stopwatch = ServiceTool.ServiceProvider.GetService<Stopwatch>();
        }

        protected override void OnBefore(IInvocation invocation)
        {
            _stopwatch.Start();
        }

        protected override void OnAfter(IInvocation invocation)
        {
            if (_stopwatch.Elapsed.TotalSeconds > _interval)
            {

                Log log = new Log
                {

                    Date = DateTime.Now,
                    Detail = $@"Performance : {_stopwatch.Elapsed.TotalSeconds}, {invocation.TargetType.FullName}",
                    Audit = (byte)LogQualification.Error,

                };
                AddLog(log);
            }
            _stopwatch.Reset();
        }

        private static void AddLog(Log log)
        {
            ILogDal logDal = ServiceTool.ServiceProvider.GetService<ILogDal>();
            logDal.Create(log);
        }
    }
}