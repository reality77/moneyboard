using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace dto.statistics
{
    public class DateStatistics
    {
        public IDictionary<DateTime, CurrencyNumber> Data { get; set; }

        public DateStatistics()
        {
            this.Data = new Dictionary<DateTime, CurrencyNumber>();
        }
    }
}