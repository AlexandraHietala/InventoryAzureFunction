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
    public interface IGetItemWorkflow
    {
        Task<Item> GetItem(int id);
        Task<List<Item>> GetItems(string search);

        Task<List<Item>> GetItemsPerCollection(int collectionId);
    }

    public class GetItemWorkflow : IGetItemWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetItemOperations _getItemOperations;
        private readonly IItemDataValidator _itemDataValidator;
        private readonly ISeriesDataValidator _seriesDataValidator;
        private readonly IBrandDataValidator _brandDataValidator;
        private readonly ICollectionDataValidator _collectionDataValidator;
        private readonly IItemDeepValidator _deepValidator;

        public GetItemWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetItemWorkflow>();
            _configuration = configuration;
            _getItemOperations = new GetItemOperations(loggerFactory, configuration);
            _itemDataValidator = new ItemDataValidator(loggerFactory, configuration);
            _seriesDataValidator = new SeriesDataValidator(loggerFactory, configuration);
            _brandDataValidator = new BrandDataValidator(loggerFactory, configuration);
            _collectionDataValidator = new CollectionDataValidator(loggerFactory, configuration);
            _deepValidator = new ItemDeepValidator(loggerFactory, configuration, _itemDataValidator, _seriesDataValidator, _brandDataValidator, _collectionDataValidator);
        }

        public async Task<Item> GetItem(int id)
        {
            _logger.LogDebug("GetItem request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateItemId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                ItemDto itemDto = await _getItemOperations.GetItem(id);
                Item item = ItemConverter.ConvertItemDtoToItem(itemDto);

                // Respond
                _logger.LogInformation("GetItem success response.");
                return item;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300009] GetItem ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300010] GetItem Exception: {e}.");
                throw;
            }
        }

        public async Task<List<Item>> GetItems(string search)
        {
            _logger.LogDebug("GetItems request received.");

            try
            {
                // Validate
                // Nothing to validate

                // Process
                List<ItemDto> itemDtos = await _getItemOperations.GetItems(search);
                List<Item> items = ItemConverter.ConvertListItemDtoToListItem(itemDtos);

                // Respond
                _logger.LogInformation("GetItems success response.");
                return items;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300011] GetItems ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300012] GetItems Exception: {e}.");
                throw;
            }
        }

        public async Task<List<Item>> GetItemsPerCollection(int collectionId)
        {
            _logger.LogDebug("GetItemsPerCollection request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateCollectionId(collectionId);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                List<ItemDto> itemDtos = await _getItemOperations.GetItemsPerCollection(collectionId);
                List<Item> items = ItemConverter.ConvertListItemDtoToListItem(itemDtos);

                // Respond
                _logger.LogInformation("GetItemsPerCollection success response.");
                return items;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300013] GetItemsPerCollection ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300014] GetItemsPerCollection Exception: {e}.");
                throw;
            }
        }
    }
}