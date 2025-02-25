namespace FoolProof.Core
{
    public class OrAttribute<LPT, RPT> : PredicateAttribute<LPT, RPT>
        where LPT : ModelAwareValidationAttribute
        where RPT : ModelAwareValidationAttribute
    {
        protected OrAttribute(
            LPT leftPart,
            RPT rightPart
        ) : this(leftPart, rightPart, "{0} is invalid") { }

        protected OrAttribute(
            LPT leftPart,
            RPT rightPart,
            string defaultMessage
        ) : base(LogicalOperator.Or, leftPart, rightPart, defaultMessage) { }

        protected OrAttribute(
            LPT leftPart,
            RPT rightPart,
            string defaultMessage,
            string targetPropName
        ) : base(LogicalOperator.Or, leftPart, rightPart, defaultMessage, targetPropName) { }

        public OrAttribute(
            object[] leftPartParams,
            object[] rightPartParams
        ) : this(leftPartParams, rightPartParams, "{0} is invalid") { }

        public OrAttribute(
            object[] leftPartParams,
            object[] rightPartParams,
            string defaultMessage
        ) : base(LogicalOperator.Or, leftPartParams, rightPartParams, defaultMessage) { }

        public OrAttribute(
            object[] leftPartParams,
            object[] rightPartParams,
            string defaultMessage,
            string targetPropName
        ) : base(LogicalOperator.Or, leftPartParams, rightPartParams, defaultMessage, targetPropName) { }
    }

    public class OrAttribute : PredicateAttribute
    {
        public OrAttribute(
            ModelAwareValidationAttribute leftPart,
            ModelAwareValidationAttribute rightPart
        ) : this(leftPart, rightPart, "{0} is invalid") { }

        public OrAttribute(
            ModelAwareValidationAttribute leftPart,
            ModelAwareValidationAttribute rightPart,
            string defaultMessage
        ) : base(LogicalOperator.Or, leftPart, rightPart, defaultMessage) { }

        public OrAttribute(
            ModelAwareValidationAttribute leftPart,
            ModelAwareValidationAttribute rightPart,
            string defaultMessage,
            string targetPropName
        ) : base(LogicalOperator.Or, leftPart, rightPart, defaultMessage, targetPropName) { }
    }
}
