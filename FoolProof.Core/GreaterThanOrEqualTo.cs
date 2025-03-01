
namespace FoolProof.Core
{
    public class GreaterThanOrEqualToAttribute : IsAttribute
    {
        public GreaterThanOrEqualToAttribute(string dependentProperty) 
            : base(Operator.GreaterThanOrEqualTo, dependentProperty) { }

        public GreaterThanOrEqualToAttribute(
            string dependentProperty, 
            string defaultMessage
        ) : base(Operator.GreaterThanOrEqualTo, dependentProperty, defaultMessage) { }
    }
}
