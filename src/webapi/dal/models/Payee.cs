using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dal.models
{
    public partial class Payee
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
