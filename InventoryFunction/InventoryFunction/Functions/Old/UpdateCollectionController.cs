using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CollectionApi.Models.Classes.;
using CollectionApi.Validators.;
using CollectionApi.Workflows.Workflows.;

namespace CollectionApi.Controllers.
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("UpdateCollectionController")]
    [Route("api/v{version:apiVersion}/collection")]
    public class UpdateCollectionController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ICollectionControllerValidator _controllerValidator;
        private readonly IUpdateCollectionWorkflow _updateCollectionWorkflow;

        public UpdateCollectionController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateCollectionController>();
            _configuration = configuration;
            _controllerValidator = new CollectionControllerValidator();
            _updateCollectionWorkflow = new UpdateCollectionWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPut]
        [Route("updatecollection")]
        public async Task<IActionResult> UpdateCollection(int id, string collectionName, string? description, string lastmodifiedby)
        {
            _logger.LogDebug("UpdateCollection request received.");

            try
            {
                // Validate
                Collection item = new Collection()
                {
                    Id = id,
                    CollectionName = collectionName,
                    Description = description,
                    CreatedBy = lastmodifiedby,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = lastmodifiedby,
                    LastModifiedDate = DateTime.Now
                };

                var failures = _controllerValidator.ValidateUpdateCollection(item);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _updateCollectionWorkflow.UpdateCollection(item);

                // Respond
                _logger.LogInformation("UpdateCollection success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[500100013] UpdateCollection ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[500100014] UpdateCollection InvalidOperationException: {ioe}.");
                return NotFound("[500100014] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[500100015] UpdateCollection Exception: {e}.");
                return Problem("[500100015] " + e.Message);
            }
        }
    }
}
