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
    public interface IGetCollectionWorkflow
    {
        Task<Collection> GetCollection(int id);
        Task<List<Collection>> GetCollections(string search);
    }

    public class GetCollectionWorkflow : IGetCollectionWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetCollectionOperations _getCollectionOperations;
        private readonly ICollectionDataValidator _dataValidator;
        private readonly ICollectionDeepValidator _deepValidator;

        public GetCollectionWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetCollectionWorkflow>();
            _configuration = configuration;
            _getCollectionOperations = new GetCollectionOperations(loggerFactory, configuration);
            _dataValidator = new CollectionDataValidator(loggerFactory, configuration);
            _deepValidator = new CollectionDeepValidator(loggerFactory, configuration, _dataValidator);
        }

        public async Task<Collection> GetCollection(int id)
        {
            _logger.LogDebug("GetCollection request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateCollectionId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                CollectionDto collectionDto = await _getCollectionOperations.GetCollection(id);
                Collection collection = CollectionConverter.ConvertCollectionDtoToCollection(collectionDto);

                // Respond
                _logger.LogInformation("GetCollection success response.");
                return collection;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[500300003] GetCollection ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[500300004] GetCollection Exception: {e}.");
                throw;
            }
        }

        public async Task<List<Collection>> GetCollections(string search)
        {
            _logger.LogDebug("GetCollections request received.");

            try
            {
                // Validate
                // Nothing to validate

                // Process
                List<CollectionDto> collectionDtos = await _getCollectionOperations.GetCollections(search);
                List<Collection> collections = CollectionConverter.ConvertListCollectionDtoToListCollection(collectionDtos);

                // Respond
                _logger.LogInformation("GetCollections success response.");
                return collections;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[500300005] GetCollections ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[500300006] GetCollections Exception: {e}.");
                throw;
            }
        }
    }
}