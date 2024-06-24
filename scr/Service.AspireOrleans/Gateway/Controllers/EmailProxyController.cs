using GrainInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Gateway.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class EmailProxyController : ControllerBase
    {
        public EmailProxyController(
            IClusterClient orleansClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _clientIp = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString() ??
                        httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? new Guid().ToString();

            _orleansClient = orleansClient;
        }

        /// <summary>
        /// 请求验证码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("{email}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<HttpStatusCode>> RequestCodeAsync(string email)
        {
            var grain = _orleansClient.GetGrain<IEmailService>(_clientIp);
            var res = await grain.RequestCodeAsync(email);

            if (res.IsSuccess) return Ok(res);
            else return BadRequest(res.ProblemDetails);
        }

        /// <summary>
        /// 验证安全代码
        /// </summary>
        /// <param name="email"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("{email}/{code}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<HttpStatusCode>> VertifyAsync(string email, string code)
        {
            var grain = _orleansClient.GetGrain<IEmailService>(_clientIp);
            var res = await grain.VertifyAsync(email, code);

            if (res.IsSuccess) return Ok(res);
            else return BadRequest(res.ProblemDetails);
        }

        private readonly IClusterClient _orleansClient;
        private readonly string _clientIp;
    }
}
