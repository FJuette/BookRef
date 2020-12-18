using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BookRef.Api.Persistence;
using System.Threading.Tasks;

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
            var repository = scope.ServiceProvider.GetRequiredService<AggregateRepository>();
            try
            {
                var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
                appContext.Database.EnsureCreated();
                if (env.IsProduction())
                {
                    // not working with in memory dbs
                    appContext.Database.Migrate();
                }

                var task = Task.Run(async () => await new SampleDataSeeder(appContext, repository).SeedAll());
                task.GetAwaiter().GetResult();
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
