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

namespace InventoryFunction.Functions
{
    public class RemoveSeries
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IRemoveSeriesWorkflow _workflow;
        private readonly ISeriesLightValidator _lightValidator;

        public RemoveSeries(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<RemoveSeries>();
            _configuration = configuration;
            _workflow = new RemoveSeriesWorkflow(loggerFactory, configuration);
            _lightValidator = new SeriesLightValidator();
        }

        [Function("RemoveSeries")]
        public async Task<HttpResponseData> Run1([HttpTrigger(AuthorizationLevel.Anonymous, "delete")] HttpRequestData req)
        {
            _logger.LogDebug("RemoveSeries request received.");

            try
            {
                // Validate
                var idUser = JsonConvert.DeserializeObject<GenericIdUser>(await new StreamReader(req.Body).ReadToEndAsync());

                var failures = _lightValidator.ValidateSeriesId(idUser.Id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _workflow.RemoveSeries(idUser.Id, idUser.LastModifiedBy);

                // Respond
                _logger.LogInformation("RemoveSeries success response.");

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


