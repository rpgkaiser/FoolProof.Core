
namespace FoolProof.Core
{
    public class RequiredIfNotAttribute : RequiredIfAttribute
    {
        public RequiredIfNotAttribute(
            string dependentProperty, 
            object dependentValue
        ) : base(dependentProperty, Operator.NotEqualTo, dependentValue) { }

        public RequiredIfNotAttribute(
            string dependentProperty,
            object dependentValue,
            string defaultMessage
        ) : base(dependentProperty, Operator.NotEqualTo, dependentValue, defaultMessage) { }
    }
}
