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
    [ControllerName("GetRoleController")]
    [Route("api/v{version:apiVersion}/role")]
    public class GetRoleController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserControllerValidator _controllerValidator;
        private readonly IGetRoleWorkflow _getRoleWorkflow;

        public GetRoleController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetRoleController>();
            _configuration = configuration;
            _controllerValidator = new UserControllerValidator();
            _getRoleWorkflow = new GetRoleWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getrole")]
        public async Task<IActionResult> GetRole(int id)
        {
            _logger.LogDebug("GetRole request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateRoleId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                Role requestedRole = await _getRoleWorkflow.GetRole(id);

                // Respond
                _logger.LogInformation("GetRole success response.");
                return Ok(requestedRole);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100100007] GetRole ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[100100008] GetRole InvalidOperationException: {ioe}.");
                return NotFound("[100100008] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[100100009] GetRole Exception: {e}.");
                return Problem("[100100009] " + e.Message);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getroles")]
        public async Task<IActionResult> GetRoles()
        {
            _logger.LogDebug("GetRoles request received.");

            try
            {
                // Validate
                // Nothing to Validate

                // Process
                List<Role> requestedRoles = await _getRoleWorkflow.GetRoles();

                // Respond
                _logger.LogInformation("GetRoles success response.");
                return Ok(requestedRoles);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100100010] GetRoles ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[100100011] GetRoles InvalidOperationException: {ioe}.");
                return NotFound("[100100011] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[100100012] GetRoles Exception: {e}.");
                return Problem("[100100012] " + e.Message);
            }
        }
    }
}
