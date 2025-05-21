using ErrorAndExceptionHandling.Library;
using Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ModelsLibrary.Models;
using PasswordManagementLibrary.Models;

namespace SRIJANWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordManagementController : ControllerBase
    {
        private readonly IErrorLoggingService _errorLoggingService;
        private readonly IPasswordManagementService _passwordManagementService;

        public PasswordManagementController(IErrorLoggingService errorLoggingService, IPasswordManagementService passwordManagementService)
        {
            _errorLoggingService = errorLoggingService;
            _passwordManagementService = passwordManagementService;
        }


        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] LoginReqModel lmr)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _passwordManagementService.SendForgotEmail(lmr);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }

        }


        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] LoginReqModel lmr)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _passwordManagementService.ResetPassword(lmr);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }

        }


        [HttpPost("ValidateResetPasswordToken")]
        public async Task<IActionResult> ValidateResetPasswordToken([FromBody] string tokenHash)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _passwordManagementService.ValidateResetPasswordToken(tokenHash);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }


        [HttpGet("GetPasswordValidationRules")]
        public async Task<IActionResult> GetPasswordValidationRules()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _passwordManagementService.GetPasswordValidationRules();
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetPasswordPolicy")]
        public async Task<IActionResult> GetPasswordPolicy()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _passwordManagementService.GetPasswordPolicy();
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("UpdatePasswordPolicy")]
        public async Task<IActionResult> UpdatePasswordPolicy(PasswordPolicyMaster PasswordPolicyMaster)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _passwordManagementService.UpdatePasswordPolicy(PasswordPolicyMaster);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }
    }
}
