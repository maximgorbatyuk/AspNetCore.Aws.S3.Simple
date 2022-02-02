using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sample.Api.Services
{
    public class HealthCheckCustomResponse : HealthCheckOptions
    {
        public const string HealthCheckRoute = "/health";

        public HealthCheckCustomResponse()
        {
            ResponseWriter = WriteAsync;
        }

        private static async Task WriteAsync(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
            {
                status = report.Status.ToString(),
                errors = report.Entries.Select(e => new { key = e.Key, value = e.Value.Status.ToString() })
            }));
        }
    }
}