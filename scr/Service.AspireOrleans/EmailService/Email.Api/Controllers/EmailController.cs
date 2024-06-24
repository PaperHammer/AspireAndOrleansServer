using Email.App.Contract.Utils;
using GrainInterfaces;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using Shared.WebApi.Controllers;
using System.Net;

namespace Email.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class EmailController : SharedControllerBase
    {
        public EmailController(
            IGrainFactory grainFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            string clientIp = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString() ??
                              httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? new Guid().ToString();

            _emailService = grainFactory.GetGrain<IEmailService>(clientIp);
        }

        /// <summary>
        /// 请求验证码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("{email}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<HttpStatusCode>> RequestCodeAsync(string email)
            => Result(await _emailService.RequestCodeAsync(email));

        /// <summary>
        /// 验证安全代码
        /// </summary>
        /// <param name="email"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("{email}/{code}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<HttpStatusCode>> VertifyAsync(string email, string code)
            => Result(await _emailService.VertifyAsync(email, code));

        private IEmailService _emailService;
    }
}
