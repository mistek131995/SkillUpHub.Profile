using Microsoft.Extensions.DependencyInjection;
using SkillUpHub.Profile.Contract.Providers;
using SkillUpHub.Profile.Contract.Repositories;
using SkillUpHub.Profile.Infrastructure.Repositories;
using IServiceProvider = System.IServiceProvider;

namespace SkillUpHub.Profile.Infrastructure.Providers;

public class RepositoryProvider(IServiceProvider serviceProvider) : IRepositoryProvider
{
    private readonly Dictionary<Type, IBaseRepository> _repositories = new();

    private T Get<T>() where T : IBaseRepository
    {
        var type = typeof(T);

        if (_repositories.TryGetValue(type, out var repository))
        {
            return (T)repository;
        }

        repository = ActivatorUtilities.CreateInstance<T>(serviceProvider);

        _repositories[type] = repository;

        return (T)repository;
    }

    public IProfileRepository ProfileRepository => Get<ProfileRepository>();
}