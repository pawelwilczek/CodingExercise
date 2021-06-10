using CodingExercise.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingExercise.Tests.Api
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Add a database context (EntitiesDbContext)
                services.AddDbContext<CodingExerciseContext>(options =>
                {
                    options.UseInMemoryDatabase("CodingExerciseDb_ApiTests");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                // Build the service provider.
                var sp = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                // Create a scope to obtain a reference to the database contexts
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<CodingExerciseContext>();

                    db.Database.EnsureCreated();

                    DataSeeder.ClearDatabase(db);
                    DataSeeder.PopulateDatabase(db);
                }
            });
        }
    }
}
