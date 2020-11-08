using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using BookRef.Api.Persistence;

namespace BookRef.Api.Health
{
    public class ApiHealthCheck : IHealthCheck
    {
        private readonly BookRefDbContext _ctx;

        public ApiHealthCheck(
            BookRefDbContext ctx) =>
            _ctx = ctx;

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var canConnect = await _ctx.Database.CanConnectAsync(cancellationToken);
            if (canConnect)
            {
                return HealthCheckResult.Healthy("Database connection is working.");
            }

            return HealthCheckResult.Unhealthy("Database connection is not working.");
        }
    }
}
