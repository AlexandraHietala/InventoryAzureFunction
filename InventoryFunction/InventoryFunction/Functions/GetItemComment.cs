using System;
using System.Collections.Generic;
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
    public class GetItemComment
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IGetItemCommentWorkflow _workflow;
        private readonly IItemCommentLightValidator _commentLightValidator;
        private readonly IItemLightValidator _itemLightValidator;

        public GetItemComment(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<GetItemComment>();
            _configuration = configuration;
            _workflow = new GetItemCommentWorkflow(loggerFactory, configuration);
            _commentLightValidator = new ItemCommentLightValidator();
            _itemLightValidator = new ItemLightValidator();
        }

        [Function("GetItemComment")]
        public async Task<HttpResponseData> Run1([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetItemComment/{commentId}")] HttpRequestData req, int commentId)
        {
            _logger.LogDebug("GetItemComment request received.");
            
            try
            {
                // Validate
                var failures = _commentLightValidator.ValidateItemCommentId(commentId);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                ItemComment comment = await _workflow.GetItemComment(commentId);

                // Respond
                _logger.LogInformation("GetItemComment success response.");

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString(JsonConvert.SerializeObject(comment));
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


        [Function("GetItemCommentsByItem")]
        public async Task<HttpResponseData> Run2([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetItemCommentsByItem/{itemId}")] HttpRequestData req, int itemId)
        {
            _logger.LogDebug("GetItemCommentsByItem request received.");
      
            try
            {
                // Validate
                var failures = _itemLightValidator.ValidateItemId(itemId);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                List<ItemComment> comments = await _workflow.GetItemComments(itemId);

                // Respond
                _logger.LogInformation("GetItemCommentsByItem success response.");

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString(JsonConvert.SerializeObject(comments));
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


