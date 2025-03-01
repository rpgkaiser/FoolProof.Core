using System;
using System.Linq;

namespace FoolProof.Core
{
    public class AndAttribute : PredicateAttribute
    {
        public AndAttribute(
            params ModelAwareValidationAttribute[] operands
        ) : this("{0} is invalid", operands) { }

        public AndAttribute(
            string defaultMessage,
            params ModelAwareValidationAttribute[] operands
        ) : base(LogicalOperator.And, defaultMessage, operands) 
        {
            if(operands.Count() < 2)
                throw new ArgumentException("Must be at least two operands.", nameof(operands));
        }
    }

    public class AndAttribute<OP1, OP2> : AndAttribute
        where OP1 : ModelAwareValidationAttribute
        where OP2 : ModelAwareValidationAttribute
    {
        protected AndAttribute(
            OP1 leftPart,
            OP2 rightPart
        ) : this(leftPart, rightPart, "{0} is invalid") { }

        protected AndAttribute(
            OP1 leftPart,
            OP2 rightPart,
            string defaultMessage
        ) : base(defaultMessage, leftPart, rightPart) { }

        public AndAttribute(
            object[] leftPartParams,
            object[] rightPartParams
        ) : this(CreateOperand<OP1>(leftPartParams), CreateOperand<OP2>(rightPartParams)) { }

        public AndAttribute(
            object[] leftPartParams,
            object[] rightPartParams,
            string defaultMessage
        ) : this(CreateOperand<OP1>(leftPartParams), CreateOperand<OP2>(rightPartParams), defaultMessage) { }
    }

    public class AndAttribute<OP1, OP2, OP3> : AndAttribute
        where OP1 : ModelAwareValidationAttribute
        where OP2 : ModelAwareValidationAttribute
        where OP3 : ModelAwareValidationAttribute
    {
        protected AndAttribute(
            OP1 operand1,
            OP2 operand2,
            OP3 operand3
        ) : this(operand1, operand2, operand3, "{0} is invalid") { }

        protected AndAttribute(
            OP1 operand1,
            OP2 operand2,
            OP3 operand3,
            string defaultMessage
        ) : base(defaultMessage, operand1, operand2, operand3) { }

        public AndAttribute(
            object[] operand1Params,
            object[] operand2Params,
            object[] operand3Params
        ) : this(CreateOperand<OP1>(operand1Params), CreateOperand<OP2>(operand2Params), CreateOperand<OP3>(operand3Params)) { }

        public AndAttribute(
            object[] operand1Params,
            object[] operand2Params,
            object[] operand3Params,
            string defaultMessage
        ) : this(CreateOperand<OP1>(operand1Params), CreateOperand<OP2>(operand2Params), CreateOperand<OP3>(operand3Params), defaultMessage) { }
    }
}
