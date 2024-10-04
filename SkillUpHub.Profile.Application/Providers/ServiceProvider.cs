using Microsoft.Extensions.DependencyInjection;
using SkillHub.Profile.Application.Services;
using SkillUpHub.Profile.Contract.Services;
using CServiceProvider = SkillUpHub.Profile.Contract.Providers.IServiceProvider;

namespace SkillHub.Profile.Application.Providers;

public class ServiceProvider(IServiceProvider serviceProvider) : CServiceProvider
{
    private readonly Dictionary<Type, IBaseService> _services = new();
    
    private T Get<T>() where T : IBaseService
    {
        var type = typeof(T);

        if (_services.TryGetValue(type, out IBaseService service))
        {
            return (T)service;
        }

        service = ActivatorUtilities.CreateInstance<T>(serviceProvider);
        
        _services[type] = service;
        
        return (T)service;
    }


    public IProfileService ProfileService => Get<ProfileService>();
}