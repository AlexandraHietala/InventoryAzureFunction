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
    public interface IUpdateCollectionWorkflow
    {
        Task UpdateCollection(Collection collection);
    }

    public class UpdateCollectionWorkflow : IUpdateCollectionWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUpdateCollectionOperations _updateCollectionOperations;
        private readonly ICollectionDataValidator _dataValidator;
        private readonly ICollectionDeepValidator _deepValidator;

        public UpdateCollectionWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateCollectionWorkflow>();
            _configuration = configuration;
            _updateCollectionOperations = new UpdateCollectionOperations(loggerFactory, configuration);
            _dataValidator = new CollectionDataValidator(loggerFactory, configuration);
            _deepValidator = new CollectionDeepValidator(loggerFactory, configuration, _dataValidator);
        }

        public async Task UpdateCollection(Collection collection)
        {
            _logger.LogDebug("UpdateCollection request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateUpdateCollection(collection);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                CollectionDto collectionDto = CollectionConverter.ConvertCollectionToCollectionDto(collection);
                await _updateCollectionOperations.UpdateCollection(collectionDto);

                // Respond
                _logger.LogInformation("UpdateCollection success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[500300009] UpdateCollection ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[500300010] UpdateCollection Exception: {e}.");
                throw;
            }
        }
    }
}