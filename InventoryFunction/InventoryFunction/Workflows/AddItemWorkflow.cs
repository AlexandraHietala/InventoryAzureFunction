using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using InventoryFunction.Data;
using InventoryFunction.Models.Classes;
using InventoryFunction.Models.Converters;
using InventoryFunction.Models.DTOs;
using InventoryFunction.Validators.DataValidators;
using InventoryFunction.Validators.DeepValidators;
using System;
using System.Threading.Tasks;

namespace InventoryFunction.Workflows
{
    public interface IAddItemWorkflow
    {
        Task<int> AddItem(Item item);
    }

    public class AddItemWorkflow : IAddItemWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddItemOperations _addItemOperations;
        private readonly IItemDataValidator _itemDataValidator;
        private readonly ISeriesDataValidator _seriesDataValidator;
        private readonly IBrandDataValidator _brandDataValidator;
        private readonly ICollectionDataValidator _collectionDataValidator;
        private readonly IItemDeepValidator _itemDeepValidator;


        public AddItemWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddItemWorkflow>();
            _configuration = configuration;
            _addItemOperations = new AddItemOperations(loggerFactory, configuration);
            _itemDataValidator = new ItemDataValidator(loggerFactory, configuration);
            _seriesDataValidator = new SeriesDataValidator(loggerFactory, configuration);
            _brandDataValidator = new BrandDataValidator(loggerFactory, configuration);
            _collectionDataValidator = new CollectionDataValidator(loggerFactory, configuration);
            _itemDeepValidator = new ItemDeepValidator(loggerFactory, configuration, _itemDataValidator, _seriesDataValidator, _brandDataValidator, _collectionDataValidator);
        }
        
        public async Task<int> AddItem(Item item)
        {
            _logger.LogDebug("AddItem request received.");

            try
            {
                // Validate
                var failures = await _itemDeepValidator.ValidateAddItem(item);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                ItemDto itemDto = ItemConverter.ConvertItemToItemDto(item);
                int newItemId = await _addItemOperations.AddItem(itemDto);

                // Respond
                _logger.LogInformation("AddItem success response.");
                return newItemId;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300003] AddItem ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300004] AddItem Exception: {e}.");
                throw;
            }
        }
    }
}