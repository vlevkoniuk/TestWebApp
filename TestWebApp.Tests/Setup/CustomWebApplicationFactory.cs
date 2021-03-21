using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace TestWebApp.Tests.Setup
{
    public class CustomWebApplicationFactory
        : WebApplicationFactory<TestWebApp.Startup>
    {
        //protected override void ConfigureWebHost(IWebHostBuilder builder)
        //{
        //    builder.ConfigureServices(services =>
        //    {
                
               

        //        var sp = services.BuildServiceProvider();

        //        using (var scope = sp.CreateScope())
        //        {
        //            var scopedServices = scope.ServiceProvider;
        //            var logger = scopedServices
        //                .GetRequiredService<ILogger<CustomWebApplicationFactory>>();
        //        }
        //    });
        //}
    }
}
