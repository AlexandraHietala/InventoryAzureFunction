using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SeriesApi.Models;
using SeriesApi.Validators.;
using SeriesApi.Workflows.Workflows.;

namespace SeriesApi.Controllers.
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("RemoveSeriesController")]
    [Route("api/v{version:apiVersion}/series")]
    public class RemoveSeriesController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ISeriesControllerValidator _controllerValidator;
        private readonly IRemoveSeriesWorkflow _removeSeriesWorkflow;

        public RemoveSeriesController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveSeriesController>();
            _configuration = configuration;
            _controllerValidator = new SeriesControllerValidator();
            _removeSeriesWorkflow = new RemoveSeriesWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete]
        [Route("removeseries")]
        public async Task<IActionResult> RemoveSeries(int id, string lastmodifiedby)
        {
            _logger.LogDebug("RemoveSeries request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateSeriesId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _removeSeriesWorkflow.RemoveSeries(id, lastmodifiedby);

                // Respond
                _logger.LogInformation("RemoveSeries success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[400100007] RemoveSeries ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[400100008] RemoveSeries InvalidOperationException: {ioe}.");
                return NotFound("[400100008] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[400100009] RemoveSeries Exception: {e}.");
                return Problem("[400100009] " + e.Message);
            }
        }
    }
}
