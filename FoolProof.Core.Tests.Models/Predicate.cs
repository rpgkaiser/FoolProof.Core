using System;
using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public class Predicate
    {
        [ModelPredicate(ErrorMessage = "Personal information validation failed.")]
        public class Model : ValidationModelBase<ModelAwareValidationAttribute>
        {
            [Required]
            public string? FirstName { get; set; }

            [Required]
            public string? LastName { get; set; }

            [Required]
            public string? Email { get; set; }

            [Required]
            public int? Age { get; set; }

            [Required]
            public int? YearsOfStudy { get; set; }

            // !Value || YearsOfStudy >= 6
            [Or<IsFalseAttribute, IsValidAttribute<IsAttribute<int>>>(
                null,
                new object[] { 
                    nameof(YearsOfStudy), 
                    new object[] { 
                        Operator.GreaterThanOrEqualTo,
                        6
                    } 
                },
                "Years of study do not cover the elementary school duration."
            )]
            public bool ElementarySchool { get; set; }

            // !Value || (ElementarySchool && YearsOfStudy >= 10)
            [Or<IsFalseAttribute, AndAttribute<IsValidAttribute<IsTrueAttribute>, IsValidAttribute<IsAttribute<int>>>>(
                null,
                new object[] {
                    new object[] {
                        nameof(ElementarySchool)
                    },
                    new object[] {
                        nameof(YearsOfStudy),
                        new object[] {
                            Operator.GreaterThanOrEqualTo,
                            10
                        }
                    }
                },
                "No elementary school or years of study do not cover the high school duration."
            )]
            public bool HighSchool { get; set; }

            // !Value || (HighSchool && YearsOfStudy >= 14)
            [Or<IsFalseAttribute, AndAttribute<IsValidAttribute<IsTrueAttribute>, IsValidAttribute<IsAttribute<int>>>>(
                null,
                new object[] {
                    new object[] {
                        nameof(HighSchool)
                    },
                    new object[] {
                        nameof(YearsOfStudy),
                        new object[] {
                            Operator.GreaterThanOrEqualTo,
                            14
                        }
                    }
                },
                "No high school or years of study do not cover the university duration."
            )]
            public bool University { get; set; }

            public string? Country { get; set; }

            // !Value || (HighSchool && YearsOfStudy >= 14)
            [Or<
                IsEmptyAttribute, 
                AndAttribute<
                    IsValidAttribute<SameTextAttribute>, 
                    IsValidAttribute<RegularExpressionAttribute>
                >,
                AndAttribute<
                    IsValidAttribute<SameTextAttribute>,
                    IsValidAttribute<RegularExpressionAttribute>
                >,
                AndAttribute<
                    IsValidAttribute<SameTextAttribute>,
                    IsValidAttribute<RegularExpressionAttribute>
                >
             >(
                null,
                new object[] {
                    new object[] {
                        nameof(Country),
                        new object[]{ "US" }
                    },
                    new object[] {
                        nameof(PhoneNumber),
                        new object[] { @"^\s*\+1\s*(\d\s*){10,}$" }
                    },
                    "Phone numbers in USA starts with +1 and at least 10 more digits."
                },
                new object[] {
                    new object[] {
                        nameof(Country),
                        new object[]{ "ES" }
                    },
                    new object[] {
                        nameof(PhoneNumber),
                        new object[] { @"^\s*\+34\s*(\d\s*){9,}$" }
                    },
                    "Phone numbers in Spain starts with +34 and at least 9 more digits."
                },
                new object[] {
                    new object[] {
                        nameof(Country),
                        new object[]{ "CU" }
                    },
                    new object[] {
                        nameof(PhoneNumber),
                        new object[] { @"^\s*\+53\s*(\d\s*){8,}$" }
                    },
                    "Phone numbers in Cuba starts with +53 and at least 8 more digits."
                },
                "Invalid international phone number format."
            )]
            [Phone]
            public string? PhoneNumber { get; set; }

            // (1 <= FirstName.Length <= 50 || 1 <= LastName.Length <= 50) && (Email Is Valid) && (Age In Range [5, 120])
            public class ModelPredicateAttribute : AndAttribute
            {
                public ModelPredicateAttribute()
                    : base(
                        new OrAttribute(
                            new AndAttribute(
                                new IsValidAttribute(
                                    nameof(FirstName),
                                    new MinLengthAttribute(3)
                                ),
                                new IsValidAttribute(
                                    nameof(FirstName),
                                    new MaxLengthAttribute(50)
                                )
                            ),
                            new IsValidAttribute(
                                nameof(LastName),
                                new StringLengthAttribute(20) { 
                                    MinimumLength = 5
                                }
                            )
                        ),
                        new IsValidAttribute(
                            nameof(Email),
                            new EmailAddressAttribute()
                        ),
                        new IsValidAttribute(
                            nameof(Age),
                            new RangeAttribute(5, 120)
                        ),
                        new IsValidAttribute(
                            nameof(Age),
                            new GreaterThanAttribute(
                                nameof(YearsOfStudy)
                            )
                        )
                    )
                { }
            }
        }
    }
}
