
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FoolProof.Core
{
    public enum LogicalOperator
    {
        And,
        Or, 
        Not
    }

    public abstract class PredicateBaseAttribute : ModelAwareValidationAttribute
    {
        protected PredicateBaseAttribute(
            LogicalOperator oper,
            ModelAwareValidationAttribute leftPart,
            ModelAwareValidationAttribute rightPart
        ) : this(oper, leftPart, rightPart, "{0} is invalid") { }

        protected PredicateBaseAttribute(
            LogicalOperator oper,
            ModelAwareValidationAttribute leftPart,
            ModelAwareValidationAttribute rightPart,
            string defaultMessage
        ) : base(defaultMessage)
        {
            Operator = oper;
            LeftPart = leftPart ?? throw new ArgumentNullException(nameof(rightPart));
            RightPart = oper != LogicalOperator.Not
                        ? rightPart ?? throw new ArgumentNullException(nameof(rightPart))
                        : null;
        }

        public LogicalOperator Operator { get; protected set; }

        public ModelAwareValidationAttribute LeftPart { get; protected set; }

        public ModelAwareValidationAttribute RightPart { get; protected set; }

        public override string ClientTypeName => "predicate";

        public override bool IsValid(object value, object container)
        {
            return Operator switch
            {
                LogicalOperator.And => LeftPart.IsValid(value, container)
                                        && RightPart.IsValid(value, container),
                LogicalOperator.Or => LeftPart.IsValid(value, container)
                                        || RightPart.IsValid(value, container),
                LogicalOperator.Not => !LeftPart.IsValid(value, container),
                _ => !LeftPart.IsValid(value, container),
            };
        }

        protected override IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters(ModelMetadata modelMetadata)
        {
            var clientParams = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("LogicalOperator", Operator.ToString()),
                new KeyValuePair<string, object>(
                    "LeftPart", 
                    new {
                        method = LeftPart.ClientTypeName,
                        @params = LeftPart.ClientValidationParameters(modelMetadata)
                    }
                ),
            };

            if (RightPart != null)
                clientParams.Add(
                    new KeyValuePair<string, object>(
                        "RightPart", 
                        new {
                            method = RightPart.ClientTypeName,
                            @params = RightPart.ClientValidationParameters(modelMetadata)
                        }
                    )
                );

            return base.GetClientValidationParameters(modelMetadata).Union(clientParams);
        }
    }

    public abstract class PredicateBaseAttribute<LPT, RPT> : PredicateBaseAttribute
        where LPT: ModelAwareValidationAttribute
        where RPT: ModelAwareValidationAttribute
    {
        private static LPT CreateLPT(object[] constParams)
            => Activator.CreateInstance(typeof(LPT), constParams) as LPT;

        private static RPT CreateRPT(object[] constParams)
            => Activator.CreateInstance(typeof(RPT), constParams) as RPT;

        protected PredicateBaseAttribute(
            LogicalOperator oper,
            LPT leftPart,
            RPT rightPart
        ): this(oper, leftPart, rightPart, "{0} is invalid") { }

        protected PredicateBaseAttribute(
            LogicalOperator oper,
            LPT leftPart,
            RPT rightPart,
            string defaultMessage
        ): base(oper, leftPart, rightPart, defaultMessage) { }

        protected PredicateBaseAttribute(
            LogicalOperator oper,
            object[] leftPartParams,
            object[] rightPartParams
        ): this(oper, leftPartParams, rightPartParams, "{0} is invalid") { }

        protected PredicateBaseAttribute(
            LogicalOperator oper,
            object[] leftPartParams,
            object[] rightPartParams,
            string defaultMessage
        ) : this(
                oper, 
                CreateLPT(leftPartParams), 
                oper != LogicalOperator.Not
                    ? CreateRPT(rightPartParams)
                    : null, 
                defaultMessage) { }

        public new LPT LeftPart 
        {
            get => (LPT)base.LeftPart;
            protected set => base.LeftPart = value;
        }

        public new RPT RightPart 
        { 
            get => (RPT)base.RightPart;
            protected set => base.RightPart = value;
        }
    }

    public class NotPredicateAttribute<LPT> : PredicateBaseAttribute<LPT, LPT>
        where LPT : ModelAwareValidationAttribute
    {
        public NotPredicateAttribute(
            LPT negatePart
        ) : this(negatePart, "{0} is invalid") { }

        public NotPredicateAttribute(
            LPT negatePart,
            string defaultMessage
        ) : base(LogicalOperator.Not, negatePart, null, "{0} is invalid") { }

        public NotPredicateAttribute(
            params object[] negatePartParams
        ) : this(negatePartParams, "{0} is invalid") { }

        public NotPredicateAttribute(
            object[] negatePartParams,
            string defaultMessage
        ) : base(LogicalOperator.Not, negatePartParams, null, defaultMessage) { }
    }

    public class NotPredicateAttribute : PredicateBaseAttribute
    {
        public NotPredicateAttribute(
            ModelAwareValidationAttribute negatePart
        ) : this(negatePart, "{0} is invalid") { }

        public NotPredicateAttribute(
            ModelAwareValidationAttribute negatePart,
            string defaultMessage
        ) : base(LogicalOperator.Not, negatePart, null, defaultMessage) { }
    }

    public class AndPredicateAttribute<LPT, RPT> : PredicateBaseAttribute<LPT, RPT>
        where LPT : ModelAwareValidationAttribute
        where RPT : ModelAwareValidationAttribute
    {
        protected AndPredicateAttribute(
            LPT leftPart,
            RPT rightPart
        ) : this(leftPart, rightPart, "{0} is invalid") { }

        protected AndPredicateAttribute(
            LPT leftPart,
            RPT rightPart,
            string defaultMessage
        ) : base(LogicalOperator.And, leftPart, rightPart, defaultMessage) { }

        public AndPredicateAttribute(
            object[] leftPartParams,
            object[] rightPartParams
        ) : this(leftPartParams, rightPartParams, "{0} is invalid") { }

        public AndPredicateAttribute(
            object[] leftPartParams,
            object[] rightPartParams,
            string defaultMessage
        ) : base(LogicalOperator.And, leftPartParams, rightPartParams, defaultMessage) { }
    }

    public class AndPredicateAttribute : PredicateBaseAttribute
    {
        public AndPredicateAttribute(
            ModelAwareValidationAttribute leftPart,
            ModelAwareValidationAttribute rightPart
        ) : this(leftPart, rightPart, "{0} is invalid") { }

        public AndPredicateAttribute(
            ModelAwareValidationAttribute leftPart,
            ModelAwareValidationAttribute rightPart,
            string defaultMessage
        ) : base(LogicalOperator.And, leftPart, rightPart, defaultMessage) { }
    }

    public class OrPredicateAttribute<LPT, RPT> : PredicateBaseAttribute<LPT, RPT>
        where LPT : ModelAwareValidationAttribute
        where RPT : ModelAwareValidationAttribute
    {
        protected OrPredicateAttribute(
            LPT leftPart,
            RPT rightPart
        ) : this(leftPart, rightPart, "{0} is invalid") { }

        protected OrPredicateAttribute(
            LPT leftPart,
            RPT rightPart,
            string defaultMessage
        ) : base(LogicalOperator.Or, leftPart, rightPart, defaultMessage) { }

        public OrPredicateAttribute(
            object[] leftPartParams,
            object[] rightPartParams
        ) : this(leftPartParams, rightPartParams, "{0} is invalid") { }

        public OrPredicateAttribute(
            object[] leftPartParams,
            object[] rightPartParams,
            string defaultMessage
        ) : base(LogicalOperator.Or, leftPartParams, rightPartParams, defaultMessage) { }
    }

    public class OrPredicateAttribute : PredicateBaseAttribute
    {
        public OrPredicateAttribute(
            ModelAwareValidationAttribute leftPart,
            ModelAwareValidationAttribute rightPart
        ) : this(leftPart, rightPart, "{0} is invalid") { }

        public OrPredicateAttribute(
            ModelAwareValidationAttribute leftPart,
            ModelAwareValidationAttribute rightPart,
            string defaultMessage
        ) : base(LogicalOperator.Or, leftPart, rightPart, defaultMessage) { }
    }
}
