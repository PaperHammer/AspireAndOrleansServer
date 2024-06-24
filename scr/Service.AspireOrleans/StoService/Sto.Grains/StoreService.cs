using Infra.Core.Core.Guard;
using Infra.Core.System.Extensions.ObjectExt;
using Infra.IdGenerater.Yitter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.App.ResultModel;
using Shared.App.Services;
using Sto.App.Contract.Dtos;
using Sto.GrainInterfaces;
using Sto.Repository.Core;
using Sto.Repository.Entities;
using System.Net;
using System.Text.Json;

namespace Sto.Grains
{
    public class StoreService(
        StoreDbContext storeDbContext,
        ILogger<StoreService> logger)
        : Grain, IStoreService
    {
        public async Task<AppSrvResult<long>> CreateAsync(StoreCreationDto input)
        {
            try
            {
                input.TrimStringFields();

                #region InputCheck 输入检测
                if (await _storeDbContext.Stores.FirstOrDefaultAsync(u => u.StoreName == input.StoreName) != null)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "店铺名称已经存在");

                Checker.Argument.IsNotEmpty(input.StoreName, "StoreName");
                Checker.Argument.IsNotEmpty(input.Position, "Position");
                #endregion

                Store store = new()
                {
                    Uid = IdGenerater.GetNextId(),
                    OwnerId = input.OwnerId,
                    StoreImage = input.StoreImage,
                    StoreName = input.StoreName,
                    Position = input.Position,
                    Score = 2.5,
                    StoreDesc = input.StoreDesc,
                    OpeningTime = input.OpeningTime,
                    ClosingTime = input.ClosingTime,
                    StartPrice = input.StartPrice,
                    DeliveryPrice = input.DeliveryPrice,
                };

                await _storeDbContext.AddAsync(store);
                await _storeDbContext.SaveChangesAsync();

                _logger.LogInformation(
                    $"\n Create a Store successfully. {JsonSerializer.Serialize(store)} \n----");

                return store.Uid;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<long>> UpdateInfoAsync(long id, StoreUpdationDto input)
        {
            try
            {
                input.TrimStringFields();

                #region InputCheck 输入检测
                Checker.Argument.IsNotEmpty(input.StoreName, "StoreName");
                Checker.Argument.IsNotEmpty(input.Position, "Position");               

                var store = await _storeDbContext.Stores.FirstOrDefaultAsync(u => u.Uid == id && !u.IsDelete); // Detached
                if (store == null)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "店铺不存在");

                if (store.OwnerId != input.OwnerId)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "用户没有该店铺的管理权");
                #endregion

                store.StoreImage = input.StoreImage;
                store.StoreName = input.StoreName;
                store.Position = input.Position;
                store.StoreDesc = input.StoreDesc;
                store.OpeningTime = input.OpeningTime;
                store.ClosingTime = input.ClosingTime;
                store.StartPrice = input.StartPrice;
                store.DeliveryPrice = input.DeliveryPrice;

                await _storeDbContext.SaveChangesAsync();

                _logger.LogInformation(
                   $"\n Update for a Store successfully. id: {id} \n----");

                return id;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<HttpStatusCode>> UpStoreAsync(long ownerId, long storeId)
        {
            try
            {
                var store = await _storeDbContext.Stores.FirstOrDefaultAsync(u => u.Uid == storeId && !u.IsDelete); // Detached
                if (store == null)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "店铺不存在");

                if (store.OwnerId != ownerId)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "用户没有该店铺的管理权");

                store.IsDelete = false;

                await _storeDbContext.SaveChangesAsync();

                _logger.LogInformation(
                   $"\n Up a Store successfully. id: {storeId} \n----");

                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<HttpStatusCode>> DownStoreAsync(long ownerId, long storeId)
        {
            try
            {
                var store = await _storeDbContext.Stores.FirstOrDefaultAsync(u => u.Uid == storeId && !u.IsDelete); // Detached
                if (store == null)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "店铺不存在");

                if (store.OwnerId != ownerId)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "用户没有该店铺的管理权");

                store.IsDelete = true;

                await _storeDbContext.SaveChangesAsync();

                _logger.LogInformation(
                   $"\n Down a Store successfully. id: {storeId} \n----");

                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<StoreInfoDto>> GetStoreInfoByIdAsync(long id)
        {
            try
            {
                var store = await _storeDbContext.Stores.FirstOrDefaultAsync(u => u.Uid == id && !u.IsDelete);
                if (store == null)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "店铺不存在");

                StoreInfoDto storeInfoDto = new()
                {
                    Uid = IdGenerater.GetNextId(),
                    OwnerId = store.OwnerId,
                    StoreImage = store.StoreImage,
                    StoreName = store.StoreName,
                    Position = store.Position,
                    Score = store.Score,
                    StoreDesc = store.StoreDesc,
                    OpeningTime = store.OpeningTime,
                    ClosingTime = store.ClosingTime,
                    StartPrice = store.StartPrice,
                    DeliveryPrice = store.DeliveryPrice,
                };

                return storeInfoDto;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<List<StoreInfoDto>>> GetNewStoresAsync(int tag)
        {
            try
            {
                List<StoreInfoDto> storeInfoDtos = [];

                await Task.Run(() =>
                {
                    var stores = _storeDbContext.Stores.Skip(tag).Take(1);

                    foreach (var store in stores)
                    {
                        if (store.IsDelete) continue;
                        StoreInfoDto storeInfoDto = new()
                        {
                            Uid = IdGenerater.GetNextId(),
                            OwnerId = store.OwnerId,
                            StoreImage = store.StoreImage,
                            StoreName = store.StoreName,
                            Position = store.Position,
                            Score = store.Score,
                            StoreDesc = store.StoreDesc,
                            OpeningTime = store.OpeningTime,
                            ClosingTime = store.ClosingTime,
                            StartPrice = store.StartPrice,
                            DeliveryPrice = store.DeliveryPrice,
                        };
                        storeInfoDtos.Add(storeInfoDto);
                    }
                });

                return storeInfoDtos;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        private StoreDbContext _storeDbContext = storeDbContext;
        private ILogger<StoreService> _logger = logger;
    }
}
