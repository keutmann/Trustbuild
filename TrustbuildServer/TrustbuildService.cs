using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Web.Http;
using TrustbuildCore.Controllers;
using TrustbuildCore.Service;
using TrustchainCore.Extensions;
using TrustchainCore.IOC;
using System.Diagnostics;


namespace TrustbuildServer
{
    public class TrustbuildService
    {
        private IDisposable _webApp;
        private Timer timer;
        private int timeInMs = 1000*60; // 1 minute
        private volatile bool process = true;

        public object Key { get; private set; }

        public void Start()
        {
            var asm = new Assembly[] { typeof(IOCAttribute).Assembly };
            UnitySingleton.Container.RegisterTypesFromAssemblies(asm);

            var core = new Assembly[] { typeof(TrustController).Assembly };
            UnitySingleton.Container.RegisterTypesFromAssemblies(core);

            var start = new StartOptions();
            start.Urls.Add("http://" + App.Config["endpoint"].ToStringValue("+") + ":" + App.Config["port"].ToInteger(12601) + "/");
            start.Urls.Add("https://" + App.Config["endpoint"].ToStringValue("+") + ":" + App.Config["sslport"].ToInteger(12701) + "/");

            _webApp = WebApp.Start<StartOwin>(start);

            timeInMs = App.Config["processinterval"].ToInteger(timeInMs);

            RunTimer(Execute);
        }

        public class StartOwin
        {
            public void Configuration(IAppBuilder appBuilder)
            {
                HttpListener listener = (HttpListener)appBuilder.Properties["System.Net.HttpListener"];
                listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
                var config = new HttpConfiguration();
                config.Formatters.Add(new BrowserJsonFormatter());
                //config.DependencyResolver = new UnityResolver(UnitySingleton.Container);

                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                    );

                appBuilder.UseWebApi(config);
            }
        }

        public void Configuration(IAppBuilder appBuilder)
        {
            HttpListener listener = (HttpListener)appBuilder.Properties["System.Net.HttpListener"];
            listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;

            var config = new HttpConfiguration();
            config.Formatters.Add(new BrowserJsonFormatter());
            config.DependencyResolver = new UnityResolver(UnitySingleton.Container);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            appBuilder.UseWebApi(config);
        }

        private void RunTimer(Action method)
        {

            method(); // Start now!

            timer = new Timer((o) =>
            {
                try
                {
                    if (process) // Run the job
                        method();
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);
                }
                finally
                {
                    // only set the initial time, do not set the recurring time
                        timer.Change(timeInMs, Timeout.Infinite);
                }
            });

            // only set the initial time, do not set the recurring time
            timer.Change(timeInMs, Timeout.Infinite);
        }

        public void Execute()
        {
            Trace.TraceInformation(DateTime.Now.ToLocalTime() + " : Processing...");
        }

        public void Pause()
        {
            process = false;
        }

        public void Continue()
        {
            process = true;
        }

        public void Stop()
        {
            _webApp.Dispose();
        }
    }
}
