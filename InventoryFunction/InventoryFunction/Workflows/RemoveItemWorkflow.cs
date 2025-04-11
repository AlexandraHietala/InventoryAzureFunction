using InventoryFunction.Data;
using InventoryFunction.Validators.DataValidators;
using InventoryFunction.Validators.DeepValidators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InventoryFunction.Workflows
{
    public interface IRemoveItemWorkflow
    {
        Task RemoveItem(int id, string lastmodifiedby);
    }

    public class RemoveItemWorkflow : IRemoveItemWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IRemoveItemOperations _removeItemOperations;
        private readonly IItemDataValidator _itemDataValidator;
        private readonly ISeriesDataValidator _seriesDataValidator;
        private readonly IBrandDataValidator _brandDataValidator;
        private readonly ICollectionDataValidator _collectionDataValidator;
        private readonly IItemDeepValidator _deepValidator;

        public RemoveItemWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveItemWorkflow>();
            _configuration = configuration;
            _removeItemOperations = new RemoveItemOperations(loggerFactory, configuration);
            _itemDataValidator = new ItemDataValidator(loggerFactory, configuration);
            _seriesDataValidator = new SeriesDataValidator(loggerFactory, configuration);
            _brandDataValidator = new BrandDataValidator(loggerFactory, configuration);
            _collectionDataValidator = new CollectionDataValidator(loggerFactory, configuration);
            _deepValidator = new ItemDeepValidator(loggerFactory, configuration, _itemDataValidator, _seriesDataValidator, _brandDataValidator, _collectionDataValidator);
        }

        public async Task RemoveItem(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveItem request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateItemId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeItemOperations.RemoveItem(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveItem success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300017] RemoveItem ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300018] RemoveItem Exception: {e}.");
                throw;
            }
        }
    }
}