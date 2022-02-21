using AspNetCore.Aws.S3.Simple.Settings;
using Sample.Api.Services;

namespace Sample.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services
            .AddHealthChecks()
            .AddS3HealthChecks(builder.Configuration);

        builder.Services
            .AddHttpContextAccessor()
            .AddS3Settings()
            .AddS3Storage<IReimbursementFileStorage, ReimbursementAttachmentS3Storage>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        // domaim.com/health
        app.UseHealthChecks(HealthCheckCustomResponse.HealthCheckRoute, new HealthCheckCustomResponse());

        app.MapControllers();
        app.Run();
    }
}