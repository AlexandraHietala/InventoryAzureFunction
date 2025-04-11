using InventoryFunction.Models.Converters;
using InventoryFunction.Models.Classes;
using InventoryFunction.Models.DTOs;
using InventoryFunction.Validators.DataValidators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using InventoryFunction.Validators.DeepValidators;
using InventoryFunction.Data;

namespace InventoryFunction.Workflows
{
    public interface IAddCollectionWorkflow
    {
        Task<int> AddCollection(Collection collection);
    }

    public class AddCollectionWorkflow : IAddCollectionWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddCollectionOperations _addCollectionOperations;
        private readonly ICollectionDataValidator _dataValidator;
        private readonly ICollectionDeepValidator _deepValidator;


        public AddCollectionWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddCollectionWorkflow>();
            _configuration = configuration;
            _addCollectionOperations = new AddCollectionOperations(loggerFactory, configuration);
            _dataValidator = new CollectionDataValidator(loggerFactory, configuration);
            _deepValidator = new CollectionDeepValidator(loggerFactory, configuration, _dataValidator);
        }

        public async Task<int> AddCollection(Collection collection)
        {
            _logger.LogDebug("AddCollection request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateAddCollection(collection);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                CollectionDto collectionDto = CollectionConverter.ConvertCollectionToCollectionDto(collection);
                int newCollectionId = await _addCollectionOperations.AddCollection(collectionDto);

                // Respond
                _logger.LogInformation("AddCollection success response.");
                return newCollectionId;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[500300001] AddCollection ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[500300002] AddCollection Exception: {e}.");
                throw;
            }
        }
    }
}