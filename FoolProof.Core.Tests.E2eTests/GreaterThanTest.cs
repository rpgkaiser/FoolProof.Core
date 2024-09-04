using System.Text.RegularExpressions;

namespace FoolProof.Core.Tests.E2eTests
{
    public abstract class GreaterThanTest: CompareBaseTest
    {
        protected override Regex PageTitleRegex() => new($@".+\s+[-]\s+GreaterThan\s+\({DataType}\)");

        protected override Uri PageUri() => new(new Uri(WebAppUrl), $"greaterthan/{DataType}");

        protected override string Value2ValidationError => "Value2 must be greater than Value1";

        [TestMethod("Value2GreaterThanValue1_Valid")]
        public override Task CompareValuesPass()
        {
            return base.CompareValuesPass();
        }

        [TestMethod("Value1GreaterThanValue2_Invalid")]
        public override Task CompareValuesFails()
        {
            return base.CompareValuesFails();
        }


        [TestClass]
        public class DateValues : GreaterThanTest
        {
            protected override string DataType => "Date";

            protected override (string Value1, string Value2) GetValues2PassCompare()
            {
                return ("5/5/2005", "10/10/2010");
            }

            protected override (string Value1, string Value2) GetValues2FailsCompare()
            {
                return ("10/10/2010", "5/5/2005");
            }
        }

        [TestClass]
        public class Int16Values : GreaterThanTest
        {
            protected override string DataType => "Int16";

            protected override (string Value1, string Value2) GetValues2PassCompare()
            {
                return ("11", "55");
            }

            protected override (string Value1, string Value2) GetValues2FailsCompare()
            {
                return ("88", "22");
            }
        }

        [TestClass]
        public class TimeValues : GreaterThanTest
        {
            protected override string DataType => "Time";

            protected override (string Value1, string Value2) GetValues2PassCompare()
            {
                return ("08:00", "22:00");
            }

            protected override (string Value1, string Value2) GetValues2FailsCompare()
            {
                return ("20:30", "10:00");
            }
        }
    }

    public abstract class GreaterThanTest_PassWithNull : CompareBaseTest_PassWithNull
    {
        protected override Regex PageTitleRegex() => new($@".+\s+[-]\s+GreaterThan\s+\({DataType}\)");

        protected override Uri PageUri() => new(new Uri(WebAppUrl), $"greaterthan/{DataType}?pwn=true");

        protected override string Value2ValidationError => "Value2 must be greater than Value1";

        [TestClass]
        public class DateValues : GreaterThanTest_PassWithNull
        {
            protected override string DataType => "Date";

            protected override (string Value1, string Value2) GetValues2PassCompare()
            {
                return ("5/5/2005", "10/10/2010");
            }

            protected override (string Value1, string Value2) GetValues2FailsCompare()
            {
                return ("10/10/2010", "5/5/2005");
            }
        }

        [TestClass]
        public class Int16Values : GreaterThanTest_PassWithNull
        {
            protected override string DataType => "Int16";

            protected override (string Value1, string Value2) GetValues2PassCompare()
            {
                return ("11", "55");
            }

            protected override (string Value1, string Value2) GetValues2FailsCompare()
            {
                return ("88", "22");
            }
        }

        [TestClass]
        public class TimeValues : GreaterThanTest_PassWithNull
        {
            protected override string DataType => "Time";

            protected override (string Value1, string Value2) GetValues2PassCompare()
            {
                return ("08:00", "22:00");
            }

            protected override (string Value1, string Value2) GetValues2FailsCompare()
            {
                return ("20:30", "10:00");
            }
        }
    }
}