using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SeriesApi.Models.Classes.;
using SeriesApi.Validators.;
using SeriesApi.Workflows.Workflows.;

namespace SeriesApi.Controllers.
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("AddSeriesController")]
    [Route("api/v{version:apiVersion}/series")]
    public class AddSeriesController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ISeriesControllerValidator _controllerValidator;
        private readonly IAddSeriesWorkflow _addSeriesWorkflow;

        public AddSeriesController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddSeriesController>();
            _configuration = configuration;
            _controllerValidator = new SeriesControllerValidator();
            _addSeriesWorkflow = new AddSeriesWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        [Route("addseries")]
        public async Task<IActionResult> AddSeries(string seriesName, string? description, string lastmodifiedby)
        {
            _logger.LogDebug("AddSeries request received.");

            try
            {
                // Validate
                Series series = new Series()
                {
                    Id = 0,
                    SeriesName = seriesName,
                    Description = description,
                    CreatedBy = lastmodifiedby,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = lastmodifiedby,
                    LastModifiedDate = DateTime.Now
                };

                var failures = _controllerValidator.ValidateAddSeries(series);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                int id = await _addSeriesWorkflow.AddSeries(series);

                // Respond
                _logger.LogInformation("AddSeries success response.");
                return Ok(id);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[400100001] AddSeries ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[400100002] AddSeries InvalidOperationException: {ioe}.");
                return NotFound("[400100002] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[400100003] AddSeries Exception: {e}.");
                return Problem("[400100003] " + e.Message);
            }
        }
    }
}
