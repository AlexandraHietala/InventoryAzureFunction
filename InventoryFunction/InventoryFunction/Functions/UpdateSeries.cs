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
    public class UpdateSeries
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUpdateSeriesWorkflow _workflow;
        private readonly ISeriesLightValidator _lightValidator;

        public UpdateSeries(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateSeries>();
            _configuration = configuration;
            _workflow = new UpdateSeriesWorkflow(loggerFactory, configuration);
            _lightValidator = new SeriesLightValidator();
        }

        [Function("UpdateSeries")]
        public async Task<HttpResponseData> Run1([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogDebug("UpdateSeries request received.");

            try
            {
                var series = JsonConvert.DeserializeObject<Series>(await new StreamReader(req.Body).ReadToEndAsync());

                // Validate
                //Series item = new Series()
                //{
                //    Id = id,
                //    SeriesName = seriesName,
                //    Description = description,
                //    CreatedBy = lastmodifiedby,
                //    CreatedDate = DateTime.Now,
                //    LastModifiedBy = lastmodifiedby,
                //    LastModifiedDate = DateTime.Now
                //};

                var failures = _lightValidator.ValidateUpdateSeries(series);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _workflow.UpdateSeries(series);

                // Respond
                _logger.LogInformation("UpdateSeries success response.");

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


