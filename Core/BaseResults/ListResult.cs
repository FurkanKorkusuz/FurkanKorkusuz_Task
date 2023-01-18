using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BaseResults
{
	public class ListResult<T>
	{
		public ListResult()
		{
			RowList = new List<T>();
		}
		public bool Success { get; set; } = true;
        public List<T> RowList { get; set; }
		public int Count { get; set; }
		public object Data { get; set; }

	}

	public class ListConfig
	{
		public const int RowsPerFetch = 30;
	}
}
