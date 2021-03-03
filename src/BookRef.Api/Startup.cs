using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BookRef.Api.Filters;
using BookRef.Api.Health;
using BookRef.Api.Infrastructure;
using BookRef.Api.Persistence;
using EventStore.ClientAPI;
using HotChocolate;
using BookRef.Api.Models.Framework;
using BookRef.Api.Persistence.DataLoader;
using BookRef.Api.Models.Relations;
using BookRef.Api.Models.Types;
using Microsoft.EntityFrameworkCore;
using BookRef.Api.Authors;
using BookRef.Api.Books;
using BookRef.Api.Categories;
using BookRef.Api.People;
using BookRef.Api.Speakers;
using System.Text.Json.Serialization;
using BookRef.Api.Services;
using BookRef.Api.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BookRef.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public void ConfigureServices(
            IServiceCollection services)
        {
            // Add MediatR - must be first
            //services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = EnvFactory.GetJwtIssuer(),
                        ValidAudience = EnvFactory.GetJwtIssuer(),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvFactory.GetJwtKey()))
                    });

            services.AddAuthorization();

            services.AddCors(options =>
                options.AddPolicy("Locations",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200");
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                        builder.AllowAnyOrigin(); //TODO remove in production and add to origin list
                    }));

            // var eventStoreConnection = EventStoreConnection.Create(
            //     connectionString: "ConnectTo=tcp://admin:changeit@localhost:1113; DefaultUserCredentials=admin:changeit;",
            //     builder: ConnectionSettings.Create().KeepReconnecting(),
            //     connectionName: "Library");
            // eventStoreConnection.ConnectAsync().GetAwaiter().GetResult();
            // services.AddSingleton(eventStoreConnection);
            // services.AddTransient<AggregateRepository>();

            // Add Swagger
            //services.AddSwaggerDocumentation();

            // Add Health Checks
            services.AddHealthChecks()
                //.AddSqlServer(EnvFactory.GetConnectionString()) //TODO Enable if real MSSQL-Server is given
                .AddCheck<ApiHealthCheck>("api");

            services.AddPooledDbContextFactory<BookRefDbContext>(options =>
                options.UseSqlite("Data Source=bookref.db")
                .UseLazyLoadingProxies()
                //.EnableSensitiveDataLogging()
            );

            // Add my own services here
            services.AddScoped<IGetClaimsProvider, GetClaimsFromUser>();
            services.AddScoped<IOpenLibraryService, OpenLibraryService>();
            services.AddSingleton<IDateTime, MachineDateTime>();

            services
                .AddGraphQLServer()
                .AddQueryType(d => d.Name("Query"))
                    .AddType<AuthorQueries>()
                    .AddType<CategoryQueries>()
                    .AddType<BookQueries>()
                    .AddType<PersonQueries>()
                    .AddType<SpeakerQueries>()
                .AddMutationType(d => d.Name("Mutation"))
                    .AddTypeExtension<PeopleMutations>()
                    .AddTypeExtension<BookMutations>()
                    .AddTypeExtension<UserMutations>()
                .AddType<AuthorType>()
                .AddType<BookRecommedationType>()
                .AddType<BookType>()
                .AddType<CategoryType>()
                .AddType<NoteType>()
                .AddType<PersonalBookType>()
                .AddType<PersonRecommedationType>()
                .AddType<PersonType>()
                .AddType<SpeakerType>()
                .AddAuthorization()
                .EnableRelaySupport()
                .AddFiltering()
                .AddSorting()
                .AddDataLoader<AuthorByIdDataLoader>()
                .AddDataLoader<BookByIdDataLoader>()
                .AddDataLoader<BookRecommendationByIdDataLoader>()
                .AddDataLoader<CategoryByIdDataLoader>()
                .AddDataLoader<NoteByIdDataLoader>()
                .AddDataLoader<PersonByIdDataLoader>()
                .AddDataLoader<PersonalBookByIdDataLoader>()
                .AddDataLoader<PersonRecommendationByIdDataLoader>()
                .AddDataLoader<SpeakerByIdDataLoader>();

            services.AddAutoMapper(typeof(Startup));

            services.AddControllers(options => options.Filters.Add(typeof(CustomExceptionFilter)))
                .AddFluentValidation(fv =>
                {
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                })
                .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                    }
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public void Configure(
            IApplicationBuilder app)
        {
            app.UseCors("Locations");
            //app.UseSwaggerDocumentation();

            app.UseHealthChecks("/health", new HealthCheckOptions {ResponseWriter = WriteHealthCheckResponse});

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGraphQL();
            });
        }

        private static Task WriteHealthCheckResponse(
            HttpContext httpContext,
            HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";
            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(
                    result.Entries.Select(pair =>
                        new JProperty(pair.Key, new JObject(
                            new JProperty("status", pair.Value.Status.ToString()),
                            new JProperty("exception", pair.Value.Exception?.Message),
                            new JProperty("description", pair.Value.Description),
                            new JProperty("data", new JObject(pair.Value.Data.Select(
                                p => new JProperty(p.Key, p.Value)))
                            )
                        )))
                ))
            );
            return httpContext.Response.WriteAsync(
                json.ToString(Formatting.Indented)
            );
        }
    }
}
