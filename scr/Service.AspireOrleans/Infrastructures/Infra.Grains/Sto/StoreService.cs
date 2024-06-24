using GrainInterfaces.Sto;
using Microsoft.Extensions.Logging;
using Shared.App.ResultModel;
using Sto.App.Contract.Dtos;
using Sto.Repository.Core;
using System.Net;

namespace Grains.Sto
{
    public class StoreService(
        StoreDbContext storeDbContext,
        ILogger<StoreService> logger)
        : Grain, IStoreService
    {


        private StoreDbContext _storeDbContext = storeDbContext;
        private ILogger<StoreService> _logger = logger;

        public Task<AppSrvResult<long>> CreateAsync(StoreCreationDto input)
        {
            throw new NotImplementedException();
        }

        public Task<AppSrvResult<HttpStatusCode>> DownStoreAsync(long id, long storeId)
        {
            throw new NotImplementedException();
        }

        public Task<AppSrvResult<StoreInfoDto>> GetStoreInfoByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<AppSrvResult<long>> UpdateInfoAsync(long id, StoreUpdationDto input)
        {
            throw new NotImplementedException();
        }

        public Task<AppSrvResult<HttpStatusCode>> UpStoreAsync(long id, long storeId)
        {
            throw new NotImplementedException();
        }
    }
}
