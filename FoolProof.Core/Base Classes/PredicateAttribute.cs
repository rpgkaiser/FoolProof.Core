
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

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public abstract class PredicateAttribute : ModelAwareValidationAttribute
    {
        protected PredicateAttribute(
            LogicalOperator oper,
            ModelAwareValidationAttribute leftPart,
            ModelAwareValidationAttribute rightPart
        ) : this(oper, leftPart, rightPart, "{0} is invalid") { }

        protected PredicateAttribute(
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

        protected PredicateAttribute(
            LogicalOperator oper,
            ModelAwareValidationAttribute leftPart,
            ModelAwareValidationAttribute rightPart,
            string defaultMessage,
            string targetPropertyName
        ) : base(defaultMessage, targetPropertyName)
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
                _ => throw new NotSupportedException()
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

    public abstract class PredicateAttribute<LPT, RPT> : PredicateAttribute
        where LPT: ModelAwareValidationAttribute
        where RPT: ModelAwareValidationAttribute
    {
        private static LPT CreateLPT(object[] constParams)
            => Activator.CreateInstance(typeof(LPT), constParams) as LPT;

        private static RPT CreateRPT(object[] constParams)
            => Activator.CreateInstance(typeof(RPT), constParams) as RPT;

        protected PredicateAttribute(
            LogicalOperator oper,
            LPT leftPart,
            RPT rightPart
        ): this(oper, leftPart, rightPart, "{0} is invalid") { }

        protected PredicateAttribute(
            LogicalOperator oper,
            LPT leftPart,
            RPT rightPart,
            string defaultMessage
        ): base(oper, leftPart, rightPart, defaultMessage) { }

        protected PredicateAttribute(
            LogicalOperator oper,
            LPT leftPart,
            RPT rightPart,
            string defaultMessage,
            string targetPropName
        ) : base(oper, leftPart, rightPart, defaultMessage, targetPropName) { }

        protected PredicateAttribute(
            LogicalOperator oper,
            object[] leftPartParams,
            object[] rightPartParams
        ): this(oper, leftPartParams, rightPartParams, "{0} is invalid") { }

        protected PredicateAttribute(
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
                defaultMessage
            ) { }

        protected PredicateAttribute(
            LogicalOperator oper,
            object[] leftPartParams,
            object[] rightPartParams,
            string defaultMessage,
            string targetPropName
        ) : this(
                oper,
                CreateLPT(leftPartParams),
                oper != LogicalOperator.Not
                    ? CreateRPT(rightPartParams)
                    : null,
                defaultMessage,
                targetPropName
            )
        { }

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
}
