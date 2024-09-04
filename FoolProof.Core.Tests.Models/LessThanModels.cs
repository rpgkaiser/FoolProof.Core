using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public class LessThan
    {
        public class DateModel : ValidationModelBase<LessThanAttribute>
        {
            [DataType(DataType.Date)]
            public DateTime? Value1 { get; set; }

            [DataType(DataType.Date)]
            [LessThan("Value1")]
            public DateTime? Value2 { get; set; }
        }

        public class DateModelWithPassOnNull : ValidationModelBase<LessThanAttribute>
        {
            [DataType(DataType.Date)]
            public DateTime? Value1 { get; set; }

            [DataType(DataType.Date)]
            [LessThan("Value1", PassOnNull = true)]
            public DateTime? Value2 { get; set; }
        }

        public class Int16Model : ValidationModelBase<LessThanAttribute>
        {
            public Int16? Value1 { get; set; }

            [LessThan("Value1")]
            public Int16? Value2 { get; set; }
        }

        public class Int16ModelWithPassOnNull : ValidationModelBase<LessThanAttribute>
        {
            public Int16? Value1 { get; set; }

            [LessThan("Value1", PassOnNull = true)]
            public Int16? Value2 { get; set; }
        }

        public class TimeModel : ValidationModelBase<LessThanAttribute>
        {
            [DataType(DataType.Time)]
            public TimeSpan? Value1 { get; set; }

            [DataType(DataType.Time)]
            [LessThan("Value1")]
            public TimeSpan? Value2 { get; set; }
        }

        public class TimeModelWithPassOnNull : ValidationModelBase<LessThanAttribute>
        {
            [DataType(DataType.Time)]
            public TimeSpan? Value1 { get; set; }

            [DataType(DataType.Time)]
            [LessThan("Value1", PassOnNull = true)]
            public TimeSpan? Value2 { get; set; }
        }
    }
}
