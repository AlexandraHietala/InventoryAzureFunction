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
using System.Collections.ObjectModel;
using System.Drawing;

namespace InventoryFunction.Functions
{
    public class UpdateItem
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUpdateItemWorkflow _workflow;
        private readonly IItemLightValidator _lightValidator;

        public UpdateItem(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<UpdateItem>();
            _configuration = configuration;
            _workflow = new UpdateItemWorkflow(loggerFactory, configuration);
            _lightValidator = new ItemLightValidator();
        }

        [Function("UpdateItem")]
        public async Task<HttpResponseData> Run1([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogDebug("UpdateItem request received.");

            try
            {
                // Validate
                var item = JsonConvert.DeserializeObject<Item>(await new StreamReader(req.Body).ReadToEndAsync());

                //Item item = new Item()
                //{
                //    Id = id,
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

                var failures = _lightValidator.ValidateUpdateItem(item);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                await _workflow.UpdateItem(item);

                // Respond
                _logger.LogInformation("UpdateItem success response.");

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


