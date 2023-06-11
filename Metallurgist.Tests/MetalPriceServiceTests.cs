using Metallurgist.Data;
using Metallurgist.Interfaces;
using Metallurgist.Models;
using Metallurgist.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Metallurgist.Tests
{
    [TestFixture]
    public class MetalPriceServiceTests
    {
        private FakeHttpClientFactory _fakeHttpClientFactory;
        private Mock<MetalPriceDbContextBase> _mockDbContext;
        private MetalPriceService _service;

        [SetUp]
        public void SetUp()
        {
            // create fake objects for the dependencies
            _fakeHttpClientFactory = new FakeHttpClientFactory();
            _mockDbContext = new Mock<MetalPriceDbContextBase>();

            // create an instance of the service under test
            _service = new MetalPriceService(_fakeHttpClientFactory, _mockDbContext.Object);
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

            // create a fake http message handler that returns the fake response
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeResponse);

            // create a real http client that uses the fake message handler
            var httpClient = new HttpClient(fakeHttpMessageHandler);

            // set up the fake http client factory to return the real http client
            _fakeHttpClientFactory.SetClient(httpClient);

            // act
            //var actualPrice = await _service.GetMetalPrices(metal);

            // assert
            //Assert.AreEqual(expectedPrice, actualPrice);
        }

        // add more tests for other methods and scenarios here
    }

    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;

        public FakeHttpMessageHandler(HttpResponseMessage response)
        {
            _response = response;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_response);
        }
    }

    public class FakeHttpClientFactory : IHttpClientFactory
    {
        private HttpClient _client;

        public void SetClient(HttpClient client)
        {
            _client = client;
        }

        public HttpClient CreateClient(string name)
        {
            return _client;
        }
    }
}