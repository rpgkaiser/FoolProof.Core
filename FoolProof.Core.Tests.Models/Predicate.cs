using System;
using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public class Predicate
    {
        [ModelPredicate(ErrorMessage = "Model predicate validation failed.")]
        public class Model : ValidationModelBase<ModelAwareValidationAttribute>
        {
            public int? Value1 { get; set; }

            public int? Value2 { get; set; }

            public int? Value3 { get; set; }

            public int? Value4 { get; set; }

            [Or
            <
                EqualToAttribute, 
                OrAttribute
                <
                    AndAttribute
                    <
                        GreaterThanAttribute, 
                        LessThanAttribute
                    >,
                    NotAttribute<EqualToAttribute>
                >
             >( // Value5 == Value1 || ((Value5 > Value2 && Value5 < Value3) || !(Value5 == Value4))
                new object[] { nameof(Value1) },
                new object[] {
                    new object[] {
                        new object[] { nameof(Value2) },
                        new object[] { nameof(Value3) }
                    },
                    new object[] {
                        new object[] { nameof(Value4) }
                    }
                }
            )]
            public int? Value5 { get; set; }

            [CustomPredicate]
            public int? Value6 { get; set; }

            // !(Value6 == Value1) && ((Value6 <= Value2 || Value6 >= Value3) && !(Value6 < Value5 && Value6 > Value4))
            public class CustomPredicateAttribute: AndAttribute
            {
                public CustomPredicateAttribute()
                    : base(
                        new NotAttribute(
                            new EqualToAttribute(nameof(Value1))
                        ),
                        new AndAttribute(
                            new OrAttribute(
                                new LessThanOrEqualToAttribute(nameof(Value2)),
                                new GreaterThanOrEqualToAttribute(nameof(Value3))
                            ),
                            new NotAttribute(
                                new AndAttribute(
                                    new LessThanAttribute(nameof(Value4)),
                                    new GreaterThanAttribute(nameof(Value5))
                                )
                            )
                        )
                    ) { }
            }

            // !(Value1 == Value2) || ((Value3 <= Value4 && Value5 >= Value6) || !(Value1 < Value2 && Value2 > Value3))
            public class ModelPredicateAttribute : OrAttribute
            {
                public ModelPredicateAttribute()
                    : base(
                        new NotAttribute(
                            new IsValidAttribute(
                                nameof(Value1),
                                new RangeAttribute(10, 20)
                            )
                        ),
                        new OrAttribute(
                            new AndAttribute(
                                new IsValidAttribute(
                                    nameof(Value3),
                                    new LessThanOrEqualToAttribute(nameof(Value4))
                                ),
                                new IsValidAttribute(
                                    nameof(Value5),
                                    new GreaterThanOrEqualToAttribute(nameof(Value6))
                                )
                            ),
                            new AndAttribute(
                                new IsValidAttribute(
                                    nameof(Value1),
                                    new LessThanAttribute(nameof(Value2))
                                ),
                                new IsValidAttribute(
                                    nameof(Value2),
                                    new GreaterThanAttribute(nameof(Value3))
                                )
                            )
                        )
                    )
                { }
            }
        }
    }
}
