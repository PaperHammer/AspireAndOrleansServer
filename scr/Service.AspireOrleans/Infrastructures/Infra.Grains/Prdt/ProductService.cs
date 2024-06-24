using GrainInterfaces.Prdt;
using Infra.Core.Core.Guard;
using Infra.Core.System.Extensions.ObjectExt;
using Infra.IdGenerater.Yitter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prdt.App.Contract.Dtos;
using Prdt.Repository.Core;
using Prdt.Repository.Entities;
using Shared.App.ResultModel;
using Shared.App.Services;
using System.Net;
using System.Text.Json;

namespace Grains.Prdt
{
    public class ProductService(
            ProductDbContext productDbContext,
            ILogger<ProductService> logger)
            : Grain, IProductService
    {
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
                var product = await _productDbContext.Products.FirstOrDefaultAsync(u => u.Uid == id);
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
                    IsDelete = product.IsDelete,
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
                        if (product.StoreUid != storeId) continue;
                        ProductInfoDto productInfoDto = new()
                        {
                            Uid = product.Uid,
                            StoreUid = product.StoreUid,
                            CategoryUid = product.CategoryUid,
                            ProductName = product.ProductName,
                            ProductDesc = product.ProductDesc,
                            ProductImage = product.ProductImage,
                            ProductPrice = product.ProductPrice,
                            IsDelete = product.IsDelete,
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
                        ProductInfoDto productInfoDto = new()
                        {
                            Uid = product.Uid,
                            StoreUid = product.StoreUid,
                            CategoryUid = product.CategoryUid,
                            ProductName = product.ProductName,
                            ProductDesc = product.ProductDesc,
                            ProductImage = product.ProductImage,
                            ProductPrice = product.ProductPrice,
                            IsDelete = product.IsDelete,
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

                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return AbstractAppService.Problem(HttpStatusCode.InternalServerError, ex.Message + "----");
            }
        }

        private ProductDbContext _productDbContext = productDbContext;
        private ILogger<ProductService> _logger = logger;
    }
}
