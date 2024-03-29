using Gradebook.Foundation.DependencyResolver.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Receipts.Foundation.DependencyResolver;
public class DependencyInjector
{
    public static void Inject(IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddHttpContextAccessor();
        AutoMapperService.Inject(services, configuration);
        IdentityService.Inject(services, configuration);
        FoundationService.Inject(services, configuration);
        PermissionsService.Inject(services, configuration);
    }
}

