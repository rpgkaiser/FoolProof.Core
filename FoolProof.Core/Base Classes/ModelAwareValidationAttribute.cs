using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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

        public virtual Dictionary<string, object> ClientValidationParameters(ClientModelValidationContext validationContext)
        {
            return ClientValidationParameters(validationContext.ModelMetadata);
        }

        public virtual Dictionary<string, object> ClientValidationParameters(ModelMetadata modelMetadata)
        {
            return GetClientValidationParameters(modelMetadata)
                   .ToDictionary(kv => kv.Key.ToLower(), kv => kv.Value);
        }

        public virtual string FormatErrorMessage(ClientModelValidationContext validationContext)
            => FormatErrorMessage(validationContext.ModelMetadata.GetDisplayName());

        public virtual string FormatErrorMessage(ValidationContext validationContext)
            => FormatErrorMessage(validationContext.DisplayName);


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return IsValid(value, validationContext.ObjectInstance) 
                    ? ValidationResult.Success 
                    : new ValidationResult(FormatErrorMessage(validationContext));
        }

        protected virtual IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters(ModelMetadata modelMetadata)
        {
            return Enumerable.Empty<KeyValuePair<string, object>>();
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

        public static PropertyInfo GetModelProperty(Type modelType, string propertyName)
        {
            PropertyInfo result = null;
            foreach (string namePart in propertyName.Split('.'))
            {
                result = modelType.GetProperty(namePart);
                if (result is null)
                    break;

                modelType = result.PropertyType;
            }

            return result;
        }

        private static readonly HashSet<Type> NumericTypes = new HashSet<Type>
        {
            typeof(int),  typeof(double),  typeof(decimal),
            typeof(long), typeof(short),   typeof(sbyte),
            typeof(byte), typeof(ulong),   typeof(ushort),
            typeof(uint), typeof(float)
        };

        protected static bool IsNumeric(Type myType)
        {
            return NumericTypes.Contains(myType);
        }
    }
}
