using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using Asp.Versioning;
using CollectionApi.Validators.;
using CollectionApi.Workflows.Workflows.;
using CollectionApi.Models.Classes.;

namespace CollectionApi.Controllers.
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("GetCollectionController")]
    [Route("api/v{version:apiVersion}/collection")]
    public class GetCollectionController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ICollectionControllerValidator _controllerValidator;
        private readonly IGetCollectionWorkflow _getCollectionWorkflow;

        public GetCollectionController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetCollectionController>();
            _configuration = configuration;
            _controllerValidator = new CollectionControllerValidator();
            _getCollectionWorkflow = new GetCollectionWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getcollection")]
        public async Task<IActionResult> GetCollection(int id)
        {
            _logger.LogDebug("GetCollection request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateCollectionId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                Collection collection = await _getCollectionWorkflow.GetCollection(id);

                // Respond
                _logger.LogInformation("GetCollection success response.");
                return Ok(collection);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[500100004] GetCollection ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[500100005] GetCollection InvalidOperationException: {ioe}.");
                return NotFound("[500100005] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[500100006] GetCollection Exception: {e}.");
                return Problem("[500100006] " + e.Message);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getcollections")]
        public async Task<IActionResult> GetCollections()
        {
            _logger.LogDebug("GetCollections request received.");

            try
            {
                // Validate
                // Nothing to validate

                // Process
                List<Collection> collections = await _getCollectionWorkflow.GetCollections();

                // Respond
                _logger.LogInformation("GetCollections success response.");
                return Ok(collections);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[500100007] GetCollections ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[500100008] GetCollections InvalidOperationException: {ioe}.");
                return NotFound("[500100008] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[500100009] GetCollections Exception: {e}.");
                return Problem("[500100009] " + e.Message);
            }
        }
    }
}
