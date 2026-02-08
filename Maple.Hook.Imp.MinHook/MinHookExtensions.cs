using Maple.Hook.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Maple.Hook.Imp.MinHook
{
    public static class MinHookExtensions
    {
        public static IServiceCollection AddMinHookFactory(this IServiceCollection services)
        {
            services.TryAddSingleton<IHookFactory, MinHookFactory>();
            return services;
        }


 
    }
}
