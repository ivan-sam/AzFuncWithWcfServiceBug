using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ServiceModel;
using Microsoft.Extensions.Configuration;

namespace WcfCheckFunctionApp
{
    public static class Function
    {
        [FunctionName("WcfCheckHttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var address = new EndpointAddress(System.Environment.GetEnvironmentVariable("SimpleWcfServiceUrl"));
            var securityMode = string.Equals("https", address.Uri.Scheme, System.StringComparison.OrdinalIgnoreCase)
                ? BasicHttpSecurityMode.Transport
                : BasicHttpSecurityMode.None;
            var binding = new BasicHttpBinding(securityMode);
            using var channelFactory = new ChannelFactory<ISimpleWcfServiceChannel>(binding, address);

            using var channel = channelFactory.CreateChannel();
            var response = await channel.GetDataAsync();
            return new OkObjectResult(new { Response = response });
        }
    }

    [ServiceContract]
    public interface ISimpleWcfService
    {
        [OperationContract]
        Task<string> GetDataAsync();
    }

    public interface ISimpleWcfServiceChannel : ISimpleWcfService, System.ServiceModel.IClientChannel
    { }
}
