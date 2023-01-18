using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public interface IResult
    {
        bool Success { get; }
        string InfoMessage { get; }
        string ErrorMessage { get; }
        string Focus { get; }
    }
}
