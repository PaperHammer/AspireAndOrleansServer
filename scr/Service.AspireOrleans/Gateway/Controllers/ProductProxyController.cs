using GrainInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Bulkhead;
using Polly.CircuitBreaker;
using Polly.RateLimit;
using Polly.Retry;
using Polly.Wrap;
using Prdt.App.Contract.Dtos;
using System.Net;

namespace Gateway.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ProductProxyController : ControllerBase
    {
        public ProductProxyController(
            IClusterClient orleansClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _clientIp = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString() ??
                        httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? new Guid().ToString();

            _orleansClient = orleansClient;
        }

        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="input">商品信息</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<long>> CreateAsync([FromBody] ProductCreationDto input)
        {
            var grain = _orleansClient.GetGrain<IProductService>(_clientIp);
            var result = await grain.CreateAsync(input);

            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result.ProblemDetails);
        }

        /// <summary>
        /// 修改商品
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="input">商品信息</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "paperhammer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<long>> UpdateInfoAsync([FromRoute] long id, [FromBody] ProductUpdationDto input)
        {
            var grain = _orleansClient.GetGrain<IProductService>(_clientIp);
            var result = await grain.UpdateInfoAsync(id, input);

            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result.ProblemDetails);
        }

        /// <summary>
        /// 下架
        /// </summary>
        /// <param name="pid">商品ID</param>
        /// <param name="storeId">店铺ID</param>
        /// <returns></returns>
        [HttpPut("{pid}/{storeId}")]
        //[Authorize(Roles = "paperhammer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<HttpStatusCode>> UpProductAsync([FromRoute] long pid, [FromRoute] long storeId)
        {
            var grain = _orleansClient.GetGrain<IProductService>(_clientIp);
            var result = await grain.UpProductAsync(pid, storeId);

            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result.ProblemDetails);
        }

        /// <summary>
        /// 上架
        /// </summary>
        /// <param name="pid">商品ID</param>
        /// <param name="storeId">店铺ID</param>
        /// <returns></returns>
        [HttpPut("{pid}/{storeId}")]
        //[Authorize(Roles = "paperhammer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<HttpStatusCode>> DownProductAsync([FromRoute] long pid, [FromRoute] long storeId)
        {
            var grain = _orleansClient.GetGrain<IProductService>(_clientIp);
            var result = await grain.DownProductAsync(pid, storeId);

            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result.ProblemDetails);
        }

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductInfoDto>> GetProductInfoByIdAsync([FromRoute] long id)
        {
            var grain = _orleansClient.GetGrain<IProductService>(_clientIp);
            var result = await grain.GetProductInfoByIdAsync(id);

            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result.ProblemDetails);
        }

        /// <summary>
        /// 获取店铺所有商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ProductInfoDto>>> GetProductInfosByStoreIdAsync([FromRoute] long id)
        {
            var grain = _orleansClient.GetGrain<IProductService>(_clientIp);
            var result = await grain.GetProductInfosByStoreIdAsync(id);

            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result.ProblemDetails);
        }

        /// <summary>
        /// 获取所有商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ProductInfoDto>>> GetAllProductInfosAsync()
        {
            //var result = await _policyWrap.ExecuteAsync(async () =>
            //{
                var grain = _orleansClient.GetGrain<IProductService>(_clientIp);
                //return await grain.GetAllProductInfosAsync();
                var result = await grain.GetAllProductInfosAsync();
            //});

            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result.ProblemDetails);          
        }

        private readonly IClusterClient _orleansClient;
        private readonly string _clientIp;

        #region Polly
        // 简单重试策略，重试1次，每次间隔 2^cnt 秒
        private static AsyncRetryPolicy _retryPolicy = Policy.Handle<Exception>()
            .WaitAndRetryAsync(1, retryAttempt =>
            {
                var waitSeconds = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt - 1));
                Console.WriteLine(DateTime.Now.ToString() + "-Retry:[" + retryAttempt + "], wait " + waitSeconds + "s!");
                return waitSeconds;
            });

        // 熔断策略，发生2次请求失败时开启熔断，熔断持续时间为10秒后尝试半开
        private static AsyncCircuitBreakerPolicy _circuitBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(2, TimeSpan.FromSeconds(10));

        //private static AsyncCircuitBreakerPolicy __circuitBreakerPolicy = Policy.Handle<Exception>()
        //.AdvancedCircuitBreakerAsync(
        //    failureThreshold: 0.5, // 异常比例阈值，这里设置为50%
        //    samplingDuration: TimeSpan.FromSeconds(2), // 观察窗口时间，这里为2秒
        //    minimumThroughput: 2, // 最小请求数，必须有足够的样本才判断是否熔断
        //    durationOfBreak: TimeSpan.FromSeconds(10) // 熔断持续时间
        //);

        // 限流策略，每30秒不超过3个请求
        private static AsyncRateLimitPolicy _rateLimiterPolicy = Policy
            .RateLimitAsync(3, TimeSpan.FromSeconds(30));

        //private static AsyncTimeoutPolicy _timeoutPolicy = Policy.
        //    TimeoutAsync(5);

        //private static AsyncFallbackPolicy<string> _fallbackPolicy = Policy<string>.Handle<Exception>()
        //    .FallbackAsync(
        //        fallbackValue: "substitute data",
        //        onFallbackAsync: (exception, context) =>
        //        {
        //            Console.WriteLine("It's Fallback,  Exception->" + exception.Exception.Message + ", return substitute data.");
        //            return Task.CompletedTask;
        //        });

        private static AsyncBulkheadPolicy _bulkheadPolicy = Policy.BulkheadAsync(
            maxParallelization: 10, // 最大并发数
            maxQueuingActions: 20 // 最大排队数量
        );

        // 组合策略，顺序执行上述策略
        private static AsyncPolicyWrap _policyWrap = Policy.WrapAsync(
            _retryPolicy,
            _circuitBreakerPolicy,
            _rateLimiterPolicy,
            _bulkheadPolicy);
        #endregion
    }
}
