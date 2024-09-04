using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FoolProof.Core
{
    public class RegularExpressionIfAttribute : RequiredIfAttribute
    {
        public string Pattern { get; set; }

        public RegularExpressionIfAttribute(string pattern, string dependentProperty, Operator @operator, object dependentValue)
            : base(dependentProperty, @operator, dependentValue)
        {
            Pattern = pattern;
            DataType = ClientDataType.String;
        }

        public RegularExpressionIfAttribute(string pattern, string dependentProperty, object dependentValue)
            : this(pattern, dependentProperty, Operator.EqualTo, dependentValue) { }

        public override bool IsValid(object value, object dependentValue, object container)
        {
            if (Metadata.IsValid(dependentValue, DependentValue))
                return OperatorMetadata.Get(Operator.RegExMatch).IsValid(value, Pattern);

            return true;
        }

        protected override IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters(ModelMetadata modelMetadata)
        {
            return base.GetClientValidationParameters(modelMetadata)
                .Union(new[] {
                    new KeyValuePair<string, object>("Pattern", Pattern),
                });
        }

        public override string FormatErrorMessage(string name)
        {
            if (string.IsNullOrEmpty(ErrorMessageResourceName) && string.IsNullOrEmpty(ErrorMessage))
                ErrorMessage = DefaultErrorMessage;

            return string.Format(ErrorMessageString, name, DependentProperty, DependentValue, Pattern);
        }

        public override string DefaultErrorMessage
        {
            get { return "{0} must be in the format of {3} due to {1} being " + Metadata.ErrorMessage + " {2}"; }
        }
    }
}
