using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        public ErrorDataResult(T data, string errorMessage, string focus, string infoMessage = "İşlem Gerçekleştirilemedi.") : base(data, false, errorMessage, focus, infoMessage)
        {
        }

        public ErrorDataResult(T data, string errorMessage, string infoMessage = "İşlem Gerçekleştirilemedi.") : base(data, false, errorMessage, infoMessage)
        {
        }

        public ErrorDataResult(T data) : base(data, false)
        {
        }





        public ErrorDataResult(string errorMessage, string focus, string infoMessage = "İşlem Gerçekleştirilemedi.") : base(default, false, errorMessage, focus, infoMessage)
        {
        }

        public ErrorDataResult(string errorMessage, string infoMessage = "İşlem Gerçekleştirilemedi.") : base(default, false, errorMessage, infoMessage)
        {
        }

        public ErrorDataResult() : base(default, false)
        {
        }
    }
}
