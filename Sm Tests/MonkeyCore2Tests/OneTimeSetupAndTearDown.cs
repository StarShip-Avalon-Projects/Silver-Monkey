using NUnit.Framework;
using static MonkeyCoreTests.Data.DatabaseConfig;
using static MonkeyCoreTests.Utilities;
using System.IO;
using MonkeyCore.Data;
using Monkeyspeak.Logging;

namespace MonkeyCoreTests
{
    [SetUpFixture]
    public class OneTimeSetupAndTearDown
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // TODO: Create Database
            database = new SQLiteDatabase(databaseFile);
            SetLogger();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // TODO: delete Databae
            //if (File.Exists(database.DatabaseFile))
            //    File.Delete(database.DatabaseFile);
        }
    }
}