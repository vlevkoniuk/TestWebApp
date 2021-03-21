using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using TestWebApp.Tests.Setup;
using TestWebApp.Tests.Helpers;
using System.Net.Mime;
using System.Text.RegularExpressions;
using TestWebApp.Models;

namespace TestWebApp.Tests.Tests.AddGeneral
{

    class AddGeneral
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;
        private string urladd = "/add";
        private XDocument xdoc;

        //public Tests(CustomWebApplicationFactory<TestWebApp.Startup> factory)
        //{
        //    _factory = factory;
        //}
        [OneTimeSetUp]
        public void InitWebFactory()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [SetUp]
        public void Setup()
        {
            FileOperations.BeforeTest();
            xdoc = XDocument.Load("./Data/Data[1].xml");
        }

        [Test]
        public async Task TestThatPostAddValidWithValidJson()
        {
            //send request to the server to /Add
            XMLEmployee xempl = new XMLEmployee("10", "AQA", "Strong", "John Doe", "2500", "3");
            //xempl.Name = "";
            HttpContent content = new StringContent(JsonConvert.SerializeObject(xempl));
            content.Headers.ContentType.MediaType = MediaTypeNames.Application.Json;
            HttpResponseMessage response = await _client.PostAsync(urladd, content);
            
            //debug
            var message = await response.Content.ReadAsStringAsync();

            //Verify that the we have NotOK status code
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode,  "Sorry, the response status is not OK");
        }

        [Test]
        public async Task TestThatPostAddValidJsonButExistingData()
        {
            //send request to the server to /Add
            XMLEmployee xempl = new XMLEmployee("10", "SDET", "Senior", "John Doe Jr", "2500", "3");
            //xempl.Name = "";
            HttpContent content = new StringContent(JsonConvert.SerializeObject(xempl));
            content.Headers.ContentType.MediaType = MediaTypeNames.Application.Json;
            HttpResponseMessage response = await _client.PostAsync(urladd, content);

            //Verify that the we have NotOK status code
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Sorry, the response status is not OK");

            //Try to add the same object
            response = await _client.PostAsync(urladd, content);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Sorry, the response status is not BadRequest");
        }





        [TearDown]
        public void AfterTest()
        {
            //FileOperations.AfterTest();
        }
    }
}
