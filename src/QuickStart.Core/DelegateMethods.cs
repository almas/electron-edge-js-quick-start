using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace QuickStart.Core
{
	public class DelegateMethods
	{
		public Func<object, Task<object>> GetCurrentTime => GetCurrentTimeMethod;

		private async Task<object> GetCurrentTimeMethod(dynamic aArgument)
		{
			return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		}
	}
}