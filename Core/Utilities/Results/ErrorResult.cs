using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class ErrorResult : Result
    {
        public ErrorResult(string errorMessage, string focus, string infoMessage = "İşlem Gerçekleştirilemedi.") : base(false, errorMessage, focus, infoMessage)
        {

        }
        public ErrorResult(string errorMessage, string infoMessage = "İşlem Gerçekleştirilemedi.") : base(false, errorMessage, infoMessage)
        {

        }
        public ErrorResult() : base(false)
        {

        }
    }
}
