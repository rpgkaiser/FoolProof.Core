using System;
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
			IStringLocalizer stringLocalizer
        )
		{
			if (attribute is ModelAwareValidationAttribute modelAwareValidAttrb)
				return new FoolProofValidationAdapter(modelAwareValidAttrb, stringLocalizer);
			
            return base.GetAttributeAdapter(attribute, stringLocalizer);
		}
	}
}
