using Prdt.App.Contract.Dtos;
using Shared.App.Attributes;
using Shared.App.Interfaces;
using Shared.App.ResultModel;
using System.Net;

namespace Prdt.GrainInterfaces
{
    [Alias("Prdt.GrainInterfaces.IProductService")]
    public interface IProductService : IAppService, IGrainWithStringKey
    {
        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "新增商品")]
        [Alias("Product_CreateAsync")]
        Task<AppSrvResult<long>> CreateAsync(ProductCreationDto input);

        /// <summary>
        /// 修改商品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "修改商品")]
        [Alias("Product_UpdateInfoAsync")]
        Task<AppSrvResult<long>> UpdateInfoAsync(long id, ProductUpdationDto input);

        /// <summary>
        /// 下架商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperateLog(LogName = "下架商品")]
        [Alias("Product_DownProductAsync")]
        Task<AppSrvResult<HttpStatusCode>> DownProductAsync(long id, long storeId);
        
        /// <summary>
        /// 上架商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperateLog(LogName = "上架商品")]
        [Alias("Product_UpProductAsync")]
        Task<AppSrvResult<HttpStatusCode>> UpProductAsync(long id, long storeId);

        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Alias("Product_GetProductInfoByIdAsync")]
        Task<AppSrvResult<ProductInfoDto>> GetProductInfoByIdAsync(long id);

        /// <summary>
        /// 获取店铺所有商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Alias("Product_GetProductInfosByStoreIdAsync")]
        Task<AppSrvResult<List<ProductInfoDto>>> GetProductInfosByStoreIdAsync(long storeId);

        /// <summary>
        /// 获取所有商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Alias("Product_GetAllProductInfosAsync")]
        Task<AppSrvResult<List<ProductInfoDto>>> GetAllProductInfosAsync();
    }
}
