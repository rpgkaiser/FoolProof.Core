using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FoolProof.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ModelAwareValidationAttribute : ValidationAttribute
    {
        protected ModelAwareValidationAttribute() : this("{0} is invalid.")
        {
        }

		protected ModelAwareValidationAttribute(string defaultMessage) : base(defaultMessage)
		{
		}

        public abstract bool IsValid(object value, object container);

        public virtual string ClientTypeName
        {
            get { return this.GetType().Name.Replace("Attribute", ""); }
        }

        public Dictionary<string, object> ClientValidationParameters(ModelMetadata modelMetadata)
        {
            return GetClientValidationParameters(modelMetadata).ToDictionary(kv => kv.Key.ToLower(), kv => kv.Value);
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool validate = IsValid(value, validationContext.ObjectInstance);
            if (validate)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
        }

        protected virtual IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters(ModelMetadata modelMetadata)
        {
            return new KeyValuePair<string, object>[0];
        }
    }
}
