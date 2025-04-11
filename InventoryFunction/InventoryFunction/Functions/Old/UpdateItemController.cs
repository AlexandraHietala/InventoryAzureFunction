using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ItemApi.Models.Classes.;
using ItemApi.Validators.;
using ItemApi.Workflows.Workflows.;

namespace ItemApi.Controllers.
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("UpdateItemController")]
    [Route("api/v{version:apiVersion}/item")]
    public class UpdateItemController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IItemControllerValidator _controllerValidator;
        private readonly IUpdateItemWorkflow _updateItemWorkflow;

        public UpdateItemController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateItemController>();
            _configuration = configuration;
            _controllerValidator = new ItemControllerValidator();
            _updateItemWorkflow = new UpdateItemWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPut]
        [Route("updateitem")]
        public async Task<IActionResult> UpdateItem(int id, int collectionId, string status, string type, int? brandId, int? seriesId, string? name, string? description, string format, string size, int? year, string? photo, string lastmodifiedby)
        {
            _logger.LogDebug("UpdateItem request received.");

            try
            {
                // Validate
                Item item = new Item()
                {
                    Id = id,
                    CollectionId = collectionId,
                    Status = status,
                    Type = type,
                    BrandId = brandId,
                    SeriesId = seriesId,
                    Name = name,
                    Description = description,
                    Format = format,
                    Size = size,
                    Year = year,
                    Photo = photo,
                    CreatedBy = lastmodifiedby,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = lastmodifiedby,
                    LastModifiedDate = DateTime.Now
                };

                var failures = _controllerValidator.ValidateUpdateItem(item);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _updateItemWorkflow.UpdateItem(item);

                // Respond
                _logger.LogInformation("UpdateItem success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100031] UpdateItem ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100032] UpdateItem InvalidOperationException: {ioe}.");
                return NotFound("[200100032] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100033] UpdateItem Exception: {e}.");
                return Problem("[200100033] " + e.Message);
            }
        }
    }
}
