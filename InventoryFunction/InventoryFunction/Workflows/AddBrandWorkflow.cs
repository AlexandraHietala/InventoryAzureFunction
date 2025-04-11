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
    public interface IAddBrandWorkflow
    {
        Task<int> AddBrand(Brand brand);
    }

    public class AddBrandWorkflow : IAddBrandWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddBrandOperations _addBrandOperations;
        private readonly IBrandDataValidator _dataValidator;
        private readonly IBrandDeepValidator _deepValidator;


        public AddBrandWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddBrandWorkflow>();
            _configuration = configuration;
            _addBrandOperations = new AddBrandOperations(loggerFactory, configuration);
            _dataValidator = new BrandDataValidator(loggerFactory, configuration);
            _deepValidator = new BrandDeepValidator(loggerFactory, configuration, _dataValidator);
        }

        public async Task<int> AddBrand(Brand brand)
        {
            _logger.LogDebug("AddBrand request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateAddBrand(brand);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                BrandDto brandDto = BrandConverter.ConvertBrandToBrandDto(brand);
                int newBrandId = await _addBrandOperations.AddBrand(brandDto);

                // Respond
                _logger.LogInformation("AddBrand success response.");
                return newBrandId;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[300300001] AddBrand ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[300300002] AddBrand Exception: {e}.");
                throw;
            }
        }
    }
}