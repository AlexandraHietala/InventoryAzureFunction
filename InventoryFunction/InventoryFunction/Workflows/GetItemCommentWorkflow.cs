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
    public interface IGetItemCommentWorkflow
    {
        Task<ItemComment> GetItemComment(int id);
        Task<List<ItemComment>> GetItemComments(int itemId);
    }

    public class GetItemCommentWorkflow : IGetItemCommentWorkflow
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetItemCommentOperations _getItemCommentOperations;
        private readonly IItemDataValidator _itemDataValidator;
        private readonly IItemCommentDataValidator _itemCommentDataValidator;
        private readonly IItemCommentDeepValidator _deepValidator;

        public GetItemCommentWorkflow(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetItemCommentWorkflow>();
            _configuration = configuration;
            _getItemCommentOperations = new GetItemCommentOperations(loggerFactory, configuration);
            _itemDataValidator = new ItemDataValidator(loggerFactory, configuration);
            _itemCommentDataValidator = new ItemCommentDataValidator(loggerFactory, configuration);
            _deepValidator = new ItemCommentDeepValidator(loggerFactory, configuration, _itemDataValidator, _itemCommentDataValidator);
        }

        public async Task<ItemComment> GetItemComment(int id)
        {
            _logger.LogDebug("GetItemComment request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateItemCommentId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                ItemCommentDto commentDto = await _getItemCommentOperations.GetItemComment(id);
                ItemComment comment = ItemCommentConverter.ConvertItemCommentDtoToItemComment(commentDto);

                // Respond
                _logger.LogInformation("GetItemComment success response.");
                return comment;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300005] GetItemComment ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300006] GetItemComment Exception: {e}.");
                throw;
            }
        }

        public async Task<List<ItemComment>> GetItemComments(int itemId)
        {
            _logger.LogDebug("GetItemComments request received.");

            try
            {
                // Validate
                var failures = await _deepValidator.ValidateItemId(itemId);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                List<ItemCommentDto> commentDtos = await _getItemCommentOperations.GetItemComments(itemId);
                List<ItemComment> comments = ItemCommentConverter.ConvertListItemCommentDtoToListItemComment(commentDtos);

                // Respond
                _logger.LogInformation("GetItemComments success response.");
                return comments;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200300007] GetItemComments ArgumentException: {ae}.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError($"[200300008] GetItemComments Exception: {e}.");
                throw;
            }
        }
    }
}