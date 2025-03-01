
namespace FoolProof.Core
{
    public class LessThanAttribute : IsAttribute
    {
        public LessThanAttribute(string dependentProperty) 
            : base(Operator.LessThan, dependentProperty) { }

        public LessThanAttribute(
            string dependentProperty, 
            string defaultMessage
        ) : base(Operator.LessThan, dependentProperty, defaultMessage) { }
    }
}
