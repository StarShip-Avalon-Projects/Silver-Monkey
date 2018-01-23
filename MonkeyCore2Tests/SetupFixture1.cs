using NUnit.Framework;
using static MonkeyCore2Tests.Data.DatabaseConfig;
using System.IO;
using MonkeyCore2.Data;

namespace MonkeyCore2Tests
{
    [SetUpFixture]
    public class SetupFixture1
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // TODO: Create Database
            database = new SQLiteDatabase(databaseFile);
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