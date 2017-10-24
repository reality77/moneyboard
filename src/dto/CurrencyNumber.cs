using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace dto
{
    public struct CurrencyNumber
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ECurrency Currency { get; set; }
        public decimal Value { get; set; }

        public static implicit operator decimal(CurrencyNumber nb)
        {
            return nb.Value;
        }
    }

    public enum ECurrency
    {
        Unknown = 0,

        EUR = 1,
    }
}