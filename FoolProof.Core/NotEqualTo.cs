
namespace FoolProof.Core
{
    public class NotEqualToAttribute : IsAttribute
    {
        public NotEqualToAttribute(string dependentProperty) 
            : base(Operator.NotEqualTo, dependentProperty) { }

        public NotEqualToAttribute(
            string dependentProperty, 
            string defultMessage
        ) : base(Operator.NotEqualTo, dependentProperty, defultMessage) { }

        public NotEqualToAttribute(
            string dependentProperty, 
            string defultMessage, 
            string targetPropName
        ) : base(Operator.NotEqualTo, dependentProperty, defultMessage, targetPropName) { }
    }
}
