using NUnit.Framework;
using static MonkeyCore2Tests.Data.DatabaseConfig;
using static MonkeyCore2Tests.Utilities;
using System.IO;
using MonkeyCore2.Data;
using Monkeyspeak.Logging;

namespace MonkeyCore2Tests
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