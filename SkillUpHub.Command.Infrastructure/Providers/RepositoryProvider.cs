using Microsoft.Extensions.DependencyInjection;
using SkillUpHub.Command.Contract.Repositories;
using SkillUpHub.Command.Infrastructure.Repositories;
using SkillUpHub.Profile.Contract.Providers;
using IServiceProvider = System.IServiceProvider;

namespace SkillUpHub.Command.Infrastructure.Providers;

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