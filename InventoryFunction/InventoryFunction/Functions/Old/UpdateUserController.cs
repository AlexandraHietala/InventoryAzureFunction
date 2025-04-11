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
    [ControllerName("UpdateUserController")]
    [Route("api/v{version:apiVersion}/user")]
    public class UpdateUserController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserControllerValidator _controllerValidator;
        private readonly IUpdateUserWorkflow _updateUserWorkflow;

        public UpdateUserController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateUserController>();
            _configuration = configuration;
            _controllerValidator = new UserControllerValidator();
            _updateUserWorkflow = new UpdateUserWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPut]
        [Route("updateuser")]
        public async Task<IActionResult> UpdateUser(int id, string name, string salt, string hash, int? roleId, string lastmodifiedby)
        {
            _logger.LogDebug("UpdateUser request received.");

            try
            {
                // Validate
                User user = new User()
                {
                    Id = id,
                    Name = name,
                    PassSalt = salt,
                    PassHash = hash,
                    RoleId = roleId,
                    LastModifiedBy = lastmodifiedby,
                    LastModifiedDate = DateTime.Now,
                    CreatedBy = lastmodifiedby,
                    CreatedDate = DateTime.Now
                };

                var failures = _controllerValidator.ValidateUpdate(user);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _updateUserWorkflow.UpdateUser(user);

                // Respond
                _logger.LogInformation("UpdateUser success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100100022] UpdateUser ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[100100023] UpdateUser InvalidOperationException: {ioe}.");
                return NotFound("[100100023] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[100100024] UpdateUser Exception: {e}.");
                return Problem("[100100024] " + e.Message);
            }
        }
    }
}
