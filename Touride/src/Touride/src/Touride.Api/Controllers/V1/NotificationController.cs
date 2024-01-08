using Microsoft.AspNetCore.Authorization;
using Touride.Abstraction.Dtos;
using Touride.Abstraction.Services;
using System.Threading.Tasks;

namespace Touride.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _emailService;
        public NotificationController(INotificationService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("emailsend")]
        public async Task<IActionResult> Insert([FromBody] EmailMessageDto emailDto)
        {
            await _emailService.Send(emailDto);

            return Accepted();
        }
    }
}
