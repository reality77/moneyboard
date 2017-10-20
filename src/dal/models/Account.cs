using System;
using System.Collections.Generic;
using System.Text;

namespace dal.models
{
    public class Account
    {
        public int ID { get; set; }
        public string AccountName { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal Balance { get; set; }
        public ECurrency Currency { get; set; }
    }

    public enum ECurrency
    {
        EUR = 0,
    }
}
