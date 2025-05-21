using MobilePortalManagementLibrary.Models;

namespace Services.Interfaces
{
    public interface IPunchingService
    {
        Task<ResponseModel> GetPunchingReportData(string userId, bool isTeamData = false);
        Task<ResponseModel> AddEpunchRecord(EPunchRequestModel ePunchRequestModel);

        Task<ResponseModel> GetAssignedSchool(string empId);
    }
}
