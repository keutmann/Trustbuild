using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Owin;
using System;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Web.Http;
using TrustbuildCore.Controllers;
using TrustbuildCore.Service;
using TrustchainCore.Data;
using TrustchainCore.Extensions;

namespace TrustbuildServer
{
    public class BrowserJsonFormatter : JsonMediaTypeFormatter
    {
        public BrowserJsonFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            this.SerializerSettings.Formatting = Formatting.Indented;
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }

    //public sealed class UnitySingleton
    //{
    //    private static readonly UnityContainer instance = new UnityContainer();

    //    private UnitySingleton() { }

    //    public static UnityContainer Container
    //    {
    //        get
    //        {
    //            return instance;
    //        }
    //    }
    //}

    public class TrustbuildService
    {
        private IDisposable _webApp;
        private Timer timer;
        private int timeInMs = 1000*60; // 1 minute
        private volatile bool process = true;

        public object Key { get; private set; }

        public void Start()
        {
            var ttyy = typeof(TrustController); // TO BE REMOVED! Make the Web Api find the controllers

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
                    Console.Error.WriteLine(ex.Message);
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
            Console.WriteLine(DateTime.Now.ToLocalTime() + " : Processing...");
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
