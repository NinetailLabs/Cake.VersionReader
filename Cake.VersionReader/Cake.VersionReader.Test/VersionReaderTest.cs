using Cake.VersionReader.Tests.Fakes;
using NUnit.Framework;

namespace Cake.VersionReader.Tests
{
    [TestFixture]
    public class VersionReaderTest
    {
        private FakeCakeContext _context;

        [SetUp]
        public void Setup()
        {
            _context = new FakeCakeContext();
        }

        [Test]
        public void RetrieveVersionNumber()
        {
            var result = _context.CakeContext.GetVersionNumber("./TestData/SemVerTest.dll");
            Assert.That(result, Is.EqualTo("1.1.2"));
        }

        [Test]
        public void RetrieveBuildServerVersionNumber()
        {
            var result = _context.CakeContext.GetVersionNumberWithContinuesIntegrationNumberAppended("./TestData/SemVerTest.dll", 5);
            Assert.That(result, Is.EqualTo("1.1.2-CI00005"));
        }
    }
}