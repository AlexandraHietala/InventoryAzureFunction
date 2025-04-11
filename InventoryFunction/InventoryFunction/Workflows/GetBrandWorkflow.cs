using InventoryFunction.Data;
using InventoryFunction.Models.Classes;
using InventoryFunction.Models.Converters;
using InventoryFunction.Models.DTOs;
using InventoryFunction.Validators.DataValidators;
using InventoryFunction.Validators.DeepValidators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryFunction.Workflows
{
    public interface IGetBrandWorkflow
    {
        Task<Brand> GetBrand(int id);
        Task<List<Brand>> GetBrands();
    }

    public class GetBrandWorkflow : IGetBrandWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetBrandOperations _getBrandOperations;
        private readonly IBrandDataValidator _dataValidator;
        private readonly IBrandDeepValidator _deepValidator;

        public GetBrandWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetBrandWorkflow>();
            _configuration = configuration;
            _getBrandOperations = new GetBrandOperations(loggerFactory, configuration);
            _dataValidator = new BrandDataValidator(loggerFactory, configuration);
            _deepValidator = new BrandDeepValidator(loggerFactory, configuration, _dataValidator);
        }

        public async Task<Brand> GetBrand(int id)
        {
            _logger.LogDebug("GetBrand request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateBrandId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                BrandDto brandDto = await _getBrandOperations.GetBrand(id);
                Brand brand = BrandConverter.ConvertBrandDtoToBrand(brandDto);

                // Respond
                _logger.LogInformation("GetBrand success response.");
                return brand;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[300300003] GetBrand ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[300300004] GetBrand Exception: {e}.");
                throw;
            }
        }

        public async Task<List<Brand>> GetBrands()
        {
            _logger.LogDebug("GetBrands request received.");

            try
            {
                // Validate
                // Nothing to validate

                // Process
                List<BrandDto> brandDtos = await _getBrandOperations.GetBrands(null);
                List<Brand> brands = BrandConverter.ConvertListBrandDtoToListBrand(brandDtos);

                // Respond
                _logger.LogInformation("GetBrands success response.");
                return brands;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[300300005] GetBrands ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[300300006] GetBrands Exception: {e}.");
                throw;
            }
        }
    }
}