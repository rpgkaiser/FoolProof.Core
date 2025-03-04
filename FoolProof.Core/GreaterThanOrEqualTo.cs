
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

    public class GreaterThanOrEqualToAttribute<T> : IsAttribute<T>
    {
        public GreaterThanOrEqualToAttribute(T dependentValue)
            : base(Operator.GreaterThanOrEqualTo, dependentValue) { }

        public GreaterThanOrEqualToAttribute(
            T dependentValue,
            string defaultMessage
        ) : base(Operator.GreaterThanOrEqualTo, dependentValue, defaultMessage) { }
    }
}
