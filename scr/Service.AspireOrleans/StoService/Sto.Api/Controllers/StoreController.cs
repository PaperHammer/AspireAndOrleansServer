using GrainInterfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.App.ResultModel;
using Shared.App.Services;
using Shared.WebApi.Controllers;
using Sto.App.Contract.Dtos;
using System.Net;

namespace Sto.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StoreController : SharedControllerBase
    {
        public StoreController(
           IGrainFactory grainFactory,
           IHttpContextAccessor httpContextAccessor)
        {
            string clientIp = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString() ??
                              httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? new Guid().ToString();

            _storeService = grainFactory.GetGrain<IStoreService>(clientIp);
            _userService = grainFactory.GetGrain<IUserService>(clientIp);
        }

        /// <summary>
        /// 新增店铺
        /// </summary>
        /// <param name="input">店铺信息</param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Roles = "paperhammer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<long>> CreateAsync([FromBody] StoreCreationDto input)
        {
            var user = await _userService.GetUserInfoAsync(input.OwnerId);
            if (!user.IsSuccess)
            {
                return CreatedResult(new AppSrvResult<long>(AbstractAppService.Problem(HttpStatusCode.BadRequest, "用户不存在")));
            }

            return CreatedResult(await _storeService.CreateAsync(input));
        }

        /// <summary>
        /// 修改店铺
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="input">店铺信息</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        //[Authorize(Roles = "paperhammer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<long>> UpdateInfoAsync([FromRoute] long id, [FromBody] StoreUpdationDto input)
            => Result(await _storeService.UpdateInfoAsync(id, input));

        /// <summary>
        /// 下架
        /// </summary>
        /// <param name="ownerId">店铺ID</param>
        /// <param name="storeId">店铺ID</param>
        /// <returns></returns>
        [HttpPut("{ownerId}/{storeId}")]
        //[Authorize(Roles = "paperhammer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<HttpStatusCode>> UpStoreAsync([FromRoute] long ownerId, [FromRoute] long storeId)
            => Result(await _storeService.UpStoreAsync(ownerId, storeId));

        /// <summary>
        /// 上架
        /// </summary>
        /// <param name="ownerId">店铺ID</param>
        /// <param name="storeId">店铺ID</param>
        /// <returns></returns>
        [HttpPut("{ownerId}/{storeId}")]
        //[Authorize(Roles = "paperhammer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<HttpStatusCode>> DownStoreAsync([FromRoute] long ownerId, [FromRoute] long storeId)
            => Result(await _storeService.DownStoreAsync(ownerId, storeId));

        /// <summary>
        /// 获取店铺信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StoreInfoDto>> GetStoreInfoByIdAsync([FromRoute] long id)
            => Result(await _storeService.GetStoreInfoByIdAsync(id));

        /// <summary>
        /// 按增量获取店铺
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpGet("{tag}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<StoreInfoDto>>> GetNewStoresAsync([FromRoute] int tag)
            => Result(await _storeService.GetNewStoresAsync(tag));

        private readonly IStoreService _storeService;
        private readonly IUserService _userService;
    }
}
