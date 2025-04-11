using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BrandApi.Models.Classes.;
using BrandApi.Validators.;
using BrandApi.Workflows.Workflows.;

namespace BrandApi.Controllers.
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("UpdateBrandController")]
    [Route("api/v{version:apiVersion}/brand")]
    public class UpdateBrandController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IBrandControllerValidator _controllerValidator;
        private readonly IUpdateBrandWorkflow _updateBrandWorkflow;

        public UpdateBrandController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateBrandController>();
            _configuration = configuration;
            _controllerValidator = new BrandControllerValidator();
            _updateBrandWorkflow = new UpdateBrandWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPut]
        [Route("updatebrand")]
        public async Task<IActionResult> UpdateBrand(int id, string brandName, string? description, string lastmodifiedby)
        {
            _logger.LogDebug("UpdateBrand request received.");

            try
            {
                // Validate
                Brand item = new Brand()
                {
                    Id = id,
                    BrandName = brandName,
                    Description = description,
                    CreatedBy = lastmodifiedby,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = lastmodifiedby,
                    LastModifiedDate = DateTime.Now
                };

                var failures = _controllerValidator.ValidateUpdateBrand(item);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _updateBrandWorkflow.UpdateBrand(item);

                // Respond
                _logger.LogInformation("UpdateBrand success response.");
                return Ok();
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[300100013] UpdateBrand ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[300100014] UpdateBrand InvalidOperationException: {ioe}.");
                return NotFound("[300100014] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[300100015] UpdateBrand Exception: {e}.");
                return Problem("[300100015] " + e.Message);
            }
        }
    }
}
