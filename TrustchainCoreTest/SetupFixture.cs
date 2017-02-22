using NUnit.Framework;
using TrustchainCore.Configuration;

namespace TrustchainCoreTest
{
    [SetUpFixture]
    public class SetupFixture
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Use in memory database
            App.Config["test"] = true; // Run as test, real timestamp not created!
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            //  all tests in the assembly have been run

        }
    }
}