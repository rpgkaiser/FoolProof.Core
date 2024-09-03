using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FoolProof.Core.Tests.E2eTests
{
    public abstract class LessThanTest: CompareBaseTest
    {
        protected override Regex PageTitleRegex() => new($@".+\s+[-]\s+LessThan\s+\({DataType}\)");

        protected override Uri PageUri() => new(new Uri(WebAppUrl), $"lessthan/{DataType}");

        protected override string Value2ValidationError => "Value2 must be less than Value1";

        [TestClass]
        public class DateValues : LessThanTest
        {
            protected override string DataType => "Date";

            protected override (string Value1, string Value2) GetValues2PassCompare()
            {
                return ("10/10/2010", "5/5/2005");
            }

            protected override (string Value1, string Value2) GetValues2FailsCompare()
            {
                return ("5/5/2005", "10/10/2010");
            }
        }

        [TestClass]
        public class Int16Values : LessThanTest
        {
            protected override string DataType => "Int16";

            protected override (string Value1, string Value2) GetValues2PassCompare()
            {
                return ("55", "11");
            }

            protected override (string Value1, string Value2) GetValues2FailsCompare()
            {
                return ("22", "88");
            }
        }

        [TestClass]
        public class TimeValues : LessThanTest
        {
            protected override string DataType => "Time";

            protected override (string Value1, string Value2) GetValues2PassCompare()
            {
                return ("20:30", "10:00");
            }

            protected override (string Value1, string Value2) GetValues2FailsCompare()
            {
                return ("08:15", "23:00");
            }
        }
    }

    public abstract class LessThanTest_PassWithNull : CompareBaseTest_PassWithNull
    {
        protected override Regex PageTitleRegex() => new($@".+\s+[-]\s+LessThan\s+\({DataType}\)");

        protected override Uri PageUri() => new(new Uri(WebAppUrl), $"lessthan/{DataType}?pwn=true");

        protected override string Value2ValidationError => "Value2 must be less than Value1";

        [TestMethod("Value2LessThanValue1_Valid")]
        public override Task CompareValuesPass()
        {
            return base.CompareValuesPass();
        }

        [TestMethod("Value1LessThanValue2_Invalid")]
        public override Task CompareValuesFails()
        {
            return base.CompareValuesFails();
        }

        [TestClass]
        public class DateValues : LessThanTest_PassWithNull
        {
            protected override string DataType => "Date";

            protected override (string Value1, string Value2) GetValues2PassCompare()
            {
                return ("10/10/2010", "5/5/2005");
            }

            protected override (string Value1, string Value2) GetValues2FailsCompare()
            {
                return ("5/5/2005", "10/10/2010");
            }
        }

        [TestClass]
        public class Int16Values : LessThanTest_PassWithNull
        {
            protected override string DataType => "Int16";

            protected override (string Value1, string Value2) GetValues2PassCompare()
            {
                return ("55", "11");
            }

            protected override (string Value1, string Value2) GetValues2FailsCompare()
            {
                return ("22", "88");
            }
        }

        [TestClass]
        public class TimeValues : LessThanTest_PassWithNull
        {
            protected override string DataType => "Time";

            protected override (string Value1, string Value2) GetValues2PassCompare()
            {
                return ("20:30", "10:00");
            }

            protected override (string Value1, string Value2) GetValues2FailsCompare()
            {
                return ("08:15", "23:00");
            }
        }
    }
}