﻿using System;
using System.Collections.Generic;
using System.Text;

namespace dto
{
    public class Category : IDtoObject
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
