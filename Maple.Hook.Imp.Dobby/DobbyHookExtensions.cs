using Maple.Hook.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Maple.Hook.Imp.Dobby
{
    public static class DobbyHookExtensions
    {
        public static IServiceCollection AddDobbyHookFactory(this IServiceCollection services,Func<IServiceProvider, IDobbyHookRuntime> runtimeFactory)
        {
            services.TryAddSingleton(p => runtimeFactory(p));
            services.TryAddSingleton<IHookFactory, DobbyHookFactory>();
            return services;
        }

    }


}
