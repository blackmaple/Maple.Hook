using Maple.Hook.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maple.Hook.Imp.Dobby.Dynamic
{
    public static class DobbyHookDynamicExtensions
    {
        public static IServiceCollection AddDobbyHookDynamicFactory(this IServiceCollection services, string dll)
        {
            services.AddDobbyHookFactory(p=> DobbyHookDynamicMethods.Create(dll));
            return services;
        }
    }
}
