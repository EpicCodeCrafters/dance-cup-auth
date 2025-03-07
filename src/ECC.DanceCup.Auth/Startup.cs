using ECC.DanceCup.Auth.Application;
using ECC.DanceCup.Auth.Domain;
using ECC.DanceCup.Auth.Infrastructure.Notifications;
using ECC.DanceCup.Auth.Infrastructure.Security;
using ECC.DanceCup.Auth.Infrastructure.Storage;
using ECC.DanceCup.Auth.Presentation.Grpc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ECC.DanceCup.Auth;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDomainServices();
        
        services.AddApplicationServices();

        services.AddStorage(_configuration);
        services.AddSecurity(_configuration);
        services.AddNotifications(_configuration);

        services.AddGrpcServices();
        services.AddGrpcHealthChecks().AddCheck(string.Empty, () => HealthCheckResult.Healthy());
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseEndpoints(endpointRouteBuilder =>
        {
            endpointRouteBuilder.UseGrpcServices();
            endpointRouteBuilder.MapGrpcHealthChecksService();
        });
    }
}