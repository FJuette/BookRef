using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BookRef.Api.Persistence;
using System.Threading.Tasks;
using System.Linq;
using BookRef.Api.Services;

namespace BookRef.Api.Extensions
{
    public static class MigrationManager
    {
        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        public static IHost MigrateDatabase(
            this IHost webHost)
        {
            using var scope = webHost.Services.CreateScope();
            var appContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<BookRefDbContext>>();
            var service = scope.ServiceProvider.GetRequiredService<IBookApiService>();
            using BookRefDbContext dbContext =
                 appContextFactory.CreateDbContext();
            //var repository = scope.ServiceProvider.GetRequiredService<AggregateRepository>();
            try
            {
                var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
                dbContext.Database.EnsureCreated();
                if (env.IsProduction())
                {
                    // not working with in memory dbs
                    dbContext.Database.Migrate();
                }

                if (!dbContext.Books.Any())
                {
                    var task = Task.Run(async () => await new SampleDataSeeder(dbContext, service).SeedAll());
                    task.GetAwaiter().GetResult();
                }

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
