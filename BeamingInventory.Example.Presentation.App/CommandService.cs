using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BeamingInventory.Example.Presentation.Entities;
using BeamingInventory.Example.Presentation.Entities.Interfaces;
using Microsoft.Extensions.Logging;

namespace BeamingInventory.Example.Presentation.App
{
    //For the sake of this example, we'll settle for having some hardcoded assumptions here, like endpoints and that it's feasible
    //to define a transformation method for each command available.
    //Could have been from a db, config file, a discovery endpoint in the API, etc. This could be endlessly complex if we wanted to so let's settle here.
    public class CommandService : ICommandService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public CommandService(IConfiguration configuration, ILogger logger, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(configuration.ApiUrl, UriKind.Absolute);
        }

        private static StringContent CreateJsonContent(object obj)
        {
            return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        public HttpRequestMessage CreateSalesRequest(int count)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count), count, "Expected a value above zero");
            return new HttpRequestMessage(HttpMethod.Post, _configuration.Endpoints['S'])
            {
                Content = CreateJsonContent(new { count })
            };
        }

        public HttpRequestMessage CreateInventoryAddRequest(int count)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count), count, "Expected a value above zero");
            return new HttpRequestMessage(HttpMethod.Post, _configuration.Endpoints['I'])
            {
                Content = CreateJsonContent(new { count })
            };
        }

        public HttpRequestMessage CreateInventoryListRequest()
        {
            return new HttpRequestMessage(HttpMethod.Get, _configuration.Endpoints['I']);
        }

        private static TOut ConvertParam<TIn, TOut>(TIn param) where TIn : IConvertible
        {
            return (TOut)Convert.ChangeType(param, typeof(TIn));
        }

        public HttpRequestMessage CreateBody(CommandType commandType, string? param)
        {
            return commandType.CommandChar switch
            {
                'S' => CreateSalesRequest(ConvertParam<string, int>(param ?? throw new ArgumentNullException(nameof(param)))),
                'I' => CreateInventoryAddRequest(ConvertParam<string, int>(param ?? throw new ArgumentNullException(nameof(param)))),
                'L' => CreateInventoryListRequest(),
                _ => throw new InvalidOperationException($"Command '{commandType.CommandChar}' doesn't have a handler associated with it")
            };
        }

        public async Task<ApiResponse> PerformAsync(CommandType commandType, string? param)
        {
            var body = CreateBody(commandType, param);
            var responseMessage = await _httpClient.SendAsync(body);
            var returnBody = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode)
            {
                _logger.LogError($"Error performing command {commandType.CommandChar} with param '{param}'. Details: {responseMessage.ReasonPhrase}");
            }

            try
            {
                var response = JsonSerializer.Deserialize<ApiResponse>(returnBody);
                if (!response.Successful) _logger.LogError($"Error with command {commandType.CommandChar} (param: {param}). Message: {response.Message}");
                return response;
            }
            catch
            {
                _logger.LogCritical($"Api response couldn't be deserialised: {returnBody}");
                return new ApiResponse
                {
                    Message = "Unknown error with API",
                    Successful = false
                };
            }
        }
    }
}
