using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDomainServices()
        {
            return services;
        }
    }
}