using GrainInterfaces;
using Microsoft.AspNetCore.Mvc;
using Sto.App.Contract.Dtos;
using System.Net;

namespace Gateway.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StoreProxyController : ControllerBase
    {
        public StoreProxyController(
           IClusterClient orleansClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _clientIp = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString() ??
                        httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? new Guid().ToString();

            _orleansClient = orleansClient;
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
            var g1 = _orleansClient.GetGrain<IUserService>(_clientIp);
            var r1 = await g1.GetUserInfoAsync(input.OwnerId);
            if (!r1.IsSuccess) return BadRequest(r1);

            var g2 = _orleansClient.GetGrain<IStoreService>(_clientIp);
            var r2 = await g2.CreateAsync(input);

            if (r2.IsSuccess) return Ok(r2);
            else return BadRequest(r2.ProblemDetails);
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
        {
            var grain = _orleansClient.GetGrain<IStoreService>(_clientIp);
            var res = await grain.UpdateInfoAsync(id, input);

            if (res.IsSuccess) return Ok(res);
            else return BadRequest(res.ProblemDetails);
        }

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
        {
            var grain = _orleansClient.GetGrain<IStoreService>(_clientIp);
            var res = await grain.UpStoreAsync(ownerId, storeId);

            if (res.IsSuccess) return Ok(res);
            else return BadRequest(res.ProblemDetails);
        }

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
        {
            var grain = _orleansClient.GetGrain<IStoreService>(_clientIp);
            var res = await grain.DownStoreAsync(ownerId, storeId);

            if (res.IsSuccess) return Ok(res);
            else return BadRequest(res.ProblemDetails);
        }

        /// <summary>
        /// 获取店铺信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StoreInfoDto>> GetStoreInfoByIdAsync([FromRoute] long id)
        {
            var grain = _orleansClient.GetGrain<IStoreService>(_clientIp);
            var res = await grain.GetStoreInfoByIdAsync(id);

            if (res.IsSuccess) return Ok(res);
            else return BadRequest(res.ProblemDetails);
        }

        /// <summary>
        /// 按增量获取店铺
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpGet("{tag}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<StoreInfoDto>>> GetNewStoresAsync([FromRoute] int tag)
        {
            var grain = _orleansClient.GetGrain<IStoreService>(_clientIp);
            var res = await grain.GetNewStoresAsync(tag);

            if (res.IsSuccess) return Ok(res);
            else return BadRequest(res.ProblemDetails);
        }

        private readonly IClusterClient _orleansClient;
        private readonly string _clientIp;
    }
}
