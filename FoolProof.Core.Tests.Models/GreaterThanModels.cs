using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public class GreaterThan
    {
        public class DateModel : ValidationModelBase<GreaterThanAttribute>
        {
            [DataType(DataType.Date)]
            public DateTime? Value1 { get; set; }

            [DataType(DataType.Date)]
            [GreaterThan("Value1")]
            public DateTime? Value2 { get; set; }
        }

        public class DateModelWithPassOnNull : ValidationModelBase<GreaterThanAttribute>
        {
            [DataType(DataType.Date)]
            public DateTime? Value1 { get; set; }

            [DataType(DataType.Date)]
            [GreaterThan("Value1", PassOnNull = true)]
            public DateTime? Value2 { get; set; }
        }

        public class Int16Model : ValidationModelBase<GreaterThanAttribute>
        {
            public Int16? Value1 { get; set; }

            [GreaterThan("Value1")]
            public Int16? Value2 { get; set; }
        }

        public class Int16ModelWithPassOnNull : ValidationModelBase<GreaterThanAttribute>
        {
            public Int16? Value1 { get; set; }

            [GreaterThan("Value1", PassOnNull = true)]
            public Int16? Value2 { get; set; }
        }

        public class TimeModel : ValidationModelBase<GreaterThanAttribute>
        {
            [DataType(DataType.Time)]
            public TimeSpan? Value1 { get; set; }

            [DataType(DataType.Time)]
            [GreaterThan("Value1")]
            public TimeSpan? Value2 { get; set; }
        }

        public class TimeModelWithPassOnNull : ValidationModelBase<GreaterThanAttribute>
        {
            [DataType(DataType.Time)]
            public TimeSpan? Value1 { get; set; }

            [DataType(DataType.Time)]
            [GreaterThan("Value1", PassOnNull = true)]
            public TimeSpan? Value2 { get; set; }
        }
    }
}
