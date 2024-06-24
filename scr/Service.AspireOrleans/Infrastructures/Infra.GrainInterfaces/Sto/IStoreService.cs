using Shared.App.Attributes;
using Shared.App.Interfaces;
using Shared.App.ResultModel;
using Sto.App.Contract.Dtos;
using System.Net;

namespace GrainInterfaces.Sto
{
    [Alias("GrainInterfaces.Sto.IStoreService")]
    public interface IStoreService : IAppService, IGrainWithStringKey
    {
        /// <summary>
        /// 新增店铺
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "新增店铺")]
        [Alias("CreateAsync")]
        Task<AppSrvResult<long>> CreateAsync(StoreCreationDto input);

        /// <summary>
        /// 修改店铺
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "修改店铺")]
        [Alias("UpdateInfoAsync")]
        Task<AppSrvResult<long>> UpdateInfoAsync(long id, StoreUpdationDto input);

        /// <summary>
        /// 下架店铺
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperateLog(LogName = "下架店铺")]
        Task<AppSrvResult<HttpStatusCode>> DownStoreAsync(long id, long storeId);

        /// <summary>
        /// 上架店铺
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperateLog(LogName = "上架店铺")]
        Task<AppSrvResult<HttpStatusCode>> UpStoreAsync(long id, long storeId);

        /// <summary>
        /// 获取店铺信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Alias("GetStoreInfoByIdAsync")]
        Task<AppSrvResult<StoreInfoDto>> GetStoreInfoByIdAsync(long id);
    }
}
