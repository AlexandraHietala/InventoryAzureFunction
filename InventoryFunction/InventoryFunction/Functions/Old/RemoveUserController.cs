using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UserApi.Models;
using UserApi.Validators.;
using UserApi.Workflows.Workflows.;

namespace UserApi.Controllers.
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("RemoveUserController")]
    [Route("api/v{version:apiVersion}/user")]
    public class RemoveUserController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserControllerValidator _controllerValidator;
        private readonly IRemoveUserWorkflow _removeUserWorkflow;

        public RemoveUserController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveUserController>();
            _configuration = configuration;
            _controllerValidator = new UserControllerValidator();
            _removeUserWorkflow = new RemoveUserWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete]
        [Route("removeuser")]
        public async Task<IActionResult> RemoveUser(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveUser request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateUserId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeUserWorkflow.RemoveUser(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveUser success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100100019] RemoveUser ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[100100020] RemoveUser InvalidOperationException: {ioe}.");
                return NotFound("[100100020] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[100100021] RemoveUser Exception: {e}.");
                return Problem("[100100021] " + e.Message);
            }
        }
    }
}
