using System;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BeamingInventory.Example.Presentation.App;
using BeamingInventory.Example.Presentation.Entities;
using BeamingInventory.Example.Presentation.Entities.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;
using HttpRequestMessage = System.Net.Http.HttpRequestMessage;

namespace BeamingInventory.Example.Presentation.Tests
{
    public class CommandServiceTests
    {
        private readonly CommandService _commandService;
        private readonly IConfiguration _configuration = new Configuration("https://coolapi.com");

        public CommandServiceTests()
        {
            var mockFactory = new Mock<IHttpClientFactory>();

            var mockMsgHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockMsgHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(new Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>((request,b) =>
                {
                    if (request.RequestUri.AbsolutePath.ToString() != _configuration.Endpoints['L'])
                        throw new NotImplementedException("Test not implemented for endpoint");

                    var content = new ApiResponse
                    {
                        Message = "The inventory count is 5",
                        Successful = true
                    };

                    var stuff = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8,
                        MediaTypeNames.Application.Json);

                    return Task.FromResult(new HttpResponseMessage
                    {
                        Content = stuff
                    });
                }));
            var httpClient = new HttpClient(mockMsgHandler.Object);

            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _commandService = new CommandService(_configuration, NullLogger.Instance, mockFactory.Object);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(5)]
        public async Task CanCreateSalesRequest(int count)
        {
            var body = JsonConvert.SerializeObject(new {count});
            var method = HttpMethod.Post;
            var endpoint = new Uri(_configuration.Endpoints['S'], UriKind.Relative);

            var actual = _commandService.CreateSalesRequest(count);
            var actualBody = await actual.Content.ReadAsStringAsync();

            Assert.Equal(body, actualBody);
            Assert.Equal(method, actual.Method);
            Assert.Equal(endpoint, actual.RequestUri);
        }

        [Theory]
        [InlineData(-5)]
        [InlineData(0)]
        public void CannotCreateNegativeSalesRequest(int count)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _commandService.CreateSalesRequest(count));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(5)]
        public async Task CanCreateInventoryAddRequest(int count)
        {
            var body = JsonConvert.SerializeObject(new {count});
            var method = HttpMethod.Post;
            var endpoint = new Uri(_configuration.Endpoints['I'], UriKind.Relative);

            var actual = _commandService.CreateInventoryAddRequest(count);
            var actualBody = await actual.Content.ReadAsStringAsync();

            Assert.Equal(body, actualBody);
            Assert.Equal(method, actual.Method);
            Assert.Equal(endpoint, actual.RequestUri);
        }

        [Theory]
        [InlineData(-5)]
        [InlineData(0)]
        public void CannotCreateNegativeInventoryAddRequest(int count)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _commandService.CreateInventoryAddRequest(count));
        }

        [Fact]
        public void CanCreateInventoryListRequest()
        {
            var method = HttpMethod.Get;
            var endpoint = new Uri(_configuration.Endpoints['I'], UriKind.Relative);

            var actual = _commandService.CreateInventoryListRequest();

            Assert.Null(actual.Content);
            Assert.Equal(method, actual.Method);
            Assert.Equal(endpoint, actual.RequestUri);
        }

        [Fact]
        public async Task CanSendRequest()
        {
            var commandsProvider = new CommandsProvider();
            var expected = new ApiResponse
            {
                Message = "The inventory count is 5",
                Successful = true
            };

            var commands = await commandsProvider.GetAsync();
            var command = commands.Single(a => a.CommandChar == 'L');
            var res = await _commandService.PerformAsync(command,null);

            Assert.Equal(expected.Message, res.Message);
            Assert.Equal(expected.Successful, res.Successful);
        }

    }
}
