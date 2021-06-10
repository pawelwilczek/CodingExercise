using CodingExercise.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace CodingExercise.Tests.Api
{
    public static class CustomWebApplicationFactoryExtensions
    {
        public static HttpClient ReinitializeDb(this CustomWebApplicationFactory<Startup> factory)
        {
            return factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var serviceProvider = services.BuildServiceProvider();

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices
                            .GetRequiredService<CodingExerciseContext>();

                        DataSeeder.ClearDatabase(db);
                        DataSeeder.PopulateDatabase(db);
                    }
                });
            })
            .CreateClient();
        }
    }
}
