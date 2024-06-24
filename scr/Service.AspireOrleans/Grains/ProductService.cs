using GrainInterfaces;
using Infra.Core.Core.Guard;
using Infra.Core.System.Extensions.ObjectExt;
using Infra.IdGenerater.Yitter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Orleans.Placement;
using Prdt.App.Contract.Dtos;
using Prdt.Repository.Core;
using Prdt.Repository.Entities;
using Shared.App.ResultModel;
using Shared.App.Services;
using System.Net;
using System.Text.Json;

namespace Grains
{
    // RandomPlacement：随机选择一个可用节点来放置Grain对象。这也是默认的策略。
    // ActivationCountBasedPlacement：根据节点上已激活的Grain对象数量选择当前负载最轻的节点来放置Grain对象.
    // PreferLocalPlacement：优先将Grain对象放置在发起调用的本地节点上，以减少跨节点通信的延迟。
    // CustomPlacement：允许用户根据特定需求自定义Grain放置策略。
    [ActivationCountBasedPlacement]
    public class ProductService(ILogger<ProductService> logger)
        : Grain, IProductService
    {
        static ProductService()
        {
            _productDbContext = new ProductDbContext();
        }

        public async Task<AppSrvResult<long>> CreateAsync(ProductCreationDto input)
        {
            try
            {
                input.TrimStringFields();
                Checker.Argument.IsNotEmpty(input.ProductName, nameof(input.ProductName));

                Product product = new()
                {
                    Uid = IdGenerater.GetNextId(),
                    CategoryUid = input.CategoryUid,
                    StoreUid = input.StoreUid,
                    ProductImage = input.ProductImage,
                    ProductName = input.ProductName,
                    ProductDesc = input.ProductDesc,
                    ProductPrice = input.ProductPrice,
                };

                await _productDbContext.AddAsync(product);
                await _productDbContext.SaveChangesAsync();

                _logger.LogInformation(
                    $"\n Create a Product successfully. {JsonSerializer.Serialize(product)} \n----");

                return product.Uid;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<HttpStatusCode>> DownProductAsync(long id, long storeId)
        {
            try
            {
                var product = await _productDbContext.Products.FirstOrDefaultAsync(u => u.Uid == id && u.StoreUid == storeId);
                if (product == null)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "商品不存在");

                product.IsDelete = true;

                await _productDbContext.SaveChangesAsync();

                _logger.LogInformation(
                   $"\n Down a Product successfully. id: {id} \n----");

                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<ProductInfoDto>> GetProductInfoByIdAsync(long id)
        {
            try
            {
                var product = await _productDbContext.Products.FirstOrDefaultAsync(u => u.Uid == id && !u.IsDelete);
                if (product == null)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "商品不存在");

                ProductInfoDto productInfoDto = new()
                {
                    Uid = product.Uid,
                    StoreUid = product.StoreUid,
                    CategoryUid = product.CategoryUid,
                    ProductName = product.ProductName,
                    ProductDesc = product.ProductDesc,
                    ProductImage = product.ProductImage,
                    ProductPrice = product.ProductPrice,
                };

                return productInfoDto;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<List<ProductInfoDto>>> GetProductInfosByStoreIdAsync(long storeId)
        {
            try
            {
                List<ProductInfoDto> products = [];
                await Task.Run(() =>
                {
                    foreach (var product in _productDbContext.Products)
                    {
                        if (product.StoreUid != storeId || product.IsDelete) continue;
                        ProductInfoDto productInfoDto = new()
                        {
                            Uid = product.Uid,
                            StoreUid = product.StoreUid,
                            CategoryUid = product.CategoryUid,
                            ProductName = product.ProductName,
                            ProductDesc = product.ProductDesc,
                            ProductImage = product.ProductImage,
                            ProductPrice = product.ProductPrice,
                        };
                        products.Add(productInfoDto);
                    }
                });

                return products;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<List<ProductInfoDto>>> GetAllProductInfosAsync()
        {
            try
            {
                List<ProductInfoDto> products = [];
                await Task.Run(() =>
                {
                    foreach (var product in _productDbContext.Products)
                    {
                        if (product.IsDelete) continue;
                        ProductInfoDto productInfoDto = new()
                        {
                            Uid = product.Uid,
                            StoreUid = product.StoreUid,
                            CategoryUid = product.CategoryUid,
                            ProductName = product.ProductName,
                            ProductDesc = product.ProductDesc,
                            ProductImage = product.ProductImage,
                            ProductPrice = product.ProductPrice,
                        };
                        products.Add(productInfoDto);
                    }
                });

                return products;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<long>> UpdateInfoAsync(long id, ProductUpdationDto input)
        {
            try
            {
                var product = await _productDbContext.Products.FirstOrDefaultAsync(u => u.Uid == id);
                if (product == null)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "商品不存在");

                product.CategoryUid = input.CategoryUid;
                product.ProductName = input.ProductName;
                product.ProductDesc = input.ProductDesc;
                product.ProductImage = input.ProductImage;
                product.ProductPrice = input.ProductPrice;

                await _productDbContext.SaveChangesAsync();

                return id;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        public async Task<AppSrvResult<HttpStatusCode>> UpProductAsync(long id, long storeId)
        {
            try
            {
                var product = await _productDbContext.Products.FirstOrDefaultAsync(u => u.Uid == id && u.StoreUid == storeId);
                if (product == null)
                    return AbstractAppService.Problem(HttpStatusCode.BadRequest, "商品不存在");

                product.IsDelete = false;

                await _productDbContext.SaveChangesAsync();

                _logger.LogInformation(
                   $"\n Up a Product successfully. id: {id} \n----");

                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        private static readonly ProductDbContext _productDbContext;
        private readonly ILogger<ProductService> _logger = logger;
    }
}
