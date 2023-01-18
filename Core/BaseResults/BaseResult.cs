using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BaseResults
{
	public class BaseResult
	{

		public BaseResult()
		{
		}


		public BaseResult(object data)
		{
			Data = data;
		}

		public object Data { get; set; }
		public string Error { get; set; }
		public bool Success { get { return string.IsNullOrEmpty(Error); } }
	}

	
}
