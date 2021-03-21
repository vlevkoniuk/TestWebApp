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

namespace TestWebApp.Tests.Tests.UpdateID
{

    class UpdateId
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;
        private string urlupdateid = "/update/id/1";
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
        public async Task TestThatPostUpdateEmpty()
        {
            //send request to the server to /Update/id/1
            HttpContent content = new StringContent(JsonConvert.SerializeObject("{}"));
            content.Headers.ContentType.MediaType = MediaTypeNames.Application.Json;
            HttpResponseMessage response = await _client.PostAsync(urlupdateid, content);

            //Verify that the we have NotOK status code
            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest, "Sorry, the response status is not BadRequest");

            content = new StringContent("");
            response = await _client.PostAsync(urlupdateid, content);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.UnsupportedMediaType, "Sorry, the response status is not UnsupportedMediaType");


        }

        [Test]
        public async Task TestThatPostUpdateWithPartialValidJson()
        {
            //send request to the server to /Update/id/1

            var obj = new { Specialization = "TestSpecialization" };
            HttpContent content = new StringContent(JsonConvert.SerializeObject(obj));
            content.Headers.ContentType.MediaType = MediaTypeNames.Application.Json;
            HttpResponseMessage response = await _client.PostAsync(urlupdateid, content);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode,  "Sorry, the response status is not OK");
            //debug
            var message = await response.Content.ReadAsStringAsync();
            //verify the patched XML with previous XML state
            var xdoc1 = XDocument.Load("./Data/Data[1].xml");

            var changeddict = Comparisons.CompareXMLsForChanges(xdoc, xdoc1, "1");

            
            Assert.IsTrue(changeddict.ContainsKey("Specialization"), "No changed data");
        }

        [Test]
        public async Task TestThatPostUpdateWithNotCorrect()
        {
            //send request to the server to /Update/id/1
            HttpContent content = new StringContent(JsonConvert.SerializeObject("{\"test\": 1}"));
            content.Headers.ContentType.MediaType = MediaTypeNames.Application.Json;
            HttpResponseMessage response = await _client.PostAsync(urlupdateid, content);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest, "Sorry, the response status is not BadRequest");
        }

        [Test]
        public async Task TestThatPostUpdateWithNotCorrectModel()
        {
            //send request to the server to /Update/id/1
            XMLUpdEmployee xempl = new XMLUpdEmployee("AQA", "Strong", "John Doe", "test", "3");
            HttpContent content = new StringContent(JsonConvert.SerializeObject(xempl));
            content.Headers.ContentType.MediaType = MediaTypeNames.Application.Json;
            HttpResponseMessage response = await _client.PostAsync(urlupdateid, content);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest, "Sorry, the response status is not BadRequest");
        }


        [TearDown]
        public void AfterTest()
        {
            //FileOperations.AfterTest();
        }
    }
}
