
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

        public GreaterThanOrEqualToAttribute(string dependentValue)
            : base(dependentValue, Operator.GreaterThanOrEqualTo) { }

        public GreaterThanOrEqualToAttribute(
            string dependentValue,
            string defaultMessage
        ) : base(dependentValue, Operator.GreaterThanOrEqualTo, defaultMessage) { }
    }

    //TODO: Add speficic class for string values, to avoid ambiguity during construction
}
