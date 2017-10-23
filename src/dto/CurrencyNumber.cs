using System;
using System.Collections.Generic;
using System.Text;

namespace dto
{
    public struct CurrencyNumber
    {
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