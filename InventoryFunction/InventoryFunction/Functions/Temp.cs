//using System;
//using System.IO;
//using System.Net;
//using System.Threading.Tasks;
//using Microsoft.Azure.Functions.Worker;
//using Microsoft.Azure.Functions.Worker.Http;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using InventoryFunction.Models.Classes;
//using InventoryFunction.Validators.LightValidators;
//using InventoryFunction.Workflows;

//namespace InventoryFunction.Functions
//{
//    public class XXX
//    {
//        private readonly ILogger _logger;
//        private readonly IConfiguration _configuration;
//        private readonly IXXXWorkflow _workflow;
//        private readonly IXXXLightValidator _lightValidator;

//        public XXX(ILoggerFactory loggerFactory, IConfiguration configuration)
//        {
//            _logger = loggerFactory.CreateLogger<XXX>();
//            _configuration = configuration;
//            _workflow = new XXXWorkflow(loggerFactory, configuration);
//            _lightValidator = new XXXLightValidator();
//        }

//        [Function("XXX")]
//        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
//        {
//            _logger.LogDebug("XXX request received.");

//            var XXX = JsonConvert.DeserializeObject<XXX>(await new StreamReader(req.Body).ReadToEndAsync());

//            try
//            {
//                XXX

//                // Respond
//                _logger.LogInformation("XXX success response.");

//                var response = req.CreateResponse(HttpStatusCode.OK);
//                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
//                response.WriteString(id.ToString());
//                return response;
//            }
//            catch (ArgumentException ae)
//            {
//                var response = req.CreateResponse(HttpStatusCode.BadRequest);
//                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
//                response.WriteString($"{ae.Message}.");
//                return response;
//            }
//            catch (InvalidOperationException ioe)
//            {
//                var response = req.CreateResponse(HttpStatusCode.BadRequest);
//                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
//                response.WriteString($"[XXX] XXX InvalidOperationException: {ioe.Message}.");
//                return response;
//            }
//            catch (Exception e)
//            {
//                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
//                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
//                response.WriteString($"[XXX] XXX Exception: {e.Message}.");
//                return response;
//            }
//        }
//    }
//}


