using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UserApi.Models.Classes.;
using UserApi.Validators.;
using UserApi.Workflows.Workflows.;

namespace UserApi.Controllers.
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("GetAuthController")]
    [Route("api/v{version:apiVersion}/auth")]
    public class GetAuthController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserControllerValidator _controllerValidator;
        private readonly IGetAuthWorkflow _getAuthWorkflow;

        public GetAuthController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetAuthController>();
            _configuration = configuration;
            _controllerValidator = new UserControllerValidator();
            _getAuthWorkflow = new GetAuthWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getauth")]
        public async Task<IActionResult> GetAuth(int id)
        {
            _logger.LogDebug("GetAuth request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateUserId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                Auth auth = await _getAuthWorkflow.GetAuth(id);

                // Respond
                _logger.LogInformation("GetAuth success response.");
                return Ok(auth);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100100004] GetAuth ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[100100005] GetAuth InvalidOperationException: {ioe}.");
                return NotFound("[100100005] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[100100006] GetAuth Exception: {e}.");
                return Problem("[100100006] " + e.Message);
            }
        }

    }
}
