using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CollectionApi.Models;
using CollectionApi.Validators.;
using CollectionApi.Workflows.Workflows.;

namespace CollectionApi.Controllers.
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("RemoveCollectionController")]
    [Route("api/v{version:apiVersion}/collection")]
    public class RemoveCollectionController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ICollectionControllerValidator _controllerValidator;
        private readonly IRemoveCollectionWorkflow _removeCollectionWorkflow;

        public RemoveCollectionController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveCollectionController>();
            _configuration = configuration;
            _controllerValidator = new CollectionControllerValidator();
            _removeCollectionWorkflow = new RemoveCollectionWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete]
        [Route("removecollection")]
        public async Task<IActionResult> RemoveCollection(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveCollection request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateCollectionId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeCollectionWorkflow.RemoveCollection(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveCollection success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[500100010] RemoveCollection ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[500100011] RemoveCollection InvalidOperationException: {ioe}.");
                return NotFound("[500100011] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[500100012] RemoveCollection Exception: {e}.");
                return Problem("[500100012] " + e.Message);
            }
        }
    }
}
