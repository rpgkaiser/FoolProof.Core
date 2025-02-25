
namespace FoolProof.Core
{
    public class LessThanOrEqualToAttribute : IsAttribute
    {
        public LessThanOrEqualToAttribute(string dependentProperty) 
            : base(Operator.LessThanOrEqualTo, dependentProperty) { }

        public LessThanOrEqualToAttribute(
            string dependentProperty, 
            string defaultMessage
        ) : base(Operator.LessThanOrEqualTo, dependentProperty, defaultMessage) { }

        public LessThanOrEqualToAttribute(
            string dependentProperty, 
            string defaultMessage, 
            string targetPropName
        ) : base(Operator.LessThanOrEqualTo, dependentProperty, defaultMessage, targetPropName) { }
    }
}
