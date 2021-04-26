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

	        timer.Elapsed += async (sender, args) =>
	        {
		        if (mNotifyFrontend != null)
		        {
			        Console.WriteLine("TimerEvent: Invoking callback at "+args.SignalTime.ToString() );
			        var task = mNotifyFrontend.Invoke(args.SignalTime);
			        Console.WriteLine("TimerEvent: Returned from callback at "+args.SignalTime.ToString() );
			        if (task != null)
			        {
			        	try	
			        	{
					        Console.WriteLine($"TimerEvent: Status: {task.Status} ({task.Status:D}), completed: {task.IsCompleted}");
					        var result = await task;
					        Console.WriteLine($"TimerEvent: task awaited. result={result}");
				      	}
				      	catch (Exception e)
				      	{
									Console.WriteLine($"TimerEvent: Exception caught={e}");
								}
			        }
		        }
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
	        dict.Add("test", "testObj äöü€");
	        dict.Add("now", DateTime.UtcNow);
	        dict.Add("long", 0xC000000000000003ul);
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
	        Console.WriteLine("RegisterEventCallback: Callback set, returning.");
	        return null;
        }
    }
}
