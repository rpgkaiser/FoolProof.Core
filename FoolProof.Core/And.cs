namespace FoolProof.Core
{
    public class AndAttribute<LPT, RPT> : PredicateAttribute<LPT, RPT>
        where LPT : ModelAwareValidationAttribute
        where RPT : ModelAwareValidationAttribute
    {
        protected AndAttribute(
            LPT leftPart,
            RPT rightPart
        ) : this(leftPart, rightPart, "{0} is invalid") { }

        protected AndAttribute(
            LPT leftPart,
            RPT rightPart,
            string defaultMessage
        ) : base(LogicalOperator.And, leftPart, rightPart, defaultMessage) { }

        protected AndAttribute(
            LPT leftPart,
            RPT rightPart,
            string defaultMessage,
            string targetPropName
        ) : base(LogicalOperator.And, leftPart, rightPart, defaultMessage, targetPropName) { }

        public AndAttribute(
            object[] leftPartParams,
            object[] rightPartParams
        ) : this(leftPartParams, rightPartParams, "{0} is invalid") { }

        public AndAttribute(
            object[] leftPartParams,
            object[] rightPartParams,
            string defaultMessage
        ) : base(LogicalOperator.And, leftPartParams, rightPartParams, defaultMessage) { }

        public AndAttribute(
            object[] leftPartParams,
            object[] rightPartParams,
            string defaultMessage,
            string targetPropName
        ) : base(LogicalOperator.And, leftPartParams, rightPartParams, defaultMessage, targetPropName) { }
    }

    public class AndAttribute : PredicateAttribute
    {
        public AndAttribute(
            ModelAwareValidationAttribute leftPart,
            ModelAwareValidationAttribute rightPart
        ) : this(leftPart, rightPart, "{0} is invalid") { }

        public AndAttribute(
            ModelAwareValidationAttribute leftPart,
            ModelAwareValidationAttribute rightPart,
            string defaultMessage
        ) : base(LogicalOperator.And, leftPart, rightPart, defaultMessage) { }

        public AndAttribute(
            ModelAwareValidationAttribute leftPart,
            ModelAwareValidationAttribute rightPart,
            string defaultMessage,
            string targetPropName
        ) : base(LogicalOperator.And, leftPart, rightPart, defaultMessage, targetPropName) { }
    }
}
