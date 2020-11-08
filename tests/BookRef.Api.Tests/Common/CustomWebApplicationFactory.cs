﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BookRef.Api.Persistence;

namespace BookRef.Api.Tests.Common
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override void ConfigureWebHost(
            IWebHostBuilder builder)
        {
            Environment.SetEnvironmentVariable("JWT_ISSUER", "http://example.com");
            Environment.SetEnvironmentVariable("JWT_KEY", "123242321312321321323");

            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // remove the existing context configuration. In testing we only want the InMemory database
                var descriptor =
                    services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BookRefDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<BookRefDbContext>(options =>
                {
                    options.UseInMemoryDatabase(new Guid().ToString());
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();


                // see https://docs.microsoft.com/de-de/aspnet/core/test/integration-tests?view=aspnetcore-3.0 for more
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var context = scopedServices.GetRequiredService<BookRefDbContext>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                // Ensure the database is created.
                context.Database.EnsureCreated();

                try
                {
                    // Seed the database with test data.
                    Utilities.InitializeDbForTests(context);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $@"An error occurred seeding the
                                        database with test data. Error: {ex.Message}");
                }
            }).UseEnvironment("Testing");
        }
    }
}
