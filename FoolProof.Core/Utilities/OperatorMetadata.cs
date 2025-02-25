using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FoolProof.Core
{
    public class OperatorMetadata
    {
        public string ErrorMessage { get; set; }
        public Func<object, object, bool> IsValid { get; set; }

        static OperatorMetadata()
        {
            CreateOperatorMetadata();
        }

        private static Dictionary<Operator, OperatorMetadata> _operatorMetadata;

        public static OperatorMetadata Get(Operator @operator)
        {
            return _operatorMetadata[@operator];
        }

        private static void CreateOperatorMetadata()
        {
            _operatorMetadata = new Dictionary<Operator, OperatorMetadata>()
            {
                {
                    Operator.EqualTo, new OperatorMetadata()
                    {
                        ErrorMessage = "equal to",
                        IsValid = (value, dependentValue) => {
                            if (value == null && dependentValue == null)
                                return true;
                            else if (value == null && dependentValue != null)
                                return false;

                            return value.Equals(dependentValue);
                        }
                    }
                },
                {
                    Operator.NotEqualTo, new OperatorMetadata()
                    {
                        ErrorMessage = "not equal to",
                        IsValid = (value, dependentValue) => {
                            if (value == null && dependentValue != null)
                                return true;
                            else if (value == null && dependentValue == null)
                                return false;

                            return !value.Equals(dependentValue);
                        }
                    }
                },
                {
                    Operator.GreaterThan, new OperatorMetadata()
                    {
                        ErrorMessage = "greater than",
                        IsValid = (value, dependentValue) => {
                            if (value == null || dependentValue == null)
                                return false;

                            return Comparer<object>.Default.Compare(value, dependentValue) >= 1;
                        }
                    }
                },
                {
                    Operator.LessThan, new OperatorMetadata()
                    {
                        ErrorMessage = "less than",
                        IsValid = (value, dependentValue) => {
                            if (value == null || dependentValue == null)
                                return false;

                            return Comparer<object>.Default.Compare(value, dependentValue) <= -1;
                        }
                    }
                },
                {
                    Operator.GreaterThanOrEqualTo, new OperatorMetadata()
                    {
                        ErrorMessage = "greater than or equal to",
                        IsValid = (value, dependentValue) => {
                            if (value == null && dependentValue == null)
                                return true;

                            if (value == null || dependentValue == null)
                                return false;

                            return Get(Operator.EqualTo).IsValid(value, dependentValue) || Comparer<object>.Default.Compare(value, dependentValue) >= 1;
                        }
                    }
                },
                {
                    Operator.LessThanOrEqualTo, new OperatorMetadata()
                    {
                        ErrorMessage = "less than or equal to",
                        IsValid = (value, dependentValue) => {
                            if (value == null && dependentValue == null)
                                return true;

                            if (value == null || dependentValue == null)
                                return false;

                            return Get(Operator.EqualTo).IsValid(value, dependentValue) || Comparer<object>.Default.Compare(value, dependentValue) <= -1;
                        }
                    }
                },
                {
                    Operator.RegExMatch, new OperatorMetadata()
                    {
                        ErrorMessage = "a match to",
                        IsValid = (value, dependentValue) => {
                            return Regex.IsMatch((value ?? "").ToString(), dependentValue.ToString());
                        }
                    }
                },
                {
                    Operator.NotRegExMatch, new OperatorMetadata()
                    {
                        ErrorMessage = "not a match to",
                        IsValid = (value, dependentValue) => {
                            return !Regex.IsMatch((value ?? "").ToString(), dependentValue.ToString());
                        }
                    }
                },
				{
					Operator.In, new OperatorMetadata()
					{
						ErrorMessage = "in",
						IsValid = (value, dependentValue) => {
							var eqOperMtd = Get(Operator.EqualTo);
                            var valueList = GetValueList(dependentValue);
							return valueList.Any(val => eqOperMtd.IsValid(value, val));
						}
					}
				},
				{
					Operator.NotIn, new OperatorMetadata()
					{
						ErrorMessage = "not in",
						IsValid = (value, dependentValue) => {
							var eqOperMtd = Get(Operator.EqualTo);
                            var valueList = GetValueList(dependentValue);
                            return valueList.All(val => !eqOperMtd.IsValid(value, val));
						}
					}
				}
			};
        }

        private static IEnumerable<object> GetValueList(object value)
        {
            if (value is string)
                return new[] { value as string };

            if(value is IEnumerable valueList)
                return valueList.Cast<object>();

            return new object[] { value };
        }
    }
}
