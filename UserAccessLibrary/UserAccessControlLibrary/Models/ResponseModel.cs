﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAccessControlLibrary.Models
{
    public class ResponseModel
    {
        public int code { get; set; }
        public string? msg { get; set; }
        public string? data { get; set; }
    }
}
