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
    [ControllerName("AddUserController")]
    [Route("api/v{version:apiVersion}/user")]
    public class AddUserController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserControllerValidator _controllerValidator;
        private readonly IAddUserWorkflow _addUserWorkflow;

        public AddUserController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddUserController>();
            _configuration = configuration;
            _controllerValidator = new UserControllerValidator();
            _addUserWorkflow = new AddUserWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        [Route("adduser")]
        public async Task<IActionResult> AddUser(string name, string salt, string hash, int? roleId, string lastmodifiedby)
        {
            _logger.LogDebug("AddUser request received.");

            try
            {
                // Validate
                User user = new User()
                {
                    Name = name,
                    PassSalt = salt,
                    PassHash = hash,
                    RoleId = roleId,
                    LastModifiedBy = lastmodifiedby,
                    LastModifiedDate = DateTime.Now,
                    CreatedBy = lastmodifiedby,
                    CreatedDate = DateTime.Now
                };

                var failures = _controllerValidator.ValidateAdd(user);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                int id = await _addUserWorkflow.AddUser(user);

                // Respond
                _logger.LogInformation("AddUser success response.");
                return Ok(id);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[100100001] AddUser ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[100100002] AddUser InvalidOperationException: {ioe}.");
                return NotFound("[100100002] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[100100003] AddUser Exception: {e}.");
                return Problem("[100100003] " + e.Message);
            }
        }
    }
}
