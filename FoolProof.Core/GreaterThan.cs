
namespace FoolProof.Core
{
    public class GreaterThanAttribute : IsAttribute
    {
        public GreaterThanAttribute(string dependentProperty) 
            : base(Operator.GreaterThan, dependentProperty) { }

        public GreaterThanAttribute(
            string dependentProperty, 
            string defaultMessage
        ) : base(Operator.GreaterThan, dependentProperty, defaultMessage) { }

        public GreaterThanAttribute(
            string dependentProperty,
            string defaultMessage,
            string targetPropName
        ) : base(Operator.GreaterThan, dependentProperty, defaultMessage, targetPropName) { }
    }
}
