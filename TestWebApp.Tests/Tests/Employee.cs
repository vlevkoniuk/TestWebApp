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
using TestWebApp.Helpers;

namespace TestWebApp.Tests.Tests.Employee
{

    class Employee
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;
        private string urlemploees = "/employee";
        private string urlemploeesId = "/employee/id/";
        private string urlemploeesPos = "/employee/position/";
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
            XMLActions.CorrectXMLIDs();
        }

        [SetUp]
        public void Setup()
        {
            FileOperations.BeforeTest();
            xdoc = XDocument.Load("./Data/Data[1].xml");
        }

        [Test]
        public async Task TestThatGetAllEmployeesWorks()
        {
            //send request to the server to /employee
            HttpResponseMessage response = await _client.GetAsync(urlemploees);

            //Verify that the we have OK status code
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK, "Sorry, the response status is NotOK");

            //Compare the the response from the HttpClient and the XML to have all the data
            dynamic JSONresp = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
            bool comp = Comparisons.ContainsMany(JSONresp, xdoc);
            Assert.IsTrue(comp, "XML objects and respnse object are not equal");
        }

        [Test]
        public async Task TestThatGetEmployeeWithIdWorks()
        {
            //send request to the server to /employee/id/#id
            string id = "5";
            HttpResponseMessage response = await _client.GetAsync(urlemploeesId + id);

            //Verify that the we have OK status code
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK, "Sorry, the response status is NotOK");

            //Compare the the response from the HttpClient and the XML to have all the right data
            dynamic JSONresp = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
            bool comp = Comparisons.ContainsCorrectObject(JSONresp, xdoc);
            Assert.IsTrue(comp, "XML objects and respnse object are not equal");
        }

        [Test]
        public async Task TestThatGetEmployeeWithIdNotFound()
        {
            //send request to the server to /employee/id/#id
            string id = "0";
            HttpResponseMessage response = await _client.GetAsync(urlemploeesId + id);

            //Verify that the we have OK status code
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound, "Sorry, the response status is not NotFound");
        }

        [Test]
        public async Task TestThatGetEmployeeWithOnePositionWorks()
        {
            //send request to the server to /employee/position/#position
            string position = "Regular";
            HttpResponseMessage response = await _client.GetAsync(urlemploeesPos + position);

            //Verify that the we have OK status code
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK, "Sorry, the response status is NotOK");

            //Compare the the response from the HttpClient and the XML to have all the right data
            dynamic JSONresp = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
            bool comp = Comparisons.ContainsAtLeastOne(JSONresp, xdoc, position);
            Assert.IsTrue(comp, "XML objects and respnse object are not equal");
        }

        [Test]
        public async Task TestThatGetEmployeeWithSeveralPositionWorks()
        {
            //send request to the server to /employee/position/#position
            string position = "Senior";
            HttpResponseMessage response = await _client.GetAsync(urlemploeesPos + position);

            //Verify that the we have OK status code
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK, "Sorry, the response status is NotOK");

            //Compare the the response from the HttpClient and the XML to have all the right data
            dynamic JSONresp = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
            bool comp = Comparisons.ContainsAtLeastOne(JSONresp, xdoc, position);
            Assert.IsTrue(comp, "XML objects and respnse object are not equal");
        }

        [Test]
        public async Task TestThatGetEmployeeWithPositionNotFound()
        {
            //send request to the server to /employee/position/#position
            string position = "notfound";
            HttpResponseMessage response = await _client.GetAsync(urlemploeesPos + position);

            //Verify that the we have OK status code
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound, "Sorry, the response status is NotOK");
        }

        [TearDown]
        public void AfterTest()
        {
            //FileOperations.AfterTest();
        }
    }
}
