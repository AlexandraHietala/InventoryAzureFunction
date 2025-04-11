using InventoryFunction.Data;
using InventoryFunction.Models.Classes;
using InventoryFunction.Models.Converters;
using InventoryFunction.Models.DTOs;
using InventoryFunction.Validators.DataValidators;
using InventoryFunction.Validators.DeepValidators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InventoryFunction.Workflows
{
    public interface IUpdateBrandWorkflow
    {
        Task UpdateBrand(Brand brand);
    }

    public class UpdateBrandWorkflow : IUpdateBrandWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUpdateBrandOperations _updateBrandOperations;
        private readonly IBrandDataValidator _dataValidator;
        private readonly IBrandDeepValidator _deepValidator;

        public UpdateBrandWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateBrandWorkflow>();
            _configuration = configuration;
            _updateBrandOperations = new UpdateBrandOperations(loggerFactory, configuration);
            _dataValidator = new BrandDataValidator(loggerFactory, configuration);
            _deepValidator = new BrandDeepValidator(loggerFactory, configuration, _dataValidator);
        }

        public async Task UpdateBrand(Brand brand)
        {
            _logger.LogDebug("UpdateBrand request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateUpdateBrand(brand);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                BrandDto brandDto = BrandConverter.ConvertBrandToBrandDto(brand);
                await _updateBrandOperations.UpdateBrand(brandDto);

                // Respond
                _logger.LogInformation("UpdateBrand success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[300300009] UpdateBrand ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[300300010] UpdateBrand Exception: {e}.");
                throw;
            }
        }
    }
}