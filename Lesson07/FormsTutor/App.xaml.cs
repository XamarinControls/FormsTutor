﻿using Xamarin.Forms;
using Akavache;
using Splat;
using System.Reactive.Linq;
using System.Reactive;
using System.Linq;
using System.Diagnostics;

namespace FormsTutor
{
    public partial class App : Application
    {
		public App()
		{
		    InitializeComponent();

		    BlobCache.ApplicationName = Configuration.ApplicationName;

		    var logger = new Logger() { Level = LogLevel.Debug };
		    Locator.CurrentMutable.RegisterConstant(logger, typeof(ILogger));

		    MainPage = new ArticlesPage();
		}

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            var caches = new [] 
            { 
                BlobCache.LocalMachine, 
                BlobCache.Secure, 
                BlobCache.UserAccount, 
                BlobCache.InMemory 
            };

            caches.Select(x => x.Flush()).Merge().Select(_ => Unit.Default).Wait();
        }

        protected override void OnResume()
        {
            
        }
    }

	public class Logger : ILogger
	{
		public void Write(string message, LogLevel logLevel)
		{
	        if ((int)logLevel < (int)Level) return;

	        Debug.WriteLine(message);
		}

		public LogLevel Level { get; set; }
	}
}
