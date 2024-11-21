using Microsoft.AspNetCore.Mvc;
using SharingNote.Api.Application.Services;
using SharingNote.Api.REST.Controllers.Requests.Otp;

namespace SharingNote.Api.REST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OtpController : ControllerBase
    {
        private readonly IOtpService _otpService;
        private readonly IEmailSendingService _emailSendingService;

        public OtpController(IOtpService otpService, IEmailSendingService emailSendingService)
        {
            _otpService = otpService;
            _emailSendingService = emailSendingService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest request)
        {
            var otp = await _otpService.GenerateOtpAsync(request.Email);

            await _emailSendingService.SendEmailAsync(
                request.Email,
                request.Email,
                request.EmailSubject,
                otp);

            return Ok();
        }
    }
}
