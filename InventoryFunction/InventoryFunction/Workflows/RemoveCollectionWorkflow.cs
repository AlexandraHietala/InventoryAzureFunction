using InventoryFunction.Data;
using InventoryFunction.Validators.DataValidators;
using InventoryFunction.Validators.DeepValidators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InventoryFunction.Workflows
{
    public interface IRemoveCollectionWorkflow
    {
        Task RemoveCollection(int id, string lastmodifiedby);
    }

    public class RemoveCollectionWorkflow : IRemoveCollectionWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IRemoveCollectionOperations _removeCollectionOperations;
        private readonly ICollectionDataValidator _dataValidator;
        private readonly ICollectionDeepValidator _deepValidator;

        public RemoveCollectionWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveCollectionWorkflow>();
            _configuration = configuration;
            _removeCollectionOperations = new RemoveCollectionOperations(loggerFactory, configuration);
            _dataValidator = new CollectionDataValidator(loggerFactory, configuration);
            _deepValidator = new CollectionDeepValidator(loggerFactory, configuration, _dataValidator);
        }

        public async Task RemoveCollection(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveCollection request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateCollectionId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeCollectionOperations.RemoveCollection(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveCollection success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[500300007] RemoveCollection ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[500300008] RemoveCollection Exception: {e}.");
                throw;
            }
        }
    }
}