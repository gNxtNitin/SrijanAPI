

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using MobilePortalManagementLibrary.Models;
using Newtonsoft.Json;
using Services.Interfaces;
using SRIJANWEBAPI.Models;

namespace SRIJANWEBAPI.Controllers
{
    //[Authorize(Roles = "ADMIN,INCHARGE,HOD,USER")]
    [Route("api/[controller]")]
    [ApiController]
    public class DAController : ControllerBase
    {
        private readonly IDAService _dAService;
        private readonly IOptions<FileSettings> _fileSettings;
        private string _daFilesPath;
        private readonly IApiAuditService _apiAuditService;
        public DAController(IDAService dAService, IOptions<FileSettings> fileSettings, IApiAuditService apiAuditService)
        {
            _dAService = dAService;
            _fileSettings = fileSettings;
            _apiAuditService = apiAuditService;
        }

        [HttpGet("GetDAReportData")]
        public async Task<IActionResult> GetDAReportData([FromQuery] ReportRequest reportRequest)
        {
            try
            {
                var response = await _dAService.GetDAReportData(reportRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }

        [HttpGet("GetKMValueByDateRange")]
        public async Task<IActionResult> GetKMValueByDateRange(string userId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                if (fromDate > toDate)
                {
                    return BadRequest("From date cannot exceed to date");
                }

                var response = await _dAService.GetKMVaueByDateRange(userId, fromDate, toDate);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }


        [HttpPost("AddDARecord")]
        public async Task<IActionResult> AddDARecord([FromForm] DARequest model)
        {
            try
            {
                var audit = new ResponseModel();
                if (ApiAuditSettings.EnableAudit)
                {
                    audit = await _apiAuditService.CreateUpdateApiAudit("C", model.EmpId, HttpContext.Request.Path, 0, JsonConvert.SerializeObject(model));
                }
                if (string.IsNullOrWhiteSpace(model.EmpId))
                    return BadRequest(new { Message = "EmpID is required." });
                var fileNames = await ProcessDAFilesAsync(model);

                var daRequest = new DARequestModel()
                {
                    EmpId = model.EmpId,
                    DA = model.DA,
                    Hotel = model.Hotel ?? 0,
                    Other = model.Other ?? 0,
                    FromDate = model.FromDate,
                    ToDate = model.ToDate,
                    KM = model.KM,
                    FilenNames = string.Join(",", fileNames),
                    Descriptions = string.Join(",", model.Descriptions)
                };

                var response = await _dAService.AddDARecord(daRequest);
                if (ApiAuditSettings.EnableAudit)
                {
                    audit = await _apiAuditService.CreateUpdateApiAudit("U", model.EmpId, HttpContext.Request.Path, audit.code, JsonConvert.SerializeObject(response));
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }


        private async Task<List<string>> ProcessDAFilesAsync(DARequest model)
        {
            List<string> fileNames = new List<string>();

            var settings = _fileSettings.Value;
            _daFilesPath = Path.IsPathRooted(settings.DAFilesRootPath) ? settings.DAFilesRootPath : Path.Combine(Directory.GetCurrentDirectory(), settings.DAFilesRootPath);


            if (model.Bills != null && model.Bills.Any())
            {
                //if any file exceeds size limit
                if(model.Bills.Any(file => file.Length > (settings.MaxUploadSize * 1024 * 1024)))
                {
                    throw new Exception("File size exceeds the limit.");
                }

                if (model.Bills.Count > settings.MaxBillsUpload)
                {
                    throw new Exception($"Cannot upload more than {settings.MaxBillsUpload} bills");
                }


                var rootFolder = Path.Combine(_daFilesPath, model.EmpId);
                Directory.CreateDirectory(rootFolder);

                foreach (var file in model.Bills)
                {
                    if (file.Length > 0)
                    {
                        var ext = Path.GetExtension(file.FileName);
                        var uniqueName = $"{model.EmpId}_{Guid.NewGuid()}{ext}";
                        var fullPath = Path.Combine(rootFolder, uniqueName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        fileNames.Add(uniqueName);
                    }
                }
            }

            if (model.Descriptions == null)
            {
                model.Descriptions = new List<string>(new string[fileNames.Count]);
            }
            else if (model.Descriptions.Count < fileNames.Count)
            {
                int diff = fileNames.Count - model.Descriptions.Count;
                model.Descriptions.AddRange(Enumerable.Repeat(string.Empty, diff));
            }

            return fileNames;
        }


        [Authorize(Roles = "ADMIN,INCHARGE,HOD")]
        [HttpPost("ApproveRejectDA")]
        public async Task<IActionResult> ApproveRejectDA([FromBody] DAApproveRejectReq dAApproveRejectReq)
        {
            try
            {
                var audit = new ResponseModel();
                if (ApiAuditSettings.EnableAudit)
                {
                    audit = await _apiAuditService.CreateUpdateApiAudit("C", dAApproveRejectReq.ARBy, HttpContext.Request.Path, 0, JsonConvert.SerializeObject(dAApproveRejectReq));
                }
                if (string.IsNullOrEmpty(dAApproveRejectReq.DAID) || string.IsNullOrEmpty(dAApproveRejectReq.ARBy) || string.IsNullOrEmpty(dAApproveRejectReq.DAEmpId))
                {
                    return BadRequest("Invalid DA Approve/Reject Request! Data is Invalid");
                }

                var response = await _dAService.ApproveRejectDA(dAApproveRejectReq);
                if (ApiAuditSettings.EnableAudit)
                {
                    audit = await _apiAuditService.CreateUpdateApiAudit("U", dAApproveRejectReq.ARBy, HttpContext.Request.Path, audit.code, JsonConvert.SerializeObject(response));
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }


        [Authorize(Roles = "ADMIN,INCHARGE,HOD")]
        [HttpPost("MultiApproveRejectDA")]
        public async Task<IActionResult> MultiApproveRejectDA([FromBody] List<DAApproveRejectReq> dAApproveRejectReq)
        {
            try
            {
                var audit = new ResponseModel();
                if (ApiAuditSettings.EnableAudit)
                {
                    audit = await _apiAuditService.CreateUpdateApiAudit("C", dAApproveRejectReq.Count != 0 ?  dAApproveRejectReq[0].ARBy : "EmptyList", HttpContext.Request.Path, 0, JsonConvert.SerializeObject(dAApproveRejectReq));
                }
                if (dAApproveRejectReq == null || !dAApproveRejectReq.Any())
                {
                    return BadRequest("Request list cannot be null or empty.");
                }

                var results = new List<int>();
                bool allSuccessful = true;

                foreach (var item in dAApproveRejectReq)
                {
                    if (string.IsNullOrWhiteSpace(item.DAID) ||
                        string.IsNullOrWhiteSpace(item.ARBy) ||
                        string.IsNullOrWhiteSpace(item.DAEmpId))
                    {
                        allSuccessful = false;
                        results.Add(-1);
                        continue;
                    }

                    try
                    {
                        var response = await _dAService.ApproveRejectDA(item);
                        results.Add(response.code);
                    }
                    catch (Exception ex)
                    {
                        allSuccessful = false;
                        results.Add(-1);
                    }
                }

                ResponseModel resultSummary = new ResponseModel
                {
                    code = allSuccessful ? 1 : -1,
                    msg = allSuccessful ? "All requests processed successfully." : "Some requests failed.",
                    data = new MultiApproveStatus()
                    {
                        SuccessCount = results.Count(r => r > 0),
                        FailureCount = results.Count(r => r <= 0)
                    }
                };
                if (ApiAuditSettings.EnableAudit)
                {
                    audit = await _apiAuditService.CreateUpdateApiAudit("U", dAApproveRejectReq.Count != 0 ? dAApproveRejectReq[0].ARBy : "EmptyList", HttpContext.Request.Path, audit.code, JsonConvert.SerializeObject(resultSummary));
                }
                return Ok(resultSummary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing the request.", Details = ex.Message });
            }
        }


        [HttpGet("DABill/{fileName}")]
        public async Task<IActionResult> GetBill(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return BadRequest("File name is required.");

            var settings = _fileSettings.Value;

          
            var sanitizedFileName = Path.GetFileName(fileName);
            string[] d = fileName.Split("_");
            string eid = d.Length > 0 ? d[0] : "0";

            var filePath = Path.Combine(settings.DAFilesRootPath,eid, sanitizedFileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found.");

         
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            UserFile userFile = new UserFile
            {
                FileName = sanitizedFileName,
                FileBytes = fileBytes,
                ContentType = contentType
            };

            return Ok(userFile);
        }

    }
}
