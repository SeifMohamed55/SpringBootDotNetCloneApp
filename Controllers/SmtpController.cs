using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MimeKit;
using SpringBootCloneApp.Services;
using SpringBootCloneApp.Controllers.RequestModels;

namespace SpringBootCloneApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmtpController : ControllerBase
    {
        private readonly IEmailingService _emailService;

        public SmtpController(IEmailingService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            if (!ModelState.IsValid)            
                return BadRequest("Invalid email request.");

            try
            {
                await _emailService.SendEmailAsync(request);
                return Ok("Email sent successfully!");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.InnerException is null ? ex.Message : ex.InnerException.Message);
            }

            
        }
    }


}
