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
    public class AddItemComment
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddItemCommentWorkflow _workflow;
        private readonly IItemCommentLightValidator _lightValidator;

        public AddItemComment(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddItemComment>();
            _configuration = configuration;
            _workflow = new AddItemCommentWorkflow(loggerFactory, configuration);
            _lightValidator = new ItemCommentLightValidator();
        }

        [Function("AddItemComment")]
        public async Task<HttpResponseData> Run1([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            _logger.LogDebug("AddItemComment request received.");
       
            try
            {
                // Validate
                var comment = JsonConvert.DeserializeObject<ItemComment>(await new StreamReader(req.Body).ReadToEndAsync());

                var failures = _lightValidator.ValidateAddItemComment(comment);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                int id = await _workflow.AddItemComment(comment);

                // Respond
                _logger.LogInformation("AddItemComment success response.");

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