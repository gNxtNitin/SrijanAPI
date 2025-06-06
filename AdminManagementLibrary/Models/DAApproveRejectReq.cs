﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePortalManagementLibrary.Models
{
    public class DAApproveRejectReq
    {
        public string DAID { get; set; }

        public string DAEmpId { get; set; }
        public string ARBy { get; set; }
        public bool IsApproved { get; set; }
    }

    public class MultiApproveStatus
    {
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
    }
}
