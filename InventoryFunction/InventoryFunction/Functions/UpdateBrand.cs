using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using InventoryFunction.Models.Classes;
using InventoryFunction.Validators.LightValidators;
using InventoryFunction.Workflows;
using InventoryFunction.Models.System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace InventoryFunction.Functions
{
    public class UpdateBrand
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUpdateBrandWorkflow _workflow;
        private readonly IBrandLightValidator _lightValidator;

        public UpdateBrand(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateBrand>();
            _configuration = configuration;
            _workflow = new UpdateBrandWorkflow(loggerFactory, configuration);
            _lightValidator = new BrandLightValidator();
        }

        [Function("UpdateBrand")]
        public async Task<HttpResponseData> Run1([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            _logger.LogDebug("UpdateBrand request received.");

            try
            {
                // Validate
                var brand = JsonConvert.DeserializeObject<Brand>(await new StreamReader(req.Body).ReadToEndAsync());

                var failures = _lightValidator.ValidateUpdateBrand(brand);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _workflow.UpdateBrand(brand);

                // Respond
                _logger.LogInformation("UpdateBrand success response.");

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                return response;
            }
            catch (ArgumentException ae)
            {
                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString($"{ae.Message}");
                return response;
            }
            catch (InvalidOperationException ioe)
            {
                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString($"{ioe.Message}");
                return response;
            }
            catch (Exception e)
            {
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString($"{e.Message}");
                return response;
            }
        }
    }
}


