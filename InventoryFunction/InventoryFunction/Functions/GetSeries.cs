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

namespace InventoryFunction.Functions
{
    public class GetSeries
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetSeriesWorkflow _workflow;
        private readonly ISeriesLightValidator _lightValidator;

        public GetSeries(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetSeries>();
            _configuration = configuration;
            _workflow = new GetSeriesWorkflow(loggerFactory, configuration);
            _lightValidator = new SeriesLightValidator();
        }

        [Function("GetASeries")]
        public async Task<HttpResponseData> Run1([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetASeries/{id}")] HttpRequestData req, int id)
        {
            _logger.LogDebug("GetASeries request received.");

            try
            {
                // Validate
                var failures = _lightValidator.ValidateSeriesId(id);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                Series series = await _workflow.GetASeries(id);

                // Respond
                _logger.LogInformation("GetASeries success response.");

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString(JsonConvert.SerializeObject(series));
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

        [Function("GetSeries")]
        public async Task<HttpResponseData> Run2([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetSeries/{search?}")] HttpRequestData req, string search)
        {
            _logger.LogDebug("GetSeries request received.");

            try
            {
                // Validate
                // TODO: Validate search string

                // Process
                List<Series> series = await _workflow.GetSeries(search);

                // Respond
                _logger.LogInformation("GetSeries success response.");

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString(JsonConvert.SerializeObject(series));
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


