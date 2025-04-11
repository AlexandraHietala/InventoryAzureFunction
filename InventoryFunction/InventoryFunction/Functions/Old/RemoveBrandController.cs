using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BrandApi.Models;
using BrandApi.Validators.;
using BrandApi.Workflows.Workflows.;

namespace BrandApi.Controllers.
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("RemoveBrandController")]
    [Route("api/v{version:apiVersion}/brand")]
    public class RemoveBrandController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IBrandControllerValidator _controllerValidator;
        private readonly IRemoveBrandWorkflow _removeBrandWorkflow;

        public RemoveBrandController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveBrandController>();
            _configuration = configuration;
            _controllerValidator = new BrandControllerValidator();
            _removeBrandWorkflow = new RemoveBrandWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete]
        [Route("removebrand")]
        public async Task<IActionResult> RemoveBrand(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveBrand request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateBrandId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeBrandWorkflow.RemoveBrand(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveBrand success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[300100010] RemoveBrand ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[300100011] RemoveBrand InvalidOperationException: {ioe}.");
                return NotFound("[300100011] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[300100012] RemoveBrand Exception: {e}.");
                return Problem("[300100012] " + e.Message);
            }
        }
    }
}
