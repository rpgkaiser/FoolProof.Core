
using System;

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
    }

    public class NotInAttribute<T> : IsAttribute<T[]>
    {
        public NotInAttribute(params T[] dependentValues)
            : base(Operator.NotIn, dependentValues ?? Array.Empty<T>()) { }

        public NotInAttribute(
            string defaultMessage,
            params T[] dependentValues
        ) : base(Operator.NotIn, dependentValues ?? Array.Empty<T>(), defaultMessage) { }
    }
}
