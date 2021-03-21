using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using System.IO;
using System.Net.Http;
using TestWebApp.Tests.Setup;

namespace TestWebApp.Tests
{
    public class Teststest
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;
        //public Tests(CustomWebApplicationFactory<TestWebApp.Startup> factory)
        //{
        //    _factory = factory;
        //}

        [SetUp]
        public void Setup()
        {
            FileOperations.BeforeTest();
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
    }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [TearDown]
        public void AfterTest()
        {
            FileOperations.AfterTest();
        }
    }
}