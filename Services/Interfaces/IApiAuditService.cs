using MobilePortalManagementLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IApiAuditService
    {
        Task<ResponseModel> CreateUpdateApiAudit(string flag, string EmpId, string ApiName, int Id, string Res);
    }
}
