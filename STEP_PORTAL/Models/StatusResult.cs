﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STEP_PORTAL.Models
{
    public class StatusResult
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string ReportSuperComment { get; set; }
        public bool ApprovalSent { get; set; }
    }
}