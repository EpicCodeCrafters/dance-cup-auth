using ECC.DanceCup.Auth.Application;
using ECC.DanceCup.Auth.Domain;
using ECC.DanceCup.Auth.Infrastructure.Notifications;
using ECC.DanceCup.Auth.Infrastructure.Security;
using ECC.DanceCup.Auth.Infrastructure.Storage;
using ECC.DanceCup.Auth.Logging;
using ECC.DanceCup.Auth.Presentation.Grpc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prometheus;

namespace ECC.DanceCup.Auth;

public class Startup(IConfiguration configuration, IWebHostEnvironment environment)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDomainServices();
        
        services.AddApplicationServices();

        services.AddStorage(configuration);
        services.AddSecurity(configuration);
        services.AddNotifications(configuration);

        services.AddGrpcServices();
        services.AddGrpcHealthChecks()
            .AddCheck(string.Empty, () => HealthCheckResult.Healthy())
            .ForwardToPrometheus();
        
        services.AddCustomLogging(configuration, environment);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseCustomLogging();
        
        app.UseRouting();
        
        app.UseGrpcMetrics();
        app.UseHttpMetrics();

        app.UseEndpoints(endpointRouteBuilder =>
        {
            endpointRouteBuilder.UseGrpcServices();
            endpointRouteBuilder.MapGrpcHealthChecksService();
            endpointRouteBuilder.MapMetrics();
        });
    }
}