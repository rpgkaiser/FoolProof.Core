
namespace FoolProof.Core
{
    public class NotInAttribute : IsAttribute
    {
        public NotInAttribute(string dependentProperty) 
            : base(Operator.NotIn, dependentProperty) { }

        public NotInAttribute(
            string dependentProperty, 
            string defaultMessage
        ) : base(Operator.NotIn, dependentProperty) { }

        public NotInAttribute(
            string dependentProperty, 
            string defaultMessage, 
            string targetPropName
        ) : base(Operator.NotIn, dependentProperty, defaultMessage, targetPropName) { }
    }
}
