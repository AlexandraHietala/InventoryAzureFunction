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
    [ControllerName("AddCollectionController")]
    [Route("api/v{version:apiVersion}/collection")]
    public class AddCollectionController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ICollectionControllerValidator _controllerValidator;
        private readonly IAddCollectionWorkflow _addCollectionWorkflow;

        public AddCollectionController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddCollectionController>();
            _configuration = configuration;
            _controllerValidator = new CollectionControllerValidator();
            _addCollectionWorkflow = new AddCollectionWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        [Route("addcollection")]
        public async Task<IActionResult> AddCollection(string collectionName, string? description, string lastmodifiedby)
        {
            _logger.LogDebug("AddCollection request received.");

            try
            {
                // Validate
                Collection collection = new Collection()
                {
                    Id = 0,
                    CollectionName = collectionName,
                    Description = description,
                    CreatedBy = lastmodifiedby,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = lastmodifiedby,
                    LastModifiedDate = DateTime.Now
                };

                var failures = _controllerValidator.ValidateAddCollection(collection);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                int id = await _addCollectionWorkflow.AddCollection(collection);

                // Respond
                _logger.LogInformation("AddCollection success response.");
                return Ok(id);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[500100001] AddCollection ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[500100002] AddCollection InvalidOperationException: {ioe}.");
                return NotFound("[500100002] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[500100003] AddCollection Exception: {e}.");
                return Problem("[500100003] " + e.Message);
            }
        }
    }
}
