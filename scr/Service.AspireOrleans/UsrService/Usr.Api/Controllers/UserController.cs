using GrainInterfaces;
using Infra.Helper.Enums;
using Microsoft.AspNetCore.Mvc;
using Shared.WebApi.Controllers;
using System.Net;
using Usr.App.Contract.Dtos;

namespace Usr.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : SharedControllerBase
    {
        public UserController(
            IGrainFactory grainFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            string clientIp = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString() ??
                              httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? new Guid().ToString();

            _usrService = grainFactory.GetGrain<IUserService>(clientIp);
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input">用户信息</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<long>> CreateAsync([FromBody] UserCreationDto input)
            => CreatedResult(await _usrService.CreateAsync(input));

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
            => Result(await _usrService.UpdateInfoAsync(id, input));

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        //[Authorize(Roles = "paperhammer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<HttpStatusCode>> DeleteAsync([FromRoute] long id)
            => Result(await _usrService.DeleteAsync(id));

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
            => Result(await _usrService.ChangeStatusAsync(id, status.Value));

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        //[Authorize(Roles = "paperhammer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserInfoDto>> GetUserInfoAsync(long id)
            => Result(await _usrService.GetUserInfoAsync(id));

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<UserValidatedInfoDto>> LoginAsync([FromBody] UserLoginDto input)
            => Result(await _usrService.LoginAsync(input));

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
            => Result(await _usrService.UpdatePasswordAsync(id, input));

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
            => Result(await _usrService.UpdateAccountRolesAsync(id, type));

        private IUserService _usrService;
    }
}
