
namespace FoolProof.Core
{
    public class RequiredIfNotEmptyAttribute : ContingentValidationAttribute
    {
        public RequiredIfNotEmptyAttribute(string dependentProperty)
            : base(dependentProperty, "{0} is required due to {1} not being empty.") { }

        public RequiredIfNotEmptyAttribute(
            string dependentProperty,
            string defaultMessage
        ) : base(dependentProperty, defaultMessage ?? "{0} is required due to {1} not being empty.") { }

        public RequiredIfNotEmptyAttribute(
            string dependentProperty, 
            string defaultMessage,
            string targetPropName
        ) : base(dependentProperty, defaultMessage ?? "{0} is required due to {1} not being empty.", targetPropName) { }

        public override bool IsValid(object value, object dependentValue, object container)
        {
            if (!string.IsNullOrEmpty((dependentValue ?? string.Empty).ToString().Trim()))
                return value != null && !string.IsNullOrEmpty(value.ToString().Trim());

            return true;
        }
    }
}
