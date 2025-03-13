using Newtonsoft.Json;
using System.Text;

namespace TeaAPI.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestLog = await LogRequest(context);
            _logger.LogInformation(requestLog);

            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                await _next(context);
                var responseLog = await LogResponse(context);
                _logger.LogInformation(responseLog);

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true).ReadToEndAsync();
            context.Request.Body.Position = 0;

            var log = new
            {
                Type = "Request",
                context.Request.Method,
                context.Request.Path,
                QueryString = context.Request.QueryString.ToString(),
                Body = body
            };
            return JsonConvert.SerializeObject(log);
        }

        private async Task<string> LogResponse(HttpContext context)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body, Encoding.UTF8, leaveOpen: true).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var log = new
            {
                Type = "Response",
                context.Response.StatusCode,
                Body = body
            };
            return JsonConvert.SerializeObject(log);
        }
    }
}
