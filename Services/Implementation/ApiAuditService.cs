using Microsoft.AspNetCore.Http;
using MobilePortalManagementLibrary.Interface;
using MobilePortalManagementLibrary.Models;
using Newtonsoft.Json;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class ApiAuditService: IApiAuditService
    {
        private readonly IApiAuditManagement _apiAuditManagement;
        public ApiAuditService(IApiAuditManagement apiAuditManagement)
        {
            _apiAuditManagement = apiAuditManagement;
        }
        public async Task<ResponseModel> CreateUpdateApiAudit(string flag, string EmpId,string ApiName,int Id , string Res)
        {
            var response = new ResponseModel();
            if (flag == "C")
            {
                var model = new ApiAuditRequest
                {
                    flag = "C",
                    EmpId = EmpId,
                    ApiName = ApiName,
                    Request = JsonConvert.SerializeObject(Res)
                };
                response = await _apiAuditManagement.CreateUpdateApiAudit(model);
            }
            else
            {
                var model = new ApiAuditRequest
                {
                    flag = "U",
                    EmpId = EmpId,
                    ApiName = ApiName,
                    Id = Id,
                    Response = JsonConvert.SerializeObject(Res)
                };
                response = await _apiAuditManagement.CreateUpdateApiAudit(model);
            }
                
            return response;
        }
    }
}
