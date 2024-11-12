using System;

namespace FoolProof.Core.Tests.Models
{
    public class Predicate
    {
        public class Model : ValidationModelBase<ModelAwareValidationAttribute>
        {
            public int? Value1 { get; set; }

            public int? Value2 { get; set; }

            public int? Value3 { get; set; }

            public int? Value4 { get; set; }

            [OrPredicate<
                EqualToAttribute, 
                OrPredicateAttribute<
                    AndPredicateAttribute<
                        GreaterThanAttribute, 
                        LessThanAttribute
                    >,
                    NotPredicateAttribute<EqualToAttribute>
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
            public class CustomPredicateAttribute: AndPredicateAttribute
            {
                public CustomPredicateAttribute()
                    : base(
                        new NotPredicateAttribute(
                            new EqualToAttribute(nameof(Value1))
                        ),
                        new AndPredicateAttribute(
                            new OrPredicateAttribute(
                                new LessThanOrEqualToAttribute(nameof(Value2)),
                                new GreaterThanOrEqualToAttribute(nameof(Value3))
                            ),
                            new NotPredicateAttribute(
                                new AndPredicateAttribute(
                                    new LessThanAttribute(nameof(Value4)),
                                    new GreaterThanAttribute(nameof(Value5))
                                )
                            )
                        )
                    ) { }
            }
        }
    }
}
