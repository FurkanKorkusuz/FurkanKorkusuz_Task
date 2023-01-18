using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class SuccessResult : Result
    {
        public SuccessResult(string errorMessage, string focus, string infoMessage) : base(true, errorMessage, focus, infoMessage)
        {

        }
        public SuccessResult(string errorMessage, string infoMessage) : base(true, errorMessage, infoMessage)
        {

        }
        public SuccessResult() : base(true)
        {

        }
    }
}
