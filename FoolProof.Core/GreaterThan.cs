
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

        public GreaterThanAttribute(string dependentValue)
            : base(dependentValue, Operator.GreaterThan) { }

        public GreaterThanAttribute(
            string dependentValue,
            string defaultMessage
        ) : base(dependentValue, Operator.GreaterThan, defaultMessage) { }
    }

    //TODO: Add speficic class for string values, to avoid ambiguity during construction
}
