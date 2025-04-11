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
    [ControllerName("RemoveItemCommentController")]
    [Route("api/v{version:apiVersion}/comment")]
    public class RemoveItemCommentController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IItemCommentControllerValidator _controllerValidator;
        private readonly IRemoveItemCommentWorkflow _removeItemCommentWorkflow;

        public RemoveItemCommentController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveItemCommentController>();
            _configuration = configuration;
            _controllerValidator = new ItemCommentControllerValidator();
            _removeItemCommentWorkflow = new RemoveItemCommentWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete]
        [Route("removeitemcomment")]
        public async Task<IActionResult> RemoveItemComment(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveItemComment request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateItemCommentId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeItemCommentWorkflow.RemoveItemComment(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveItemComment success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100022] RemoveItemComment ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100023] RemoveItemComment InvalidOperationException: {ioe}.");
                return NotFound("[200100023] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100024] RemoveItemComment Exception: {e}.");
                return Problem("[200100024] " + e.Message);
            }
        }
    }
}
