using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

namespace FoolProof.Core
{
    public class FoolProofValidationAdapter : AttributeAdapterBase<ModelAwareValidationAttribute>
    {
        public FoolProofValidationAdapter(ModelAwareValidationAttribute attribute, IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer) { }

		public override void AddValidation(ClientModelValidationContext context)
		{
			if (Attribute is ContingentValidationAttribute contingAttr)
			{
				var otherPropertyInfo = context.ModelMetadata.ContainerType.GetProperty(contingAttr.DependentProperty);

				var displayName = GetMetaDataDisplayName(otherPropertyInfo);
				if (displayName != null)
					contingAttr.DependentPropertyDisplayName = displayName;
			}

			var validName = Attribute.ClientTypeName.ToLowerInvariant();
			//Add validation rule attributes
			MergeAttribute(context.Attributes, "data-val", "true");
			MergeAttribute(context.Attributes, $"data-val-{validName}", GetErrorMessage(context));

			//Add validation params attributes
			foreach (var validationParam in Attribute.ClientValidationParameters(context.ModelMetadata))
				MergeAttribute(
					context.Attributes, 
					$"data-val-{validName}-{validationParam.Key.ToLowerInvariant()}",
					validationParam.Value != null && validationParam.Value.GetType() != typeof(string) 
						? JsonSerializer.Serialize(validationParam.Value) 
						: validationParam.Value as string
				);
		}

		public override string GetErrorMessage(ModelValidationContextBase validationContext)
		{
			return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName());
		}


		protected virtual string GetAttributeDisplayName(PropertyInfo property)
        {
            var result = property.GetCustomAttributes<DisplayAttribute>(true).FirstOrDefault()?.GetName()
                         ?? property.GetCustomAttributes<DisplayNameAttribute>(true).FirstOrDefault()?.DisplayName;
            return result;
        }

        protected virtual string GetMetaDataDisplayName(PropertyInfo property)
        {
            var attrs = property.DeclaringType.GetCustomAttributes<ModelMetadataTypeAttribute>(true);
            if (attrs.Any())
                property = attrs.First().MetadataType.GetProperty(property.Name) ?? property;

            return GetAttributeDisplayName(property);
        }
    }
}
