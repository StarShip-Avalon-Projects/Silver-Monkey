using NUnit.Framework;
using System;
using static SmEngineTests.Utilities;
using FurcLog = Furcadia.Logging;
using MsLog = Monkeyspeak.Logging;

namespace SmEngineTests
{
    [SetUpFixture]
    public class OneTimeSetupAndTearDown
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            SetLogger();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // TODO: Add code here that is run after
            //  all tests in the assembly have been run
        }
    }
}