using GrainInterfaces;
using Microsoft.AspNetCore.Mvc;
using Odr.App.Contract.Dtos;
using Shared.App.ResultModel;
using Shared.App.Services;
using Shared.WebApi.Controllers;
using System.Net;

namespace Odr.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class OrderController : SharedControllerBase
    {
        public OrderController(
            IGrainFactory grainFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            string clientIp = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString() ??
                              httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? new Guid().ToString();

            _orderService = grainFactory.GetGrain<IOrderService>(clientIp);
            _userService = grainFactory.GetGrain<IUserService>(clientIp);
            _storeService = grainFactory.GetGrain<IStoreService>(clientIp);
        }

        /// <summary>
        /// 新增订单
        /// </summary>
        /// <param name="input">订单信息</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<long>> CreateAsync([FromBody] OrderCreationDto input)
        {
            var user = await _userService.GetUserInfoAsync(input.CustomerUid);
            if (!user.IsSuccess)
            {
                return CreatedResult(new AppSrvResult<long>(AbstractAppService.Problem(HttpStatusCode.BadRequest, "用户不存在")));
            }

            var store = await _storeService.GetStoreInfoByIdAsync(input.StoreUid);
            if (!store.IsSuccess)
            {
                return CreatedResult(new AppSrvResult<long>(AbstractAppService.Problem(HttpStatusCode.BadRequest, "店铺不存在")));
            }

            return CreatedResult(await _orderService.CreateAsync(input));
        }

        /// <summary>
        /// 删除已支付或退款的订单
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns></returns>
        [HttpPut("{orderId}")]
        //[Authorize(Roles = "paperhammer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<HttpStatusCode>> DeleteAsync([FromRoute] long orderId)
            => Result(await _orderService.DeleteAsync(orderId));

        /// <summary>
        /// 骑手接单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("{orderId}/{riderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<HttpStatusCode>> RiderGetAsync(long orderId, long riderId)
        {
            var user = await _userService.GetUserInfoAsync(riderId);
            if (!user.IsSuccess)
            {
                return CreatedResult(new AppSrvResult<HttpStatusCode>(AbstractAppService.Problem(HttpStatusCode.BadRequest, "用户不存在")));
            }

            return Result(await _orderService.RiderGetAsync(orderId, riderId));
        }

        /// <summary>
        /// 完成订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("{orderId}/{cutomerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<HttpStatusCode>> FinishOrderAsync(long orderId, long cutomerId)
        {
            var user = await _userService.GetUserInfoAsync(cutomerId);
            if (!user.IsSuccess)
            {
                return CreatedResult(new AppSrvResult<HttpStatusCode>(AbstractAppService.Problem(HttpStatusCode.BadRequest, "用户不存在")));
            }
            
            return Result(await _orderService.FinishOrderAsync(orderId, cutomerId));
        }

        /// <summary>
        /// 用户申请订单退款
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("{orderId}/{cutomerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<HttpStatusCode>> RefundOrderAsync(long orderId, long cutomerId)
            => Result(await _orderService.RefundOrderAsync(orderId, cutomerId));

        /// <summary>
        /// 用户获取订单信息
        /// </summary>
        /// <param name="customerUid"></param>
        /// <returns></returns>
        [HttpPut("{cutomerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<OrderInfoDto>>> GetOrdersByCustomerUid(long customerUid)
            => Result(await _orderService.GetOrdersByCustomerUid(customerUid));

        /// <summary>
        /// 骑手获取订单信息
        /// </summary>
        /// <param name="customerUid"></param>
        /// <returns></returns>
        [HttpGet("{riderUid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<OrderInfoDto>>> GetOrdersByRiderUid(long riderUid)
            => Result(await _orderService.GetOrdersByRiderUid(riderUid));

        /// <summary>
        /// 店铺获取订单信息
        /// </summary>
        /// <param name="customerUid"></param>
        /// <returns></returns>
        [HttpGet("{storeUid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<OrderInfoDto>>> GetOrdersByStoreUid(long storeUid)
            => Result(await _orderService.GetOrdersByStoreUid(storeUid));

        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IStoreService _storeService;
    }
}
