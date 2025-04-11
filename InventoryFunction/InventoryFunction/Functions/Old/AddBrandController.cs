using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BrandApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("AddBrandController")]
    [Route("api/v{version:apiVersion}/brand")]
    public class AddBrandController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IBrandControllerValidator _controllerValidator;
        private readonly IAddBrandWorkflow _addBrandWorkflow;

        public AddBrandController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddBrandController>();
            _configuration = configuration;
            _controllerValidator = new BrandControllerValidator();
            _addBrandWorkflow = new AddBrandWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        [Route("addbrand")]
        public async Task<IActionResult> AddBrand(string brandName, string? description, string lastmodifiedby)
        {
            _logger.LogDebug("AddBrand request received.");

            try
            {
                // Validate
                Brand brand = new Brand()
                {
                    Id = 0,
                    BrandName = brandName,
                    Description = description,
                    CreatedBy = lastmodifiedby,
                    CreatedDate = DateTime.Now,
                    LastModifiedBy = lastmodifiedby,
                    LastModifiedDate = DateTime.Now
                };

                var failures = _controllerValidator.ValidateAddBrand(brand);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                int id = await _addBrandWorkflow.AddBrand(brand);

                // Respond
                _logger.LogInformation("AddBrand success response.");
                return Ok(id);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[300100001] AddBrand ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[300100002] AddBrand InvalidOperationException: {ioe}.");
                return NotFound("[300100002] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[300100003] AddBrand Exception: {e}.");
                return Problem("[300100003] " + e.Message);
            }
        }
    }
}
