﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePortalManagementLibrary.Models
{
    public class ResponseModel
    {
        public int code { get; set; }
        public string? msg { get; set; }
        public object? data { get; set; }
    }
}
