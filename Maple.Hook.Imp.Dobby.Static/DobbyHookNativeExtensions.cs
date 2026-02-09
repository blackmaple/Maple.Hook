using Microsoft.Extensions.DependencyInjection;

namespace Maple.Hook.Imp.Dobby.Static
{
    public static class DobbyHookNativeExtensions
    {
        public static IServiceCollection AddDobbyHookNativeFactory(this IServiceCollection services )
        {
            services.AddDobbyHookFactory(p => new DobbyHookNativeMethods());
            return services;
        }
    }
}
