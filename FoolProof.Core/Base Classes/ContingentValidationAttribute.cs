using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FoolProof.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ContingentValidationAttribute : ModelAwareValidationAttribute
    {
        public string DependentProperty { get; private set; }

        public string DependentPropertyDisplayName { get; set; }

        public ContingentValidationAttribute(string dependentProperty) : this(dependentProperty, "{0} is invalid due to {1}.")
        {
        }

        public ContingentValidationAttribute(string dependentProperty, string defaultMessage) : base(defaultMessage)
        {
            DependentProperty = dependentProperty;
        }

        public ContingentValidationAttribute(string dependentProperty, string defaultMessage, string targetPropertyName) 
            : base(defaultMessage, targetPropertyName)
        {
            DependentProperty = dependentProperty;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, DependentPropertyDisplayName ?? DependentProperty);
        }

        public override bool IsValid(object value, object container)
        {
            return IsValid(value, GetPropertyValue(DependentProperty, container), container);
        }

        public abstract bool IsValid(object value, object dependentValue, object container);


		protected override IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters(ModelMetadata modelMetadata)
		{
			return base.GetClientValidationParameters(modelMetadata)
				.Union(new[] { new KeyValuePair<string, object>("DependentProperty", DependentProperty) });
		}
	}
}
