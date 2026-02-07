using Maple.Hook.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maple.Hook.Imp.MinHook
{
    public static class MinHookExtensions
    {
        public static IServiceCollection AddMinHookFactory(this IServiceCollection services)
        {
            services.AddSingleton<IHookFactory, MinHookFactory>();
            return services;
        }
    }
}
