using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public class LessThanOrEqualTo
    {
        public class DateModel : ValidationModelBase<LessThanOrEqualToAttribute>
        {
            public DateTime? Value1 { get; set; }

            [LessThanOrEqualTo("Value1")]
            public DateTime? Value2 { get; set; }
        }

        public class DateModelWithPassNull : ValidationModelBase<LessThanOrEqualToAttribute>
        {
            public DateTime? Value1 { get; set; }

            [LessThanOrEqualTo("Value1", PassOnNull = true)]
            public DateTime? Value2 { get; set; }
        }

        public class Int16Model : ValidationModelBase<LessThanOrEqualToAttribute>
        {
            public Int16? Value1 { get; set; }

            [LessThanOrEqualTo("Value1")]
            public Int16? Value2 { get; set; }
        }

        public class Int16ModelWithPassNull : ValidationModelBase<LessThanOrEqualToAttribute>
        {
            public Int16? Value1 { get; set; }

            [LessThanOrEqualTo("Value1", PassOnNull = true)]
            public Int16? Value2 { get; set; }
        }

        public class TimeModel : ValidationModelBase<LessThanOrEqualToAttribute>
        {
            public TimeSpan? Value1 { get; set; }

            [LessThanOrEqualTo("Value1")]
            public TimeSpan? Value2 { get; set; }
        }

        public class TimeModelWithPassNull : ValidationModelBase<LessThanOrEqualToAttribute>
        {
            public TimeSpan? Value1 { get; set; }

            [LessThanOrEqualTo("Value1", PassOnNull = true)]
            public TimeSpan? Value2 { get; set; }
        }
    }
}
