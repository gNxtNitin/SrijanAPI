using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MobilePortalManagementLibrary.Models;
using Services.Interfaces;
using SRIJANWEBAPI.Models;

namespace SRIJANWEBAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PunchingController : ControllerBase
    {
        private readonly IPunchingService _punchingService;
        private readonly IOptions<FileSettings> _fileSettings;
        private string _ePunchFilesPath;
        public PunchingController(IPunchingService punchingService, IOptions<FileSettings> fileSettings)
        {
            _punchingService = punchingService;
            _fileSettings = fileSettings;
        }

        [HttpGet("GetPunchingReportData")]
        public async Task<IActionResult> GetPunchingReportData(string userId, bool isTeamData = false)
        {
            try
            {
                var response = await _punchingService.GetPunchingReportData(userId, isTeamData);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }

        [HttpGet("GetAssignedSchool")]
        public async Task<IActionResult> GetAssignedSchool(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("Invalid User!");
                }
                var response = await _punchingService.GetAssignedSchool(userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }

        [HttpPost("AddEpunchRecord")]
        public async Task<IActionResult> AddEpunchRecord([FromForm] EpunchModel ePunchModel)
        {
            try
            {
               
                if (string.IsNullOrWhiteSpace(ePunchModel.EmpID))
                    return BadRequest(new { Message = "EmpID is required." });

                string savedFileName = string.Empty;
                

                if (ePunchModel.EPhoto != null && ePunchModel.EPhoto.Length > 0)
                {
                    var settings = _fileSettings.Value;
                    _ePunchFilesPath = Path.IsPathRooted(settings.EPunchFilesRootPath) ? settings.EPunchFilesRootPath : Path.Combine(Directory.GetCurrentDirectory(), settings.EPunchFilesRootPath);

                    var rootFolder = _ePunchFilesPath;
                    var dateFolder = DateTime.Now.ToString("yyyy-MM-dd");
                    var fullFolderPath = Path.Combine(rootFolder, dateFolder);

                  
                    Directory.CreateDirectory(fullFolderPath);

                
                    var fileExt = Path.GetExtension(ePunchModel.EPhoto.FileName);
                    var fileGuid = Guid.NewGuid().ToString("N");
                    savedFileName = $"{ePunchModel.EmpID}_{fileGuid}{fileExt}";

                    var fullPath = Path.Combine(fullFolderPath, savedFileName);

                  
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await ePunchModel.EPhoto.CopyToAsync(stream);
                    }

                }

                var ePunchRequestModel = new EPunchRequestModel()
                {
                    EmpID = ePunchModel.EmpID,
                    Latitude = ePunchModel.Latitude ?? 0,
                    Longitude = ePunchModel.Longitude ?? 0,
                    FileName = savedFileName ?? string.Empty,
                    KM = ePunchModel.KM ?? 0,
                    SchoolId = ePunchModel.SchoolId,
                    Location = ePunchModel.Location ?? string.Empty,
                    Address = ePunchModel.Address ?? string.Empty
                };

                var response = await _punchingService.AddEpunchRecord(ePunchRequestModel);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }

    }
}
