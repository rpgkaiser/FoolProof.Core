using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FoolProof.Core
{
    public static class Config
    {
        [Obsolete]
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<IValidationAttributeAdapterProvider, FoolProofValidationAttributeAdapterProvider>();
        }

        public static void AddFoolProof(this IServiceCollection services)
        {
            services.AddSingleton<IValidationAttributeAdapterProvider, FoolProofValidationAttributeAdapterProvider>();
        }
    }
}
