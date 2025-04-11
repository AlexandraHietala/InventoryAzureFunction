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
using System.Collections;

namespace InventoryFunction.Functions
{
    public class UpdateCollection
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUpdateCollectionWorkflow _workflow;
        private readonly ICollectionLightValidator _lightValidator;

        public UpdateCollection(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateCollection>();
            _configuration = configuration;
            _workflow = new UpdateCollectionWorkflow(loggerFactory, configuration);
            _lightValidator = new CollectionLightValidator();
        }

        [Function("UpdateCollection")]
        public async Task<HttpResponseData> Run1([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            _logger.LogDebug("UpdateCollection request received.");

            try
            {
                // Validate
                var collection = JsonConvert.DeserializeObject<Collection>(await new StreamReader(req.Body).ReadToEndAsync());

                var failures = _lightValidator.ValidateUpdateCollection(collection);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _workflow.UpdateCollection(collection);


                // Respond
                _logger.LogInformation("UpdateCollection success response.");

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


