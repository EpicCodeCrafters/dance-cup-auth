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
        services.AddGrpc();
        services.AddGrpcHealthChecks().AddCheck(string.Empty, () => HealthCheckResult.Healthy());
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseEndpoints(endpointRouteBuilder =>
        {
            endpointRouteBuilder.MapGrpcHealthChecksService();
        });
    }
}