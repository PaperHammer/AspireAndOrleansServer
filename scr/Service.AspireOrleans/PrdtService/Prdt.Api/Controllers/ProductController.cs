using GrainInterfaces;
using Microsoft.AspNetCore.Mvc;
using Prdt.App.Contract.Dtos;
using Shared.WebApi.Controllers;
using System.Net;

namespace Prdt.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ProductController : SharedControllerBase
    {
        public ProductController(
            IGrainFactory grainFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            string clientIp = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString() ??
                              httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? new Guid().ToString();

            _productService = grainFactory.GetGrain<IProductService>(clientIp);
        }

        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="input">商品信息</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<long>> CreateAsync([FromBody] ProductCreationDto input)
            => CreatedResult(await _productService.CreateAsync(input));

        /// <summary>
        /// 修改商品
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="input">商品信息</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        //[Authorize(Roles = "paperhammer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<long>> UpdateInfoAsync([FromRoute] long id, [FromBody] ProductUpdationDto input)
            => Result(await _productService.UpdateInfoAsync(id, input));

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
            => Result(await _productService.UpProductAsync(pid, storeId));

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
            => Result(await _productService.DownProductAsync(pid, storeId));

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductInfoDto>> GetProductInfoByIdAsync([FromRoute] long id)
            => Result(await _productService.GetProductInfoByIdAsync(id));
        
        /// <summary>
        /// 获取店铺所有商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ProductInfoDto>>> GetProductInfosByStoreIdAsync([FromRoute] long id)
            => Result(await _productService.GetProductInfosByStoreIdAsync(id));
        
        /// <summary>
        /// 获取所有商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ProductInfoDto>>> GetAllProductInfosAsync()
            => Result(await _productService.GetAllProductInfosAsync());

        private IProductService _productService;
    }
}
