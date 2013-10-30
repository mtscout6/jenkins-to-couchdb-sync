using NUnit.Framework;
using sabatoast_puller.Couch;
using Should;

namespace sabatoast_puller.Tests.Couch
{
    public class CouchClientSaveAndGetTester : CouchDbInteractionContext
    {
        private TestData _sourceData;
        private ICouchClient _client;
        private TestData _retrievedData;

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
        }

        [Test]
        public void SavesDoc()
        {
            _sourceData._rev.ShouldNotBeEmpty();
        }

        [Test]
        public void RetrievesDocument()
        {
            _retrievedData.ShouldNotBeNull();
            _retrievedData.ShouldNotBeSameAs(_sourceData);
        }

        [Test]
        public void RetrievesStringValue()
        {
            _retrievedData.StringData.ShouldEqual(_sourceData.StringData);
        }

        [Test]
        public void RetrievesBooleanValue()
        {
            _retrievedData.BooleanData.ShouldEqual(_sourceData.BooleanData);
        }

        [Test]
        public void RetrievesIntegerValue()
        {
            _retrievedData.IntegerData.ShouldEqual(_sourceData.IntegerData);
        }

        [Test]
        public void RetrievesDoubleValue()
        {
            _retrievedData.DoubleData.ShouldEqual(_sourceData.DoubleData);
        }

        [Test]
        public void RetrievesNestedObject()
        {
            _retrievedData.NestedData.ShouldNotBeNull();
            _retrievedData.NestedData.ShouldNotBeSameAs(_sourceData.NestedData);
        }

        [Test]
        public void RetrievesNestedStringValue()
        {
            _retrievedData.NestedData.StringData.ShouldEqual(_sourceData.NestedData.StringData);
        }

        [Test]
        public void RetrievesNestedBooleanValue()
        {
            _retrievedData.NestedData.BooleanData.ShouldEqual(_sourceData.NestedData.BooleanData);
        }

        [Test]
        public void RetrievesNestedIntegerValue()
        {
            _retrievedData.NestedData.IntegerData.ShouldEqual(_sourceData.NestedData.IntegerData);
        }

        [Test]
        public void RetrievesNestedDoubleValue()
        {
            _retrievedData.NestedData.DoubleData.ShouldEqual(_sourceData.NestedData.DoubleData);
        }

        [Test]
        public void RetrievesNestedNull()
        {
            _retrievedData.NestedData.NestedData.ShouldBeNull();
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