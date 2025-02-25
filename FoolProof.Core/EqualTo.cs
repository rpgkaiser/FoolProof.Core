
namespace FoolProof.Core
{
    public class EqualToAttribute : IsAttribute
    {
        public EqualToAttribute(string dependentProperty) 
            : base(Operator.EqualTo, dependentProperty) { }

        public EqualToAttribute(
            string dependentProperty, 
            string defaultMessage
        ) : base(Operator.EqualTo, dependentProperty, defaultMessage) { }

        public EqualToAttribute(
            string dependentProperty,
            string defaultMessage,
            string targetPropName
        ) : base(Operator.EqualTo, dependentProperty, defaultMessage, targetPropName) { }
    }
}
