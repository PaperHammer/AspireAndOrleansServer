using GrainInterfaces;
using Infra.Helper.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Usr.App.Contract.Dtos;

namespace Gateway.Controllers
{
    [Route("[controller][action]")]
    [ApiController]
    public class UserProxyController : ControllerBase
    {
        public UserProxyController(
            IClusterClient orleansClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _clientIp = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString() ??
                        httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? new Guid().ToString();

            _orleansClient = orleansClient;
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input">用户信息</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<long>> CreateAsync([FromBody] UserCreationDto input)
        {
            var grain = _orleansClient.GetGrain<IUserService>(_clientIp);
            var result = await grain.CreateAsync(input);

            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result.ProblemDetails);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="input">用户信息</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        //[Authorize(Roles = "paperhammer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<HttpStatusCode>> UpdateInfoAsync([FromRoute] long id, [FromBody] UserUpdationDto input)
        {
            var grain = _orleansClient.GetGrain<IUserService>(_clientIp);
            var result = await grain.UpdateInfoAsync(id, input);

            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result.ProblemDetails);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        //[Authorize(Roles = "paperhammer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<HttpStatusCode>> DeleteAsync([FromRoute] long id)
        {
            var grain = _orleansClient.GetGrain<IUserService>(_clientIp);
            var result = await grain.DeleteAsync(id);

            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result.ProblemDetails);
        }

        /// <summary>
        /// 变更用户状态
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        //[Authorize(Roles = "paperhammer")]       
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<HttpStatusCode>> ChangeStatusAsync([FromRoute] long id, [FromBody] SimpleDto<int> status)
        {
            var grain = _orleansClient.GetGrain<IUserService>(_clientIp);
            var result = await grain.ChangeStatusAsync(id, status.Value);

            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result.ProblemDetails);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        //[Authorize(Roles = "paperhammer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserInfoDto>> GetUserInfoAsync(long id)
        {
            var grain = _orleansClient.GetGrain<IUserService>(_clientIp);
            var result = await grain.GetUserInfoAsync(id);

            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result.ProblemDetails);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<UserValidatedInfoDto>> LoginAsync([FromBody] UserLoginDto input)
        {
            var grain = _orleansClient.GetGrain<IUserService>(_clientIp);
            var result = await grain.LoginAsync(input);

            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result.ProblemDetails);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        //[Authorize(Roles = "paperhammer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<HttpStatusCode>> UpdatePasswordAsync([FromRoute] long id, [FromBody] UserChangePwdDto input)
        {
            var grain = _orleansClient.GetGrain<IUserService>(_clientIp);
            var result = await grain.UpdatePasswordAsync(id, input);

            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result.ProblemDetails);
        }

        /// <summary>
        /// 修改身份
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPut("{id}/{type}")]
        //[Authorize(Roles = "paperhammer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<HttpStatusCode>> UpdateAccountRolesAsync([FromRoute] long id, [FromRoute] RoleTypeEnum type)
        {
            var grain = _orleansClient.GetGrain<IUserService>(_clientIp);
            var result = await grain.UpdateAccountRolesAsync(id, type);

            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result.ProblemDetails);
        }
        
        private readonly IClusterClient _orleansClient;
        private readonly string _clientIp;
    }
}
