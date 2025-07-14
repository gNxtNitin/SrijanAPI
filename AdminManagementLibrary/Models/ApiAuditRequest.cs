using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePortalManagementLibrary.Models
{
    public class ApiAuditRequest
    {
        public string flag { get; set; } = "G"; // 'I' for insert, 'U' for update
        public string EmpId { get; set; }
        public int? Id { get; set; } // Used as OUT when insert, IN when update
        public string? ApiName { get; set; }
        public string? Request { get; set; } // CLOB
        public string? Response { get; set; } // CLOB
    }
}
