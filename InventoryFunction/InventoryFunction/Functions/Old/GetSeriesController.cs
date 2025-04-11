using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using Asp.Versioning;
using SeriesApi.Validators.;
using SeriesApi.Workflows.Workflows.;
using SeriesApi.Models.Classes.;

namespace SeriesApi.Controllers.
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("GetSeriesController")]
    [Route("api/v{version:apiVersion}/series")]
    public class GetSeriesController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ISeriesControllerValidator _controllerValidator;
        private readonly IGetSeriesWorkflow _getSeriesWorkflow;

        public GetSeriesController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetSeriesController>();
            _configuration = configuration;
            _controllerValidator = new SeriesControllerValidator();
            _getSeriesWorkflow = new GetSeriesWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getaseries")]
        public async Task<IActionResult> GetASeries(int id)
        {
            _logger.LogDebug("GetSeries request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateSeriesId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                Series requestedSeries = await _getSeriesWorkflow.GetASeries(id);

                // Respond
                _logger.LogInformation("GetSeries success response.");
                return Ok(requestedSeries);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[400100004] GetSeries ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[400100005] GetSeries InvalidOperationException: {ioe}.");
                return NotFound("[400100005] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[400100006] GetSeries Exception: {e}.");
                return Problem("[400100006] " + e.Message);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getseries")]
        public async Task<IActionResult> GetSeries()
        {
            _logger.LogDebug("GetSeries request received.");

            try
            {
                // Validate
                // Nothing to validate

                // Process
                List<Series> requestedSeriess = await _getSeriesWorkflow.GetSeries();

                // Respond
                _logger.LogInformation("GetSeries success response.");
                return Ok(requestedSeriess);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100034] GetSeries ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100035] GetSeries InvalidOperationException: {ioe}.");
                return NotFound("[200100035] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100036] GetSeries Exception: {e}.");
                return Problem("[200100036] " + e.Message);
            }
        }
    }
}
