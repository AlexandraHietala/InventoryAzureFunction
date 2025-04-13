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
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker.Extensions;
using Microsoft.AspNetCore.Mvc;


namespace InventoryFunction.Functions
{
    public class GetBrand
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetBrandWorkflow _workflow;
        private readonly IBrandLightValidator _lightValidator;

        public GetBrand(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetBrand>();
            _configuration = configuration;
            _workflow = new GetBrandWorkflow(loggerFactory, configuration);
            _lightValidator = new BrandLightValidator();
        }

        [Function("GetBrand")]
        public async Task<HttpResponseData> Run1([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetBrand/{id}")] HttpRequestData req, int id)
        {
            _logger.LogDebug("GetBrand request received.");

            try
            {
                // Validate
                var failures = _lightValidator.ValidateBrandId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                Brand brand = await _workflow.GetBrand(id);

                // Respond
                _logger.LogInformation("GetBrand success response.");

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString(JsonConvert.SerializeObject(brand));
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

        [Function("GetBrands")]
        public async Task<HttpResponseData> Run2([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetBrands/{search?}")] HttpRequestData req, string search)
        {
            _logger.LogDebug("GetBrands request received.");
 
            try
            {
                // Validate
                // TODO: Validate search string

                // Process
                List<Brand> brands = await _workflow.GetBrands(search);

                // Respond
                _logger.LogInformation("GetBrands success response.");

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString(JsonConvert.SerializeObject(brands));
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


