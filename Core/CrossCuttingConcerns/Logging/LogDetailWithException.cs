using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Logging
{
    public class LogDetailWithException : LogDetail
    {
#pragma warning disable CS0108 // 'LogDetailWithException.ExceptionMessage' hides inherited member 'LogDetail.ExceptionMessage'. Use the new keyword if hiding was intended.
        public string ExceptionMessage { get; set; }
#pragma warning restore CS0108 // 'LogDetailWithException.ExceptionMessage' hides inherited member 'LogDetail.ExceptionMessage'. Use the new keyword if hiding was intended.
    }
}
