using Shared.App.Attributes;
using Shared.App.Interfaces;
using Shared.App.ResultModel;
using Sto.App.Contract.Dtos;
using System.Net;

namespace Sto.GrainInterfaces
{
    [Alias("Sto.GrainInterfaces.IStoreService")]
    public interface IStoreService : IAppService, IGrainWithStringKey
    {
        /// <summary>
        /// 新增店铺
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "新增店铺")]
        [Alias("Store_CreateAsync")]
        Task<AppSrvResult<long>> CreateAsync(StoreCreationDto input);

        /// <summary>
        /// 修改店铺
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "修改店铺")]
        [Alias("Store_UpdateInfoAsync")]
        Task<AppSrvResult<long>> UpdateInfoAsync(long id, StoreUpdationDto input);

        /// <summary>
        /// 下架店铺
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperateLog(LogName = "下架店铺")]
        [Alias("Store_DownStoreAsync")]
        Task<AppSrvResult<HttpStatusCode>> DownStoreAsync(long ownerId, long storeId);

        /// <summary>
        /// 上架店铺
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperateLog(LogName = "上架店铺")]
        [Alias("Store_UpStoreAsync")]
        Task<AppSrvResult<HttpStatusCode>> UpStoreAsync(long ownerId, long storeId);

        /// <summary>
        /// 获取店铺信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Alias("Store_GetStoreInfoByIdAsync")]
        Task<AppSrvResult<StoreInfoDto>> GetStoreInfoByIdAsync(long id);

        /// <summary>
        /// 按增量获取店铺
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [Alias("Store_GetNewStoresAsync")]
        Task<AppSrvResult<List<StoreInfoDto>>> GetNewStoresAsync(int tag);
    }
}
