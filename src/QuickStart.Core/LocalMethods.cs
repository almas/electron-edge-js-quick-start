using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Threading.Tasks;
using System.Timers;

namespace QuickStart.Core
{
    public class LocalMethods
    {
        private IDictionary<string, object> mDict;
        private Func<object, Task<object>> mNotifyFrontend;

        public LocalMethods()
        {
	        var timer = new Timer(5000);

	        timer.Elapsed += (sender, args) =>
	        {
		        mNotifyFrontend?.Invoke(args.SignalTime);
	        };

	        timer.Start();
        }

        public async Task<object> GetAppDomainDirectory(dynamic input)
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public async Task<object> GetCurrentTime(dynamic input)
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public async Task<object> UseDynamicInput(dynamic input)
        {
            return $".NET Core welcomes {input}";
        }

        public async Task<object> GetDelegateHandle(dynamic input)
        {
	        var obj = new DelegateMethods();
	        return obj;
        }

        public async Task<object> GetComplexObject(dynamic input)
        {
	        var obj = new ExpandoObject();
	        IDictionary<string, object> dict;
	        dict = obj;
	        dict.Add("test", "testObj");

	        return obj;
        }

        public async Task<object> GetFromThread(dynamic input)
        {
	        return Task.Run(() =>
	        {
		        var obj = new ExpandoObject();
		        mDict = obj;
		        mDict.Add("test", "testObj");

            }).ContinueWith((t) =>
	        {
		        if (!t.IsFaulted)
		        {
			        return mDict;
		        }

		        throw t.Exception;
	        });
        }

        public async Task<object> RegisterEventCallback(dynamic input)
        {
	        //Debugger.Launch();
	        mNotifyFrontend = input.EventCallback;
	        return null;
        }
    }
}
