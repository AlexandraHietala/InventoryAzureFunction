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
    [ControllerName("UpdateItemCommentController")]
    [Route("api/v{version:apiVersion}/comment")]
    public class UpdateItemCommentController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IItemCommentControllerValidator _controllerValidator;
        private readonly IUpdateItemCommentWorkflow _updateItemCommentWorkflow;

        public UpdateItemCommentController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateItemCommentController>();
            _configuration = configuration;
            _controllerValidator = new ItemCommentControllerValidator();
            _updateItemCommentWorkflow = new UpdateItemCommentWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPut]
        [Route("updateitemcomment")]
        public async Task<IActionResult> UpdateItemComment(int id, int itemId, string comment, string lastmodifiedby)
        {
            _logger.LogDebug("UpdateItemComment request received.");

            try
            {
                // Validate
                ItemComment item = new ItemComment()
                {
                    Id = id,
                    ItemId = itemId,
                    Comment = comment,
                    CreatedBy = lastmodifiedby,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = lastmodifiedby,
                    LastModifiedDate = DateTime.Now
                };

                var failures = _controllerValidator.ValidateUpdateItemComment(item);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _updateItemCommentWorkflow.UpdateItemComment(item);

                // Respond
                _logger.LogInformation("UpdateItemComment success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[200100028] UpdateItemComment ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[200100029] UpdateItemComment InvalidOperationException: {ioe}.");
                return NotFound("[200100029] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[200100030] UpdateItemComment Exception: {e}.");
                return Problem("[200100030] " + e.Message);
            }
        }
    }
}
