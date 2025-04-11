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
using System.Security.Policy;

namespace InventoryFunction.Functions
{
    public class AddUser
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IAddUserWorkflow _workflow;
        private readonly IUserLightValidator _lightValidator;

        public AddUser(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<AddUser>();
            _configuration = configuration;
            _workflow = new AddUserWorkflow(loggerFactory, configuration);
            _lightValidator = new UserLightValidator();
        }

        [Function("AddUser")]
        public async Task<HttpResponseData> Run1([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogDebug("AddUser request received.");

            try
            {
                // Validate
                var user = JsonConvert.DeserializeObject<User>(await new StreamReader(req.Body).ReadToEndAsync());

                //User user = new User()
                //{
                //    Name = name,
                //    PassSalt = salt,
                //    PassHash = hash,
                //    RoleId = roleId,
                //    LastModifiedBy = lastmodifiedby,
                //    LastModifiedDate = DateTime.Now,
                //    CreatedBy = lastmodifiedby,
                //    CreatedDate = DateTime.Now
                //};

                var failures = _lightValidator.ValidateAdd(user);
                if (!string.IsNullOrEmpty(failures)) throw new ArgumentException(failures);

                // Process
                int id = await _workflow.AddUser(user);

                // Respond
                _logger.LogInformation("AddUser success response.");

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


