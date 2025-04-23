
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

    public class LessThanAttribute<T> : IsAttribute<T>
    {
        public LessThanAttribute(T dependentValue)
            : base(Operator.LessThan, dependentValue) { }

        public LessThanAttribute(
            T dependentValue,
            string defaultMessage
        ) : base(Operator.LessThan, dependentValue, defaultMessage) { }

        public LessThanAttribute(string dependentValue)
            : base(dependentValue, Operator.LessThan) { }

        public LessThanAttribute(
            string dependentValue,
            string defaultMessage
        ) : base(dependentValue, Operator.LessThan, defaultMessage) { }
    }

    //TODO: Add speficic class for string values, to avoid ambiguity during construction
}
