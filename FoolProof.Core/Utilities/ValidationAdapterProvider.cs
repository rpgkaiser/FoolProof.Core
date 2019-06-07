using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;

namespace FoolProof.Core
{
	class FoolProofValidationAttributeAdapterProvider : ValidationAttributeAdapterProvider, IValidationAttributeAdapterProvider
	{
		public FoolProofValidationAttributeAdapterProvider() { }

		IAttributeAdapter IValidationAttributeAdapterProvider.GetAttributeAdapter(
			ValidationAttribute attribute,
			IStringLocalizer stringLocalizer)
		{
			IAttributeAdapter adapter;
			if (attribute is ModelAwareValidationAttribute modelAwareValidAttrb)
				adapter = new FoolProofValidationAdapter(modelAwareValidAttrb, stringLocalizer);
			else
				adapter = GetAttributeAdapter(attribute, stringLocalizer);

			return adapter;
		}
	}
}
