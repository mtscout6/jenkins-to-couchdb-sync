using System.Collections.Generic;
using NUnit.Framework;
using sabatoast_puller.Jenkins.Models;
using Should;

namespace sabatoast_puller.Tests.Jenkins.Models
{
    public class RootTester
    {
        [Test]
        public void NotEqualWithDifferentRevisions()
        {
            var root1 = new Root {_rev = "1"};
            var root2 = new Root {_rev = "2"};

            root1.Equals(root2).ShouldBeFalse();
        }

        [Test]
        public void ReferenceEquals()
        {
            var root = new Root {_rev = "1"};

            root.Equals(root).ShouldBeTrue();
        }

        [Test]
        public void EqualWithSameRevisions()
        {
            var root1 = new Root {_rev = "1"};
            var root2 = new Root {_rev = "1"};

            root1.Equals(root2).ShouldBeTrue();
        }

        [Test]
        public void EqualWithSameListValues()
        {
            var root1 = new Root
                {
                    _rev = "1",
                    Jobs = new List<RootJob>
                        {
                            new RootJob {Name = "Job1", Url = "Url1"},
                            new RootJob {Name = "Job2", Url = "Url2"}
                        }
                };

            var root2 = new Root
                {
                    _rev = "1",
                    Jobs = new List<RootJob>
                        {
                            new RootJob {Name = "Job1", Url = "Url1"},
                            new RootJob {Name = "Job2", Url = "Url2"}
                        }
                };

            root1.Equals(root2).ShouldBeTrue();
        }

        [Test]
        public void EqualWithSameListValuesIgnoreOrder()
        {
            var root1 = new Root
                {
                    _rev = "1",
                    Jobs = new List<RootJob>
                        {
                            new RootJob {Name = "Job1", Url = "Url1"},
                            new RootJob {Name = "Job2", Url = "Url2"}
                        }
                };

            var root2 = new Root
                {
                    _rev = "1",
                    Jobs = new List<RootJob>
                        {
                            new RootJob {Name = "Job2", Url = "Url2"},
                            new RootJob {Name = "Job1", Url = "Url1"}
                        }
                };

            root1.Equals(root2).ShouldBeTrue();
        }

        [Test]
        public void NotEqualWithDifferentListValues()
        {
            var root1 = new Root
                {
                    _rev = "1",
                    Jobs = new List<RootJob>
                        {
                            new RootJob {Name = "Job1", Url = "Url1"},
                            new RootJob {Name = "Job2", Url = "Url2"}
                        }
                };

            var root2 = new Root
                {
                    _rev = "1",
                    Jobs = new List<RootJob>
                        {
                            new RootJob {Name = "Job3", Url = "Url3"},
                            new RootJob {Name = "Job4", Url = "Url4"}
                        }
                };

            root1.Equals(root2).ShouldBeFalse();
        }
    }
}