﻿using Microsoft.Extensions.DependencyInjection;

namespace ECC.DanceCup.Auth.Application;

public static class Registrar
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Registrar).Assembly);
        });
        
        return services;
    }
}