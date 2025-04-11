using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using Asp.Versioning;
using BrandApi.Validators.;
using BrandApi.Workflows.Workflows.;
using BrandApi.Models.Classes.;

namespace BrandApi.Controllers.
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ControllerName("GetBrandController")]
    [Route("api/v{version:apiVersion}/brand")]
    public class GetBrandController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IBrandControllerValidator _controllerValidator;
        private readonly IGetBrandWorkflow _getBrandWorkflow;

        public GetBrandController(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetBrandController>();
            _configuration = configuration;
            _controllerValidator = new BrandControllerValidator();
            _getBrandWorkflow = new GetBrandWorkflow(loggerFactory, configuration);
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getbrand")]
        public async Task<IActionResult> GetBrand(int id)
        {
            _logger.LogDebug("GetBrand request received.");

            try
            {
                // Validate
                var failures = _controllerValidator.ValidateBrandId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                Brand brand = await _getBrandWorkflow.GetBrand(id);

                // Respond
                _logger.LogInformation("GetBrand success response.");
                return Ok(brand);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[300100004] GetBrand ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[300100005] GetBrand InvalidOperationException: {ioe}.");
                return NotFound("[300100005] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[300100006] GetBrand Exception: {e}.");
                return Problem("[300100006] " + e.Message);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("getbrands")]
        public async Task<IActionResult> GetBrands() // TODO: Add search string param
        {
            _logger.LogDebug("GetBrands request received.");

            try
            {
                // Validate
                // Nothing to validate

                // Process
                List<Brand> brands = await _getBrandWorkflow.GetBrands();

                // Respond
                _logger.LogInformation("GetBrands success response.");
                return Ok(brands);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError($"[300100007] GetBrands ArgumentException: {ae}.");
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError($"[300100008] GetBrands InvalidOperationException: {ioe}.");
                return NotFound("[300100008] " + ioe.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"[300100009] GetBrands Exception: {e}.");
                return Problem("[300100009] " + e.Message);
            }
        }
    }
}
