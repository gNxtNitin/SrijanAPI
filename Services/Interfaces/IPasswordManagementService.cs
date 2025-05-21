

using ModelsLibrary.Models;
using PasswordManagementLibrary.Models;

namespace Services.Interfaces
{
    public interface IPasswordManagementService
    {
        public Task<ResponseModel> SendForgotEmail(LoginReqModel lrm);
        public Task<ResponseModel> ResetPassword(LoginReqModel lrm);

        public Task<ResponseModel> ValidateResetPasswordToken(string token);

        public Task<ResponseModel> GetPasswordValidationRules();

        public Task<ResponseModel> UpdatePasswordPolicy(PasswordPolicyMaster PasswordPolicyMaster);

        public Task<ResponseModel> GetPasswordPolicy();
    }
}
