using GrainInterfaces;
using Microsoft.AspNetCore.Mvc;
using Odr.App.Contract.Dtos;
using System.Net;

namespace Gateway.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class OrderProxyController : ControllerBase
    {
        public OrderProxyController(
           IClusterClient orleansClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _clientIp = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString() ??
                        httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? new Guid().ToString();

            _orleansClient = orleansClient;
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
            var g1 = _orleansClient.GetGrain<IUserService>(_clientIp);
            var r1 = await g1.GetUserInfoAsync(input.CustomerUid);
            if (!r1.IsSuccess) return BadRequest(r1);

            var g2 = _orleansClient.GetGrain<IStoreService>(_clientIp);
            var r2 = await g2.GetStoreInfoByIdAsync(input.StoreUid);
            if (!r2.IsSuccess) return BadRequest(r2);

            var g3 = _orleansClient.GetGrain<IOrderService>(_clientIp);
            var r3 = await g3.CreateAsync(input);

            if (r3.IsSuccess) return Ok(r3);
            else return BadRequest(r3.ProblemDetails);
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
        {
            var grain = _orleansClient.GetGrain<IOrderService>(_clientIp);
            var res = await grain.DeleteAsync(orderId);

            if (res.IsSuccess) return Ok(res);
            else return BadRequest(res.ProblemDetails);
        }

        /// <summary>
        /// 骑手接单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("{orderId}/{riderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<HttpStatusCode>> RiderGetAsync(long orderId, long riderId)
        {
            var g1 = _orleansClient.GetGrain<IUserService>(_clientIp);
            var r1 = await g1.GetUserInfoAsync(riderId);
            if (!r1.IsSuccess) return BadRequest(r1);

            var g3 = _orleansClient.GetGrain<IOrderService>(_clientIp);
            var r3 = await g3.RiderGetAsync(orderId, riderId);

            if (r3.IsSuccess) return Ok(r3);
            else return BadRequest(r3.ProblemDetails);
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
            var g1 = _orleansClient.GetGrain<IUserService>(_clientIp);
            var r1 = await g1.GetUserInfoAsync(cutomerId);
            if (!r1.IsSuccess) return BadRequest(r1);

            var g3 = _orleansClient.GetGrain<IOrderService>(_clientIp);
            var r3 = await g3.FinishOrderAsync(orderId, cutomerId);

            if (r3.IsSuccess) return Ok(r3);
            else return BadRequest(r3.ProblemDetails);
        }

        /// <summary>
        /// 用户申请订单退款
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("{orderId}/{cutomerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<HttpStatusCode>> RefundOrderAsync(long orderId, long cutomerId)
        {
            var grain = _orleansClient.GetGrain<IOrderService>(_clientIp);
            var res = await grain.RefundOrderAsync(orderId, cutomerId);

            if (res.IsSuccess) return Ok(res);
            else return BadRequest(res.ProblemDetails);
        }

        /// <summary>
        /// 用户获取订单信息
        /// </summary>
        /// <param name="customerUid"></param>
        /// <returns></returns>
        [HttpPut("{cutomerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<OrderInfoDto>>> GetOrdersByCustomerUid(long customerUid)
        {
            var grain = _orleansClient.GetGrain<IOrderService>(_clientIp);
            var res = await grain.GetOrdersByCustomerUid(customerUid);

            if (res.IsSuccess) return Ok(res);
            else return BadRequest(res.ProblemDetails);
        }

        /// <summary>
        /// 骑手获取订单信息
        /// </summary>
        /// <param name="customerUid"></param>
        /// <returns></returns>
        [HttpGet("{riderUid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<OrderInfoDto>>> GetOrdersByRiderUid(long riderUid)
        {
            var grain = _orleansClient.GetGrain<IOrderService>(_clientIp);
            var res = await grain.GetOrdersByRiderUid(riderUid);

            if (res.IsSuccess) return Ok(res);
            else return BadRequest(res.ProblemDetails);
        }

        /// <summary>
        /// 店铺获取订单信息
        /// </summary>
        /// <param name="customerUid"></param>
        /// <returns></returns>
        [HttpGet("{storeUid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<OrderInfoDto>>> GetOrdersByStoreUid(long storeUid)
        {
            var grain = _orleansClient.GetGrain<IOrderService>(_clientIp);
            var res = await grain.GetOrdersByStoreUid(storeUid);

            if (res.IsSuccess) return Ok(res);
            else return BadRequest(res.ProblemDetails);
        }

        //private readonly IOrderService _orderService;
        //private readonly IUserService _userService;
        //private readonly IStoreService _storeService;
        private readonly IClusterClient _orleansClient;
        private readonly string _clientIp;
    }
}
