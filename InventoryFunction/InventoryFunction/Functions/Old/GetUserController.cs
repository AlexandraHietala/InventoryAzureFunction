using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using Asp.Versioning;
using UserApi.Validators.;
using UserApi.Workflows.Workflows.;
using UserApi.Models.Classes.;

namespace UserApi.Controllers.
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("GetUserController")]
    [Route("api/v{version:apiVersion}/user")]
    public class GetUserController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserControllerValidator _controllerValidator;
        private readonly IGetUserWorkflow _getUserWorkflow;

        public GetUserController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetUserController>();
            _configuration = configuration;
            _controllerValidator = new UserControllerValidator();
            _getUserWorkflow = new GetUserWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getuser")]
        public async Task<IActionResult> GetUser(int id)
        {
            _logger.LogDebug("GetUser request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateUserId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                User requestedUser = await _getUserWorkflow.GetUser(id);

                // Respond
                _logger.LogInformation("GetUser success response.");
                return Ok(requestedUser);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100100013] GetUser ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[100100014] GetUser InvalidOperationException: {ioe}.");
                return NotFound("[100100014] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[100100015] GetUser Exception: {e}.");
                return Problem("[100100015] " + e.Message);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getusers")]
        public async Task<IActionResult> GetUsers()
        {
            _logger.LogDebug("GetUsers request received.");

            try
            {
                // Validate
                // Nothing to validate

                // Process
                List<User> requestedUsers = await _getUserWorkflow.GetUsers();

                // Respond
                _logger.LogInformation("GetUsers success response.");
                return Ok(requestedUsers);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100100016] GetUsers ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[100100017] GetUsers InvalidOperationException: {ioe}.");
                return NotFound("[100100017] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[100100018] GetUsers Exception: {e}.");
                return Problem("[100100018] " + e.Message);
            }
        }
    }
}
