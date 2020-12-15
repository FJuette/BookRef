using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BookRef.Api.Filters;
using BookRef.Api.Health;
using BookRef.Api.Infrastructure;
using BookRef.Api.Persistence;
using EventStore.ClientAPI;
using BookRef.Api.Models.Schemas;
using GraphQL.Server;
using Microsoft.Extensions.Logging;
using GraphQL.Server.Ui.Playground;

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
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //     .AddJwtBearer(options =>
            //         options.TokenValidationParameters = new TokenValidationParameters
            //         {
            //             ValidateIssuer = true,
            //             ValidateAudience = true,
            //             ValidateLifetime = true,
            //             ValidateIssuerSigningKey = true,
            //             ValidIssuer = EnvFactory.GetJwtIssuer(),
            //             ValidAudience = EnvFactory.GetJwtIssuer(),
            //             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvFactory.GetJwtKey()))
            //         });

            // // At least a module claim is required to use any protected endpoint
            // services.AddAuthorization(
            //     auth => auth.DefaultPolicy = new AuthorizationPolicyBuilder()
            //         .RequireClaim("modules", "claim-module-name")
            //         .Build());

            services.AddCors(options =>
                options.AddPolicy("Locations",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200");
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                        builder.AllowAnyOrigin(); //TODO remove in production and add to origin list
                    }));

            var eventStoreConnection = EventStoreConnection.Create(
                connectionString: "ConnectTo=tcp://admin:changeit@localhost:1113; DefaultUserCredentials=admin:changeit;",
                builder: ConnectionSettings.Create().KeepReconnecting(),
                connectionName: "User");
            eventStoreConnection.ConnectAsync().GetAwaiter().GetResult();
            services.AddSingleton(eventStoreConnection);
            services.AddTransient<AggregateRepository>();

            services
                .AddScoped<BookSchema>()
                .AddGraphQL((options, provider) =>
                {
                    options.EnableMetrics = true;
                    var logger = provider.GetRequiredService<ILogger<Startup>>();
                    options.UnhandledExceptionDelegate = ctx => logger.LogError("{Error} occurred", ctx.OriginalException.Message);
                })
                // Add required services for GraphQL request/response de/serialization
                .AddSystemTextJson() // For .NET Core 3+
                .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
                .AddDataLoader() // Add required services for DataLoader support
                .AddGraphTypes(ServiceLifetime.Scoped); // Add all IGraphType implementors in assembly which ChatSchema exists

            // Add Swagger
            services.AddSwaggerDocumentation();

            // Add Health Checks
            services.AddHealthChecks()
                //.AddSqlServer(EnvFactory.GetConnectionString()) //TODO Enable if real MSSQL-Server is given
                .AddCheck<ApiHealthCheck>("api");

            services.AddScoped<BookRefDbContext>();

            services.AddAutoMapper(typeof(Startup));

            // Avoid the MultiPartBodyLength error
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            // Add my own services here
            services.AddScoped<IGetClaimsProvider, GetClaimsFromUser>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IDateTime, MachineDateTime>();

            services.AddControllers(options => options.Filters.Add(typeof(CustomExceptionFilter)))
                .AddFluentValidation(fv =>
                {
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                }).AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public void Configure(
            IApplicationBuilder app)
        {
            app.UseCors("Locations");
            app.UseSwaggerDocumentation();
            app.UseGraphQL<BookSchema>();
            // app.UseGraphiQl();
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());
            //to explorer API navigate https://*DOMAIN*/ui/playground

            app.UseHealthChecks("/health", new HealthCheckOptions {ResponseWriter = WriteHealthCheckResponse});

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
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
