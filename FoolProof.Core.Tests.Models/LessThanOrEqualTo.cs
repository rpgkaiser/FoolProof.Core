﻿using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public class LessThanOrEqualTo
    {
        public class DateModel : ValidationModelBase<LessThanOrEqualToAttribute>
        {
            [DataType(DataType.Date)]
            public DateOnly? Value1 { get; set; }

            [DataType(DataType.Date)]
            [LessThanOrEqualTo(nameof(Value1))]
            public DateOnly? Value2 { get; set; }

            [DataType(DataType.Date)]
            [LessThanOrEqualTo(nameof(Value1), PassOnNull = true)]
            public DateOnly? ValuePwn { get; set; }
        }

        public class Int16Model : ValidationModelBase<LessThanOrEqualToAttribute>
        {
            public Int16? Value1 { get; set; }

            [LessThanOrEqualTo(nameof(Value1))]
            public Int16? Value2 { get; set; }

            [LessThanOrEqualTo(nameof(Value1), PassOnNull = true)]
            public Int16? ValuePwn { get; set; }
        }

        public class TimeModel : ValidationModelBase<LessThanOrEqualToAttribute>
        {
            [DataType(DataType.Time)]
            public TimeSpan? Value1 { get; set; }

            [DataType(DataType.Time)]
            [LessThanOrEqualTo(nameof(Value1))]
            public TimeSpan? Value2 { get; set; }

            [DataType(DataType.Time)]
            [LessThanOrEqualTo(nameof(Value1), PassOnNull = true)]
            public TimeSpan? ValuePwn { get; set; }
        }

        public class DateTimeModel : ValidationModelBase<LessThanOrEqualToAttribute>
        {
            [DataType(DataType.DateTime)]
            public DateTime? Value1 { get; set; }

            [DataType(DataType.DateTime)]
            [LessThanOrEqualTo(nameof(Value1))]
            public DateTime? Value2 { get; set; }

            [DataType(DataType.DateTime)]
            [LessThanOrEqualTo(nameof(Value1), PassOnNull = true)]
            public DateTime? ValuePwn { get; set; }
        }
    }
}
