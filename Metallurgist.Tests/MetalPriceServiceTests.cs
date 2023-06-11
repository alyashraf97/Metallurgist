using Metallurgist.Interfaces;
using Metallurgist.Services;
using Moq;
using System.Text.Json;

namespace Metallurgist.Tests
{
    [TestFixture]
    public class MetalPriceServiceTests
    {
        private Mock<IHttpClientFactory> _mockHttpClientFactory;
        private Mock<MetalPriceDbContextBase> _mockDbContext;
        private MetalPriceService _service;

        [SetUp]
        public void SetUp()
        {
            // create mock objects for the dependencies
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockDbContext = new Mock<MetalPriceDbContextBase>();

            // create an instance of the service under test
            _service = new MetalPriceService(_mockHttpClientFactory.Object, _mockDbContext.Object);
        }

        [Test]
        public async Task GetMetalPrice_ShouldReturnCorrectPrice_WhenValidMetalNameIsGiven()
        {
            // arrange
            var metal = "copper";
            var expectedPrice = 9.5m;

            // create a fake http response with the expected price
            var fakeResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(new { price = expectedPrice }))
            };

            // set up the mock http client to return the fake response
            var mockHttpClient = new Mock<HttpClient>();
            mockHttpClient.Setup(c => c.GetAsync(It.IsAny<string>())).ReturnsAsync(fakeResponse);

            // set up the mock http client factory to return the mock http client
            _mockHttpClientFactory.Setup(f => f.CreateClient()).Returns(mockHttpClient.Object);

            // act
            var actualPrice = await _service.GetMetalPrice(metal);

            // assert
            Assert.AreEqual(expectedPrice, actualPrice);
        }

        // add more tests for other methods and scenarios here
    }
}
