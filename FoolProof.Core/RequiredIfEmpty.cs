
namespace FoolProof.Core
{
    public class RequiredIfEmptyAttribute : ContingentValidationAttribute
    {
        public RequiredIfEmptyAttribute(string dependentProperty)
            : base(dependentProperty) { }

        public override bool IsValid(object value, object dependentValue, object container)
        {
            if (string.IsNullOrEmpty((dependentValue ?? string.Empty).ToString().Trim()))
                return value != null && !string.IsNullOrEmpty(value.ToString().Trim());

            return true;
        }

        public override string DefaultErrorMessage
        {
            get { return "{0} is required due to {1} being empty."; }
        }
    }
}
