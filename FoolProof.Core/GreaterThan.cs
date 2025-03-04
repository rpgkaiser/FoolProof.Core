
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
    }

    public class GreaterThanAttribute<T> : IsAttribute<T>
    {
        public GreaterThanAttribute(T dependentValue)
            : base(Operator.GreaterThan, dependentValue) { }

        public GreaterThanAttribute(
            T dependentValue,
            string defaultMessage
        ) : base(Operator.GreaterThan, dependentValue, defaultMessage) { }
    }
}
