using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class SuccessDataResult<T> : DataResult<T>
    {
        public SuccessDataResult(T data, string errorMessage, string infoMessage) : base(data, true, errorMessage, infoMessage)
        {
        }
        public SuccessDataResult(T data, string errorMessage, string focus, string infoMessage) : base(data, true, errorMessage, focus, infoMessage)
        {
        }
        public SuccessDataResult(T data) : base(data, true)
        {
        }

        public SuccessDataResult(string errorMessage, string infoMessage) : base(default, true, errorMessage, infoMessage)
        {
        }
        public SuccessDataResult(string errorMessage, string focus, string infoMessage) : base(default, true, errorMessage, focus, infoMessage)
        {
        }

        public SuccessDataResult() : base(default, true)
        {
        }
    }
}
