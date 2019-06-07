using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace FoolProof.Core
{
	public static class Config
    {
        public static void Register(IServiceCollection services)
        {
			services.AddSingleton<IValidationAttributeAdapterProvider, FoolProofValidationAttributeAdapterProvider>();
		}
    }
}
