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
    public interface IUpdateItemWorkflow
    {
        Task UpdateItem(Item item);
    }

    public class UpdateItemWorkflow : IUpdateItemWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUpdateItemOperations _updateItemOperations;
        private readonly IItemDataValidator _itemDataValidator;
        private readonly ISeriesDataValidator _seriesDataValidator;
        private readonly IBrandDataValidator _brandDataValidator;
        private readonly ICollectionDataValidator _collectionDataValidator;
        private readonly IItemDeepValidator _deepValidator;

        public UpdateItemWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateItemWorkflow>();
            _configuration = configuration;
            _updateItemOperations = new UpdateItemOperations(loggerFactory, configuration);
            _itemDataValidator = new ItemDataValidator(loggerFactory, configuration);
            _seriesDataValidator = new SeriesDataValidator(loggerFactory, configuration);
            _brandDataValidator = new BrandDataValidator(loggerFactory, configuration);
            _collectionDataValidator =  new CollectionDataValidator(loggerFactory, configuration);
            _deepValidator = new ItemDeepValidator(loggerFactory, configuration, _itemDataValidator, _seriesDataValidator, _brandDataValidator, _collectionDataValidator);
        }

        public async Task UpdateItem(Item item)
        {
            _logger.LogDebug("UpdateItem request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateUpdateItem(item);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                ItemDto itemDto = ItemConverter.ConvertItemToItemDto(item);
                await _updateItemOperations.UpdateItem(itemDto);

                // Respond
                _logger.LogInformation("UpdateItem success response.");
                return;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300021] UpdateItem ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300022] UpdateItem Exception: {e}.");
                throw;
            }
        }
    }
}