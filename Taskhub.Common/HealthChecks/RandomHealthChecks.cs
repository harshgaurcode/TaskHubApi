using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskhub.Common.HealthChecks
{
    public class RandomHealthChecks : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            int responseTimeInMs=Random.Shared.Next(300);

            if(responseTimeInMs < 100)
            {
                return Task.FromResult(HealthCheckResult.Healthy(description:$"The response time is excellent" +
                    $"({responseTimeInMs})"));

            }

            else if (responseTimeInMs < 200)
            {
                return Task.FromResult(HealthCheckResult.Degraded(description: $"The response time is greater than expected" +
                    $"({responseTimeInMs})"));

            }
            else
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(description: $"The response time is unacceptable" +
                    $"({responseTimeInMs})"));

            }
        }
    }
}
