using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using Asp.Versioning;
using ItemApi.Validators.;
using ItemApi.Workflows.Workflows.;
using ItemApi.Models.Classes.;

namespace ItemApi.Controllers.
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("GetItemController")]
    [Route("api/v{version:apiVersion}/item")]
    public class GetItemController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IItemControllerValidator _controllerValidator;
        private readonly IGetItemWorkflow _getItemWorkflow;

        public GetItemController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetItemController>();
            _configuration = configuration;
            _controllerValidator = new ItemControllerValidator();
            _getItemWorkflow = new GetItemWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getitem")]
        public async Task<IActionResult> GetItem(int id)
        {
            _logger.LogDebug("GetItem request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateItemId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                Item requestedItem = await _getItemWorkflow.GetItem(id);

                // Respond
                _logger.LogInformation("GetItem success response.");
                return Ok(requestedItem);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100013] GetItem ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100014] GetItem InvalidOperationException: {ioe}.");
                return NotFound("[200100014] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100015] GetItem Exception: {e}.");
                return Problem("[200100015] " + e.Message);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getitems")]
        public async Task<IActionResult> GetItems()
        {
            _logger.LogDebug("GetItems request received.");

            try
            {
                // Validate
                // Nothing to validate

                // Process
                List<Item> requestedItems = await _getItemWorkflow.GetItems();

                // Respond
                _logger.LogInformation("GetItems success response.");
                return Ok(requestedItems);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100016] GetItems ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100017] GetItems InvalidOperationException: {ioe}.");
                return NotFound("[200100017] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100018] GetItems Exception: {e}.");
                return Problem("[200100018] " + e.Message);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getitemspercollection")]
        public async Task<IActionResult> GetItemsPerCollection(int collectionId)
        {
            _logger.LogDebug("GetItemsPerCollection request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateCollectionId(collectionId);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                List<Item> requestedItems = await _getItemWorkflow.GetItemsPerCollection(collectionId);

                // Respond
                _logger.LogInformation("GetItemsPerCollection success response.");
                return Ok(requestedItems);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100019] GetItemsPerCollection ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100020] GetItemsPerCollection InvalidOperationException: {ioe}.");
                return NotFound("[200100020] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100021] GetItemsPerCollection Exception: {e}.");
                return Problem("[200100021] " + e.Message);
            }
        }


        
    }
}
