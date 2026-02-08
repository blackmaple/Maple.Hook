using Maple.Hook.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Maple.Hook.Imp.Dobby
{
    public static class DobbyHookExtensions
    {
        public static IServiceCollection AddDobbyHookFactory(this IServiceCollection services, string dll)
        {
            services.TryAddSingleton<IHookFactory>(p => DobbyHookFactory.Create(dll));
            return services;
        }

    }


}
