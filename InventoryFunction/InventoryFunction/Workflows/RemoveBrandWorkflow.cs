using InventoryFunction.Data;
using InventoryFunction.Validators.DataValidators;
using InventoryFunction.Validators.DeepValidators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InventoryFunction.Workflows
{
    public interface IRemoveBrandWorkflow
    {
        Task RemoveBrand(int id, string lastmodifiedby);
    }

    public class RemoveBrandWorkflow : IRemoveBrandWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IRemoveBrandOperations _removeBrandOperations;
        private readonly IBrandDataValidator _dataValidator;
        private readonly IBrandDeepValidator _deepValidator;

        public RemoveBrandWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveBrandWorkflow>();
            _configuration = configuration;
            _removeBrandOperations = new RemoveBrandOperations(loggerFactory, configuration);
            _dataValidator = new BrandDataValidator(loggerFactory, configuration);
            _deepValidator = new BrandDeepValidator(loggerFactory, configuration, _dataValidator);
        }

        public async Task RemoveBrand(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveBrand request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateBrandId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeBrandOperations.RemoveBrand(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveBrand success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[300300007] RemoveBrand ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[300300008] RemoveBrand Exception: {e}.");
                throw;
            }
        }
    }
}