using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobilePortalManagementLibrary.Models;
using Newtonsoft.Json;
using Services.Interfaces;
using SRIJANWEBAPI.Models;

namespace SRIJANWEBAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IApiAuditService _apiAuditService;
        public UserController(IAdminService adminService, IApiAuditService apiAuditService)
        {
            _adminService = adminService;
            _apiAuditService = apiAuditService;
        }
        [HttpGet("GetAllSchool")]
        public async Task<IActionResult> GetAllSchool(string cid1)
        {
            try
            {
                SchoolRequestModel srm = new SchoolRequestModel
                {
                    flag = "G",
                    EmpId = cid1
                };


                ResponseModel response = await _adminService.GetCreateUpdateDeleteSchool(srm);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }

       
        [HttpGet("GetData")]
        public async Task<IActionResult> GetData(string cid1, string? cid2)
        {
            try
            {

                ResponseModel response = await _adminService.GetData(cid1, cid2);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpPost("CreateUpdateDeleteSchool")]
        public async Task<IActionResult> CreateUpdateDeleteSchool([FromBody] SchoolRequestModel srm)
        {
            try
            {
                //List<Menu> menuList = new List<Menu>();
                var audit = new ResponseModel();
                if (ApiAuditSettings.EnableAudit)
                {
                    audit = await _apiAuditService.CreateUpdateApiAudit("C", "NonAdmin", HttpContext.Request.Path, 0, JsonConvert.SerializeObject(srm));
                }
                var res = await _adminService.GetCreateUpdateDeleteSchool(srm);
                if (ApiAuditSettings.EnableAudit)
                {
                    audit = await _apiAuditService.CreateUpdateApiAudit("U", "NonAdmin", HttpContext.Request.Path, audit.code, JsonConvert.SerializeObject(res));
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
    }
}
