using NUnit.Framework;
using sabatoast_puller.Couch;
using Should;

namespace sabatoast_puller.Tests.Couch
{
    public class CouchClientUpdateTester : CouchDbInteractionContext
    {
        private TestData _sourceData;
        private ICouchClient _client;
        private TestData _retrievedData1;
        private TestData _retrievedData2;

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
            _retrievedData1 = requestGet.Result;

            _retrievedData1.StringData = "UpdatedString";
            _retrievedData1.BooleanData = false;
            _retrievedData1.IntegerData = 10;
            _retrievedData1.DoubleData = 100.56;
            _retrievedData1.NestedData = new TestData
                {
                    StringData = "UpdatedNestedString",
                    BooleanData = true,
                    IntegerData = 15,
                    DoubleData = 16.777
                };

            requestSave = _client.Save(_retrievedData1);
            requestSave.Wait(1000).ShouldBeTrue();

            requestGet = _client.Get<TestData>(_sourceData._id);
            requestGet.Wait(1000).ShouldBeTrue();
            _retrievedData2 = requestGet.Result;
        }

        [Test]
        public void SavesDoc()
        {
            _retrievedData2._rev.ShouldEqual(_retrievedData1._rev);
            _retrievedData2._rev.ShouldNotEqual(_sourceData._rev);
        }

        [Test]
        public void RetrievesDocument()
        {
            _retrievedData2.ShouldNotBeNull();
            _retrievedData2.ShouldNotBeSameAs(_sourceData);
            _retrievedData2.ShouldNotBeSameAs(_retrievedData1);
        }

        [Test]
        public void RetrievesStringValue()
        {
            _retrievedData2.StringData.ShouldEqual(_retrievedData1.StringData);
        }

        [Test]
        public void RetrievesBooleanValue()
        {
            _retrievedData2.BooleanData.ShouldEqual(_retrievedData1.BooleanData);
        }

        [Test]
        public void RetrievesIntegerValue()
        {
            _retrievedData2.IntegerData.ShouldEqual(_retrievedData1.IntegerData);
        }

        [Test]
        public void RetrievesDoubleValue()
        {
            _retrievedData2.DoubleData.ShouldEqual(_retrievedData1.DoubleData);
        }

        [Test]
        public void RetrievesNestedObject()
        {
            _retrievedData2.NestedData.ShouldNotBeNull();
            _retrievedData2.NestedData.ShouldNotBeSameAs(_retrievedData1.NestedData);
        }

        [Test]
        public void RetrievesNestedStringValue()
        {
            _retrievedData2.NestedData.StringData.ShouldEqual(_retrievedData1.NestedData.StringData);
        }

        [Test]
        public void RetrievesNestedBooleanValue()
        {
            _retrievedData2.NestedData.BooleanData.ShouldEqual(_retrievedData1.NestedData.BooleanData);
        }

        [Test]
        public void RetrievesNestedIntegerValue()
        {
            _retrievedData2.NestedData.IntegerData.ShouldEqual(_retrievedData1.NestedData.IntegerData);
        }

        [Test]
        public void RetrievesNestedDoubleValue()
        {
            _retrievedData2.NestedData.DoubleData.ShouldEqual(_retrievedData1.NestedData.DoubleData);
        }

        [Test]
        public void RetrievesNestedNull()
        {
            _retrievedData2.NestedData.NestedData.ShouldBeNull();
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