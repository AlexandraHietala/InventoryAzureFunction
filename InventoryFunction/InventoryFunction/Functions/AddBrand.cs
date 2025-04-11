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

namespace InventoryFunction.Functions
{
    public class AddBrand
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddBrandWorkflow _workflow;
        private readonly IBrandLightValidator _lightValidator;

        public AddBrand(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddBrand>();
            _configuration = configuration;
            _workflow = new AddBrandWorkflow(loggerFactory, configuration);
            _lightValidator = new BrandLightValidator();
        }

        [Function("AddBrand")]
        public async Task<HttpResponseData> Run1([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogDebug("AddBrand request received.");

            try
            {
                // Validate
                var brand = JsonConvert.DeserializeObject<Brand>(await new StreamReader(req.Body).ReadToEndAsync());

                //Brand brand = new Brand()
                //{
                //    Id = 0,
                //    BrandName = brandName,
                //    Description = description,
                //    CreatedBy = lastmodifiedby,
                //    CreatedDate = DateTime.Now,
                //    LastModifiedBy = lastmodifiedby,
                //    LastModifiedDate = DateTime.Now
                //};

                var failures = _lightValidator.ValidateAddBrand(brand);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                int id = await _workflow.AddBrand(brand);

                // Respond
                _logger.LogInformation("AddBrand success response.");

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString(id.ToString());
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


