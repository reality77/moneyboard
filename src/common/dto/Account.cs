using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace dto
{
    public class Account : IDtoObject
    {
        public int ID { get; set; }
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ECurrency Currency { get; set; }
        public CurrencyNumber InitialBalance { get; set; }
        public CurrencyNumber Balance { get; set; }
    }
}
