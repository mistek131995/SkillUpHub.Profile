using Microsoft.Extensions.DependencyInjection;
using SkillHub.Profile.Application.Interfaces;
using SkillHub.Profile.Application.Services;
using IServiceProvider = SkillHub.Profile.Application.Interfaces.IServiceProvider;

namespace SkillHub.Profile.Application.Providers;

public class ServiceProvider(System.IServiceProvider serviceProvider) : IServiceProvider
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