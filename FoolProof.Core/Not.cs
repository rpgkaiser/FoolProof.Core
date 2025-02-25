namespace FoolProof.Core
{
    public class NotAttribute<LPT> : PredicateAttribute<LPT, LPT>
        where LPT : ModelAwareValidationAttribute
    {
        public NotAttribute(
            LPT negatePart
        ) : this(negatePart, "{0} is invalid") { }

        public NotAttribute(
            LPT negatePart,
            string defaultMessage
        ) : base(LogicalOperator.Not, negatePart, null, defaultMessage) { }

        public NotAttribute(
            LPT negatePart,
            string defaultMessage,
            string targetPropName
        ) : base(LogicalOperator.Not, negatePart, null, defaultMessage, targetPropName) { }

        public NotAttribute(
            params object[] negatePartParams
        ) : this(negatePartParams, "{0} is invalid") { }

        public NotAttribute(
            object[] negatePartParams,
            string defaultMessage
        ) : base(LogicalOperator.Not, negatePartParams, null, defaultMessage) { }

        public NotAttribute(
            object[] negatePartParams,
            string defaultMessage,
            string targetPropName
        ) : base(LogicalOperator.Not, negatePartParams, null, defaultMessage, targetPropName) { }
    }

    public class NotAttribute : PredicateAttribute
    {
        public NotAttribute(
            ModelAwareValidationAttribute negatePart
        ) : this(negatePart, "{0} is invalid") { }

        public NotAttribute(
            ModelAwareValidationAttribute negatePart,
            string defaultMessage
        ) : base(LogicalOperator.Not, negatePart, null, defaultMessage) { }

        public NotAttribute(
            ModelAwareValidationAttribute negatePart,
            string defaultMessage,
            string targetPropName
        ) : base(LogicalOperator.Not, negatePart, null, defaultMessage, targetPropName) { }
    }
}
