using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SEInventoryCollection.Models.Classes;
using SEInventoryCollection.Validators.LightValidators;
using SEInventoryCollection.Workflows;

namespace SEInventoryCollection.Functions
{
    public class AddItem
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddItemWorkflow _workflow;
        private readonly IItemLightValidator _lightValidator;

        public AddItem(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddItem>();
            _configuration = configuration;
            _workflow = new AddItemWorkflow(loggerFactory, configuration);
            _lightValidator = new ItemLightValidator();
        }

        [Function("AddItem")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogDebug("AddItem request received.");

            var item = JsonConvert.DeserializeObject<Item>(await new StreamReader(req.Body).ReadToEndAsync());

            try
            {
                // Validate
                //Item item = new Item()
                //{
                //    Id = 0,
                //    CollectionId = collectionId,
                //    Status = status,
                //    Type = type,
                //    BrandId = brandId,
                //    SeriesId = seriesId,
                //    Name = name,
                //    Description = description,
                //    Format = format,
                //    Size = size,
                //    Year = year,
                //    Photo = photo,
                //    CreatedBy = lastmodifiedby,
                //    CreatedDate = DateTime.Now,
                //    LastModifiedBy = lastmodifiedby,
                //    LastModifiedDate = DateTime.Now
                //};

                var failures = _lightValidator.ValidateAddItem(item);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                int id = await _workflow.AddItem(item);

                // Respond
                _logger.LogInformation("AddItem success response.");

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString(id.ToString());
                return response;
            }
            catch (ArgumentException ae)
            {
                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString($"[200100004] AddItem ArgumentException: {ae.Message}.");
                return response;
            }
            catch (InvalidOperationException ioe)
            {
                var response = req.CreateResponse(HttpStatusCode.BadRequest);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString($"[200100005] AddItem InvalidOperationException: {ioe.Message}.");
                return response;
            }
            catch (Exception e)
            {
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString($"[200100006] AddItem Exception: {e.Message}.");
                return response;
            }
        }
    }
}


