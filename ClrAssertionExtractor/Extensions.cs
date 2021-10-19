using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace ClrAssertionExtractor
{
    public static class Extensions
    {
        // Step 1: Inject smuggler when building web host
        public static IWebHostBuilder SniffRouteData(this IWebHostBuilder builder)
        {
            return builder.ConfigureServices(svc => svc.AddSingleton<Capture>());
        }
    
        // Step 2: Swipe the route data in application startup
        public static IApplicationBuilder UseMvcAndSniffRoutes(this IApplicationBuilder app)
        {
            var capture = app.ApplicationServices.GetRequiredService<Capture>();
            IRouteBuilder capturedRoutes = null;
            app.UseMvc(routeBuilder => capturedRoutes = routeBuilder);
            capture.Router = capturedRoutes?.Build();
            return app;
        }
    
        // Step 3: Build the UrlHelper using the captured routes and webhost
        public static IUrlHelper GetStaticUrlHelper(this IWebHost host, string baseUri)
            => GetStaticUrlHelper(host, new Uri(baseUri));
        public static IUrlHelper GetStaticUrlHelper(this IWebHost host, Uri baseUri)
        {
            HttpContext httpContext = new DefaultHttpContext()
            {
                RequestServices = host.Services,
                Request =
                {
                    Scheme = baseUri.Scheme,
                    Host = HostString.FromUriComponent(baseUri),
                    PathBase = PathString.FromUriComponent(baseUri),
                },
            };
    
            var captured = host.Services.GetRequiredService<Capture>();
            var actionContext = new ActionContext
            {
                HttpContext = httpContext,
                RouteData = new RouteData { Routers = { captured.Router }},
                ActionDescriptor = new ActionDescriptor(),
            };
            return new UrlHelper(actionContext);
        }
    }
    
    public class Capture
    {
        public IRouter Router { get; set; }
    }
}