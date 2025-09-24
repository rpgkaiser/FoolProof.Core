using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

                            return CompareValues(value,dependentValue) == 0;
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

                            return CompareValues(value, dependentValue) != 0;
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

                            return CompareValues(value, dependentValue) >= 1;
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

                            return CompareValues(value, dependentValue) <= -1;
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

                            return Get(Operator.EqualTo).IsValid(value, dependentValue) || CompareValues(value, dependentValue) >= 1;
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

                            return Get(Operator.EqualTo).IsValid(value, dependentValue) || CompareValues(value, dependentValue) <= -1;
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
							return (!valueList.Any() && value is null)
                                   || (valueList.Any() && valueList.Any(val => eqOperMtd.IsValid(value, val)));
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
                            return !valueList.Any() || valueList.All(val => !eqOperMtd.IsValid(value, val));
						}
					}
				}
			};
        }

        private static int CompareValues(object value, object dependentValue, bool convertIfRequired = true)
        {
            try
            {
                return Comparer<object>.Default.Compare(value, dependentValue);
            }
            catch
            {
                //Possible type mismatch
                if(value.GetType() != dependentValue.GetType() && convertIfRequired)
                {
                    object convDepValue;
                    var converter = TypeDescriptor.GetConverter(value.GetType());
                    if (converter is not null && converter.CanConvertFrom(dependentValue.GetType()))
                        convDepValue = converter.ConvertFrom(dependentValue);
                    else
                        convDepValue = Convert.ChangeType(dependentValue, value.GetType());

                    return Comparer<object>.Default.Compare(value, convDepValue);
                }
                throw;
            }
        }

        private static IEnumerable<object> GetValueList(object value)
        {
            if (value is null)
                return Array.Empty<object>();

            if (value is string)
                return new[] { value as string };

            if(value is IEnumerable valueList)
                return valueList.Cast<object>();

            return new object[] { value };
        }
    }
}
