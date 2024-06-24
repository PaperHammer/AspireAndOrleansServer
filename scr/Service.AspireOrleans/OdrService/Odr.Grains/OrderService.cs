using Infra.Helper.Enums;
using Infra.IdGenerater.Yitter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Odr.App.Contract.Dtos;
using Odr.GrainInterfaces;
using Odr.Repository.Core;
using Odr.Repository.Entities;
using Shared.App.ResultModel;
using Shared.App.Services;
using System.Net;
using System.Text.Json;

namespace Odr.Grains
{
    public class OrderService(
        OrderDbContext orderDbContext,
        ILogger<OrderService> logger)
        : Grain, IOrderService
    {        
        public async Task<AppSrvResult<long>> CreateAsync(OrderCreationDto input)
        {
            try
            {
                Order order = new()
                {
                    Uid = IdGenerater.GetNextId(),
                    CustomerUid = input.CustomerUid,
                    StoreUid = input.StoreUid,
                    CreateTime = input.CreateTime,
                    StartPrice = input.StartPrice,
                    DeliveryPrice = input.DeliveryPrice,
                    Statu = (int)OrderStatuEnum.StoreRecieved,
                };

                await _orderDbContext.AddAsync(order);
                await _orderDbContext.SaveChangesAsync();

                _logger.LogInformation(
                    $"\n Create a Order successfully. {JsonSerializer.Serialize(order)} \n----");

                return order.Uid;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<HttpStatusCode>> DeleteAsync(long orderId)
        {
            try
            {
                var order = await _orderDbContext.Orders.FirstOrDefaultAsync(
                    u => u.Uid == orderId 
                         && (u.Statu == (int)OrderStatuEnum.Finished || u.Statu == (int)OrderStatuEnum.Refund));
                if (order == null)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "订单未完成或不存在，无法删除");

                order.IsDelete = true;

                await _orderDbContext.SaveChangesAsync();

                _logger.LogInformation(
                   $"\n Delete a Order successfully. id: {orderId} \n----");

                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<List<OrderInfoDto>>> GetOrdersByCustomerUid(long customerUid)
        {
            try
            {
                List<OrderInfoDto> orders = [];
                await Task.Run(() =>
                {
                    foreach (var order in _orderDbContext.Orders)
                    {
                        if (order.CustomerUid != customerUid || order.IsDelete) continue;
                        OrderInfoDto productInfoDto = new()
                        {
                            Uid = order.Uid,
                            CustomerUid = order.CustomerUid,
                            StoreUid = order.StoreUid,
                            CreateTime = order.CreateTime,
                            FinishTime = order.FinishTime,
                            StartPrice = order.StartPrice,
                            DeliveryPrice = order.DeliveryPrice,
                            Statu = order.Statu,
                        };
                        orders.Add(productInfoDto);
                    }
                });

                return orders;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<List<OrderInfoDto>>> GetOrdersByRiderUid(long riderUid)
        {
            try
            {
                List<OrderInfoDto> orders = [];
                await Task.Run(() =>
                {
                    foreach (var order in _orderDbContext.Orders)
                    {
                        if (order.RiderUid != riderUid || order.IsDelete) continue;
                        OrderInfoDto productInfoDto = new()
                        {
                            Uid = order.Uid,
                            CustomerUid = order.CustomerUid,
                            StoreUid = order.StoreUid,
                            CreateTime = order.CreateTime,
                            FinishTime = order.FinishTime,
                            StartPrice = order.StartPrice,
                            DeliveryPrice = order.DeliveryPrice,
                            Statu = order.Statu,
                        };
                        orders.Add(productInfoDto);
                    }
                });

                return orders;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<List<OrderInfoDto>>> GetOrdersByStoreUid(long storeUid)
        {
            try
            {
                List<OrderInfoDto> orders = [];
                await Task.Run(() =>
                {
                    foreach (var order in _orderDbContext.Orders)
                    {
                        if (order.StoreUid != storeUid || order.IsDelete) continue;
                        OrderInfoDto productInfoDto = new()
                        {
                            Uid = order.Uid,
                            CustomerUid = order.CustomerUid,
                            StoreUid = order.StoreUid,
                            CreateTime = order.CreateTime,
                            FinishTime = order.FinishTime,
                            StartPrice = order.StartPrice,
                            DeliveryPrice = order.DeliveryPrice,
                            Statu = order.Statu,
                        };
                        orders.Add(productInfoDto);
                    }
                });

                return orders;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<HttpStatusCode>> RiderGetAsync(long orderId, long riderId)
        {
            try
            {
                var order = await _orderDbContext.Orders.FirstOrDefaultAsync(
                    u => u.Uid == orderId && (u.Statu == (int)OrderStatuEnum.StoreRecieved));
                if (order == null)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "订单已被接取或不存在");

                order.RiderUid = riderId;
                order.Statu = (int)OrderStatuEnum.RiderRecieved;

                await _orderDbContext.SaveChangesAsync();

                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<HttpStatusCode>> FinishOrderAsync(long orderId, long cutomerId)
        {
            try
            {
                var order = await _orderDbContext.Orders.FirstOrDefaultAsync(
                    u => u.Uid == orderId && u.CustomerUid == cutomerId);
                if (order == null)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "订单尚未送达或不存在，无法完成");

                order.Statu = (int)OrderStatuEnum.Finished;

                await _orderDbContext.SaveChangesAsync();

                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<HttpStatusCode>> RefundOrderAsync(long orderId, long cutomerId)
        {
            try
            {
                var order = await _orderDbContext.Orders.FirstOrDefaultAsync(
                    u => u.Uid == orderId && u.CustomerUid == cutomerId && (u.Statu == (int)OrderStatuEnum.Finished));
                if (order == null)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "订单尚未完成或不存在，无法退款");

                order.Statu = (int)OrderStatuEnum.Refund;

                await _orderDbContext.SaveChangesAsync();

                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        private readonly OrderDbContext _orderDbContext = orderDbContext;
        private readonly ILogger<OrderService> _logger = logger;
    }
}
