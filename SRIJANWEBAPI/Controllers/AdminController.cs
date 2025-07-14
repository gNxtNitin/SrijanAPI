using Microsoft.AspNetCore.Authorization;
//using CustomerManagementLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MobilePortalManagementLibrary.Models;
using Newtonsoft.Json;
using Services.Implementation;
using Services.Interfaces;
using SRIJANWEBAPI.Models;
using SRIJANWEBAPI.Utility;
using Twilio.TwiML.Voice;

namespace SRIJANWEBAPI.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly EncryptDecryptClass encDcService = new EncryptDecryptClass();
        private readonly IApiAuditService _apiAuditService;
        public AdminController(IAdminService adminService, IApiAuditService apiAuditService)
        {
            _adminService = adminService;
            _apiAuditService = apiAuditService;
        }
        [HttpGet("GetDashboardCharts")]
        public async Task<IActionResult> GetDashboardCharts(string cid1)
        {
            try
            {
                ResponseModel response = await _adminService.GetDashboardCharts(cid1);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpGet("GetAllZones")]
        public async Task<IActionResult> GetAllZones()
        {
            try
            {
                ZoneRequestModel zrm = new ZoneRequestModel
                {
                    flag = "G"
                };


                ResponseModel response = await _adminService.GetCreateUpdateDeleteZone(zrm);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpPost("CreateUpdateDeleteZone")]
        public async Task<IActionResult> CreateUpdateDeleteZone([FromBody] ZoneRequestModel zrm)
        {
            try
            {
                var audit = new ResponseModel();
                if (ApiAuditSettings.EnableAudit)
                {
                    audit = await _apiAuditService.CreateUpdateApiAudit("C", "ADMIN", HttpContext.Request.Path, 0, JsonConvert.SerializeObject(zrm));
                }
                //List<Menu> menuList = new List<Menu>();
                var res = await _adminService.GetCreateUpdateDeleteZone(zrm);
                if (ApiAuditSettings.EnableAudit)
                {
                    audit = await _apiAuditService.CreateUpdateApiAudit("U", "ADMIN", HttpContext.Request.Path, audit.code, JsonConvert.SerializeObject(res));
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpGet("GetAllCity")]
        public async Task<IActionResult> GetAllCity()
        {
            try
            {
                CityRequestModel crm = new CityRequestModel
                {
                    flag = "G"
                };


                ResponseModel response = await _adminService.GetCreateUpdateDeleteCity(crm);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpPost("CreateUpdateDeleteCity")]
        public async Task<IActionResult> CreateUpdateDeleteCity([FromBody] CityRequestModel crm)
        {
            try
            {
                var audit = new ResponseModel();
                if (ApiAuditSettings.EnableAudit)
                {
                    audit = await _apiAuditService.CreateUpdateApiAudit("C", "ADMIN", HttpContext.Request.Path, 0, JsonConvert.SerializeObject(crm));
                }
                //List<Menu> menuList = new List<Menu>();
                var res = await _adminService.GetCreateUpdateDeleteCity(crm);
                if (ApiAuditSettings.EnableAudit)
                {
                    audit = await _apiAuditService.CreateUpdateApiAudit("U", "ADMIN", HttpContext.Request.Path, audit.code, JsonConvert.SerializeObject(res));
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
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
        [HttpPost("CreateUpdateDeleteSchool")]
        public async Task<IActionResult> CreateUpdateDeleteSchool([FromBody] SchoolRequestModel srm)
        {
            try
            {
                var audit = new ResponseModel();
                if (ApiAuditSettings.EnableAudit)
                {
                    audit = await _apiAuditService.CreateUpdateApiAudit("C", "ADMIN", HttpContext.Request.Path, 0, JsonConvert.SerializeObject(srm));
                }
                //List<Menu> menuList = new List<Menu>();
                var res = await _adminService.GetCreateUpdateDeleteSchool(srm);
                if (ApiAuditSettings.EnableAudit)
                {
                    audit = await _apiAuditService.CreateUpdateApiAudit("U", "ADMIN", HttpContext.Request.Path, audit.code, JsonConvert.SerializeObject(res));
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpGet("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                EmployeeRequestModel erm = new EmployeeRequestModel
                {
                    flag = "G"
                };


                ResponseModel response = await _adminService.GetCreateUpdateDeleteEmployees(erm);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpPost("CreateUpdateDeleteEmployee")]
        public async Task<IActionResult> CreateUpdateDeleteEmployee([FromBody] EmployeeRequestModel erm)
        {
            try
            {
                var audit = new ResponseModel();
                if (ApiAuditSettings.EnableAudit)
                {
                    audit = await _apiAuditService.CreateUpdateApiAudit("C", "ADMIN", HttpContext.Request.Path, 0, JsonConvert.SerializeObject(erm));
                }
                if (!string.IsNullOrEmpty(erm.Password))
                {
                    erm.Password = await encDcService.Encrypt(erm.Password);
                }
                
                //List<Menu> menuList = new List<Menu>();
                var res = await _adminService.GetCreateUpdateDeleteEmployees(erm);
                if (ApiAuditSettings.EnableAudit)
                {
                    audit = await _apiAuditService.CreateUpdateApiAudit("U", "ADMIN", HttpContext.Request.Path, audit.code, JsonConvert.SerializeObject(res));
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpGet("GetSchoolIncharge")]
        public async Task<IActionResult> GetSchoolIncharge()
        {
            try
            {
                SchoolRequestModel srm = new SchoolRequestModel
                {
                    flag = "G"
                };


                ResponseModel response = await _adminService.AssignSchoolIncharge(srm);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpPost("AssignSchoolIncharge")]
        public async Task<IActionResult> AssignSchoolIncharge([FromBody] SchoolRequestModel srm)
        {
            try
            {
                var audit = new ResponseModel();
                if (ApiAuditSettings.EnableAudit)
                {
                    audit = await _apiAuditService.CreateUpdateApiAudit("C", "ADMIN", HttpContext.Request.Path, 0, JsonConvert.SerializeObject(srm));
                }
                var res = await _adminService.AssignSchoolIncharge(srm);
                if (ApiAuditSettings.EnableAudit)
                {
                    audit = await _apiAuditService.CreateUpdateApiAudit("U", "ADMIN", HttpContext.Request.Path, audit.code, JsonConvert.SerializeObject(res));
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
