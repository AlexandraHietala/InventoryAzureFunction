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
    public class AddCollection
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddCollectionWorkflow _workflow;
        private readonly ICollectionLightValidator _lightValidator;

        public AddCollection(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddCollection>();
            _configuration = configuration;
            _workflow = new AddCollectionWorkflow(loggerFactory, configuration);
            _lightValidator = new CollectionLightValidator();
        }

        [Function("AddCollection")]
        public async Task<HttpResponseData> Run1([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            _logger.LogDebug("AddCollection request received.");

            try
            {
                // Validate
                var collection = JsonConvert.DeserializeObject<Collection>(await new StreamReader(req.Body).ReadToEndAsync());
                var failures = _lightValidator.ValidateAddCollection(collection);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                int id = await _workflow.AddCollection(collection);

                // Respond
                _logger.LogInformation("AddCollection success response.");

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


