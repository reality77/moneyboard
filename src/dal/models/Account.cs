using System;
using System.Collections.Generic;
using System.Text;

namespace dal.models
{
    public partial class Account
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal Balance { get; set; }
        public dto.ECurrency Currency { get; set; }
    }
}
