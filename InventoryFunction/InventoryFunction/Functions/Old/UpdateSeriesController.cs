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
    [ControllerName("UpdateSeriesController")]
    [Route("api/v{version:apiVersion}/series")]
    public class UpdateSeriesController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ISeriesControllerValidator _controllerValidator;
        private readonly IUpdateSeriesWorkflow _updateSeriesWorkflow;

        public UpdateSeriesController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateSeriesController>();
            _configuration = configuration;
            _controllerValidator = new SeriesControllerValidator();
            _updateSeriesWorkflow = new UpdateSeriesWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPut]
        [Route("updateseries")]
        public async Task<IActionResult> UpdateSeries(int id, string seriesName, string? description, string lastmodifiedby)
        {
            _logger.LogDebug("UpdateSeries request received.");

            try
            {
                // Validate
                Series item = new Series()
                {
                    Id = id,
                    SeriesName = seriesName,
                    Description = description,
                    CreatedBy = lastmodifiedby,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = lastmodifiedby,
                    LastModifiedDate = DateTime.Now
                };

                var failures = _controllerValidator.ValidateUpdateSeries(item);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _updateSeriesWorkflow.UpdateSeries(item);

                // Respond
                _logger.LogInformation("UpdateSeries success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[400100010] UpdateSeries ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[400100011] UpdateSeries InvalidOperationException: {ioe}.");
                return NotFound("[400100011] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[400100012] UpdateSeries Exception: {e}.");
                return Problem("[400100012] " + e.Message);
            }
        }
    }
}
