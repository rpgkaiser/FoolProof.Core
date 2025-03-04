
using System;

namespace FoolProof.Core
{
    public class InAttribute : IsAttribute
    {
        public InAttribute(string dependentProperty) 
            : base(Operator.In, dependentProperty) { }

        public InAttribute(
            string dependentProperty, 
            string defaultMessage
        ) : base(Operator.In, dependentProperty, defaultMessage) { }
    }

    public class InAttribute<T> : IsAttribute<T[]>
    {
        public InAttribute(params T[] dependentValues)
            : base(Operator.In, dependentValues ?? Array.Empty<T>()) 
        {
            if (dependentValues is null || dependentValues.Length == 0)
                throw new ArgumentException("Depenedent value list must not be empty.", nameof(dependentValues));
        }

        public InAttribute(
            string defaultMessage,
            params T[] dependentValues
        ) : base(Operator.In, dependentValues ?? Array.Empty<T>(), defaultMessage) 
        {
            if (dependentValues is null || dependentValues.Length == 0)
                throw new ArgumentException("Depenedent value list must not be empty.", nameof(dependentValues));
        }
    }
}
