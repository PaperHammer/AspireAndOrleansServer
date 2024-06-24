using Infra.Helper.Enums;
using Odr.App.Contract.Dtos;
using Shared.App.Attributes;
using Shared.App.Interfaces;
using Shared.App.ResultModel;
using System.Net;

namespace Odr.GrainInterfaces
{
    [Alias("Odr.GrainInterfaces.IOrderService")]
    public interface IOrderService : IAppService, IGrainWithStringKey
    {
        /// <summary>
        /// 新增订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "新增订单")]
        [Alias("Odr_CreateAsync")]
        Task<AppSrvResult<long>> CreateAsync(OrderCreationDto input);

        /// <summary>
        /// 删除已支付或退款的订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [OperateLog(LogName = "删除已支付或退款的订单")]
        [Alias("Odr_DeleteAsync")]
        Task<AppSrvResult<HttpStatusCode>> DeleteAsync(long orderId);

        /// <summary>
        /// 骑手接单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [OperateLog(LogName = "骑手接单")]
        [Alias("Odr_RiderGetAsync")]
        Task<AppSrvResult<HttpStatusCode>> RiderGetAsync(long orderId, long riderId);

        /// <summary>
        /// 完成订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [OperateLog(LogName = "完成订单")]
        [Alias("Odr_FinishOrderAsync")]
        Task<AppSrvResult<HttpStatusCode>> FinishOrderAsync(long orderId, long cutomerId);
        
        /// <summary>
        /// 用户申请订单退款
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [OperateLog(LogName = "用户申请订单退款")]
        [Alias("Odr_RefundOrderAsync")]
        Task<AppSrvResult<HttpStatusCode>> RefundOrderAsync(long orderId, long cutomerId);

        /// <summary>
        /// 用户获取订单信息
        /// </summary>
        /// <param name="customerUid"></param>
        /// <returns></returns>
        [OperateLog(LogName = "用户获取订单信息")]
        [Alias("Odr_GetOrdersByCustomerUid")]
        Task<AppSrvResult<List<OrderInfoDto>>> GetOrdersByCustomerUid(long customerUid);

        /// <summary>
        /// 骑手获取订单信息
        /// </summary>
        /// <param name="customerUid"></param>
        /// <returns></returns>
        [OperateLog(LogName = "骑手获取订单信息")]
        [Alias("Odr_GetOrdersByRiderUid")]
        Task<AppSrvResult<List<OrderInfoDto>>> GetOrdersByRiderUid(long riderUid);

        /// <summary>
        /// 店铺获取订单信息
        /// </summary>
        /// <param name="customerUid"></param>
        /// <returns></returns>
        [OperateLog(LogName = "店铺获取订单信息")]
        [Alias("Odr_GetOrdersByStoreUid")]
        Task<AppSrvResult<List<OrderInfoDto>>> GetOrdersByStoreUid(long storeUid);
    }
}
