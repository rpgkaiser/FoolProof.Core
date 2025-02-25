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

        protected ModelAwareValidationAttribute(string defaultMessage, string targetPropertyName) 
            : base(defaultMessage ?? "{0} is invalid.")
        {
            TargetPropertyName = targetPropertyName;
        }

        public abstract bool IsValid(object value, object container);

        public virtual string ClientTypeName
        {
            get { return this.GetType().Name.Replace("Attribute", ""); }
        }

        public virtual string TargetPropertyName { get; set; }

        public Dictionary<string, object> ClientValidationParameters(ModelMetadata modelMetadata)
        {
            return GetClientValidationParameters(modelMetadata)
                   .ToDictionary(kv => kv.Key.ToLower(), kv => kv.Value);
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(TargetPropertyName))
                value = GetPropertyValue(TargetPropertyName, validationContext.ObjectInstance);

            return IsValid(value, validationContext.ObjectInstance) 
                    ? ValidationResult.Success 
                    : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        protected virtual IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters(ModelMetadata modelMetadata)
        {
            var clientParams = new List<KeyValuePair<string, object>>();
            
            if (!string.IsNullOrWhiteSpace(TargetPropertyName))
                clientParams.Add(new KeyValuePair<string, object>("TargetPropertyName", TargetPropertyName));

            return clientParams;
        }

        protected virtual object GetPropertyValue(string propertyName, object container)
        {
            var currentType = container.GetType();
            var value = container;

            foreach (string namePart in propertyName.Split('.'))
            {
                var property = currentType.GetProperty(namePart);
                value = property.GetValue(value, null);
                currentType = property.PropertyType;
            }

            return value;
        }
    }
}
