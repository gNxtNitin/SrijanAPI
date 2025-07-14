using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace SRIJANWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitsController : ControllerBase
    {
        private readonly IVisitService _visitService;
        private readonly IApiAuditService _apiAuditService;
        public VisitsController(IVisitService visitService, IApiAuditService apiAuditService)
        {
            _visitService = visitService;
            _apiAuditService = apiAuditService;
        }

        [HttpGet("GetVisitReportData")]
        public async Task<IActionResult> GetVisitReportData(string userId, bool isTeamData = false)
        {
            try
            {
                var response = await _visitService.GetVisitReportData(userId, isTeamData);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }
    }
}
