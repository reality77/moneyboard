using System;
using System.Collections.Generic;
using System.Text;

namespace dto
{
    public class Account : IDtoObject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ECurrency Currency { get; set; }
        public CurrencyNumber InitialBalance { get; set; }
        public CurrencyNumber Balance { get; set; }
    }
}
