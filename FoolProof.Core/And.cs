using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FoolProof.Core
{
    public class AndAttribute : PredicateAttribute
    {
        public AndAttribute(
            params ValidationAttribute[] operands
        ) : this("{0} is invalid", operands) { }

        public AndAttribute(
            string defaultMessage,
            params ValidationAttribute[] operands
        ) : base(LogicalOperator.And, defaultMessage, operands) 
        {
            if(operands.Count() < 2)
                throw new ArgumentException("Must be at least two operands.", nameof(operands));
        }
    }

    public class AndAttribute<OP1, OP2> : AndAttribute
        where OP1 : ValidationAttribute
        where OP2 : ValidationAttribute
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
        ) : this(leftPartParams, rightPartParams, "{0} is invalid") { }

        public AndAttribute(
            object[] leftPartParams,
            object[] rightPartParams,
            string defaultMessage
        ) : this(
                CreateOperand<OP1>(leftPartParams), 
                CreateOperand<OP2>(rightPartParams), 
                defaultMessage
            ) { }
    }

    public class AndAttribute<OP1, OP2, OP3> : AndAttribute
        where OP1 : ValidationAttribute
        where OP2 : ValidationAttribute
        where OP3 : ValidationAttribute
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
        ) : this(operand1Params, operand2Params, operand3Params, "{0} is invalid") { }

        public AndAttribute(
            object[] operand1Params,
            object[] operand2Params,
            object[] operand3Params,
            string defaultMessage
        ) : this(
                CreateOperand<OP1>(operand1Params),
                CreateOperand<OP2>(operand2Params),
                CreateOperand<OP3>(operand3Params),
                defaultMessage
            )
        { }
    }

    public class AndAttribute<OP1, OP2, OP3, OP4> : AndAttribute
        where OP1 : ValidationAttribute
        where OP2 : ValidationAttribute
        where OP3 : ValidationAttribute
        where OP4 : ValidationAttribute
    {
        protected AndAttribute(
            OP1 operand1,
            OP2 operand2,
            OP3 operand3,
            OP4 operand4
        ) : this(operand1, operand2, operand3, operand4, "{0} is invalid") { }

        protected AndAttribute(
            OP1 operand1,
            OP2 operand2,
            OP3 operand3,
            OP4 operand4,
            string defaultMessage
        ) : base(defaultMessage, operand1, operand2, operand3, operand4) { }

        public AndAttribute(
            object[] operand1Params,
            object[] operand2Params,
            object[] operand3Params,
            object[] operand4Params
        ) : this(operand1Params, operand2Params, operand3Params, operand4Params, "{0} is invalid") { }

        public AndAttribute(
            object[] operand1Params,
            object[] operand2Params,
            object[] operand3Params,
            object[] operand4Params,
            string defaultMessage
        ) : this(
                CreateOperand<OP1>(operand1Params),
                CreateOperand<OP2>(operand2Params),
                CreateOperand<OP3>(operand3Params),
                CreateOperand<OP4>(operand4Params),
                defaultMessage
            )
        { }
    }
}
