
namespace FoolProof.Core
{
    public class InAttribute : IsAttribute
    {
        public InAttribute(string dependentProperty) 
            : base(Operator.In, dependentProperty) { }

        public InAttribute(
            string dependentProperty, 
            string defaultMessage
        ) : base(Operator.In, dependentProperty, defaultMessage) { }

        public InAttribute(
            string dependentProperty,
            string defaultMessage,
            string targetPropName
        ) : base(Operator.In, dependentProperty, defaultMessage, targetPropName) { }
    }
}
