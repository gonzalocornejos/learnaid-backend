using learnaid_backend.Core.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace learnaid_backend.Core.Middleware
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate nextMiddleware;

        public ExceptionHandler(RequestDelegate nextMiddleware)
        {
            this.nextMiddleware = nextMiddleware;
        }

        /// <summary>
        ///    Procesa el mensaje, atrapando las excepciones generadas.
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await nextMiddleware.Invoke(context);
            }
            catch (AppException exception)
            {
                await SendPayload(context, Envelope.Error(exception.Message), exception.StatusCode);
            }
            catch (Exception exception)
            {
                await SendPayload(context, Envelope.Error(exception.Message), HttpStatusCode.InternalServerError);
            }
        }

        private async Task SendPayload<TPayload>(HttpContext context, TPayload payload, HttpStatusCode statusCode)
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var result = JsonConvert.SerializeObject(payload, settings);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(result);
        }
    }
}
