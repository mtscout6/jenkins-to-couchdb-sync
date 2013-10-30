using System.Net;
using NUnit.Framework;
using RestSharp;
using Should;
using sabatoast_puller.Couch;

namespace sabatoast_puller.Tests.Couch
{
    public class CouchClientStaleDocumentFailTester : CouchDbInteractionContext
    {
        private TestData _sourceData;
        private ICouchClient _client;
        private TestData _retrievedData;
        private IRestResponse<CouchResponse> _response;

        protected override void BeforeEach()
        {
            _client = Container.GetInstance<ICouchClient>();

            _sourceData = new TestData
                {
                    _id = "TestDataId1",
                    StringData = "SomeString",
                    BooleanData = true,
                    IntegerData = 5,
                    DoubleData = 6.5,
                    NestedData = new TestData
                        {
                            StringData = "SomeNestedString",
                            BooleanData = false,
                            IntegerData = 4,
                            DoubleData = 7.3
                        }
                };

            var requestSave = _client.Save(_sourceData);
            requestSave.Wait(1000).ShouldBeTrue();

            var requestGet = _client.Get<TestData>(_sourceData._id);
            requestGet.Wait(1000).ShouldBeTrue();
            _retrievedData = requestGet.Result.Data;

            _retrievedData.StringData = "UpdatedString";
            _retrievedData.BooleanData = false;
            _retrievedData.IntegerData = 10;
            _retrievedData.DoubleData = 100.56;
            _retrievedData.NestedData = new TestData
                {
                    StringData = "UpdatedNestedString",
                    BooleanData = true,
                    IntegerData = 15,
                    DoubleData = 16.777
                };

            requestSave = _client.Save(_retrievedData);
            requestSave.Wait(1000).ShouldBeTrue();

            requestSave = _client.Save(_sourceData);
            requestSave.Wait(1000).ShouldBeTrue();

            _response = requestSave.Result;
        }

        [Test]
        public void FailsToSave()
        {
            _response.StatusCode.ShouldEqual(HttpStatusCode.Conflict);
        }

        public class TestData : CouchDocument<TestData>
        {
            public string StringData { get; set; }
            public bool BooleanData { get; set; }
            public int IntegerData { get; set; }
            public double DoubleData { get; set; }
            public TestData NestedData { get; set; }
        }
    }
}