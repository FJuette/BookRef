using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BookRef.Api.Persistence;

namespace BookRef.Api.Extensions
{
    public static class MigrationManager
    {
        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        public static IHost MigrateDatabase(
            this IHost webHost)
        {
            using var scope = webHost.Services.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<BookRefDbContext>();
            try
            {
                var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
                appContext.Database.EnsureCreated();
                if (env.IsProduction())
                {
                    // not working with in memory dbs
                    appContext.Database.Migrate();
                }

                new SampleDataSeeder(appContext).SeedAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Log errors or do anything you think it's needed
                throw;
            }

            return webHost;
        }
    }
}
