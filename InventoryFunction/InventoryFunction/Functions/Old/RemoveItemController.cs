using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Models;
using ItemApi.Validators.;
using ItemApi.Workflows.Workflows.;

namespace ItemApi.Controllers.
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("RemoveItemController")]
    [Route("api/v{version:apiVersion}/item")]
    public class RemoveItemController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IItemControllerValidator _controllerValidator;
        private readonly IRemoveItemWorkflow _removeItemWorkflow;

        public RemoveItemController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveItemController>();
            _configuration = configuration;
            _controllerValidator = new ItemControllerValidator();
            _removeItemWorkflow = new RemoveItemWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete]
        [Route("removeitem")]
        public async Task<IActionResult> RemoveItem(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveItem request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateItemId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeItemWorkflow.RemoveItem(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveItem success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100025] RemoveItem ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100026] RemoveItem InvalidOperationException: {ioe}.");
                return NotFound("[200100026] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100027] RemoveItem Exception: {e}.");
                return Problem("[200100027] " + e.Message);
            }
        }
    }
}
