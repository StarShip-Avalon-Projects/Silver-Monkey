using MonkeyCore2.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MonkeyCore2Tests.Data.DatabaseConfig;

namespace MonkeyCore2Tests.Data
{
    [TestFixture]
    public class SQLiteDatabaseTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        public void AddColumnTest(string ColumnName)
        {
            database.AddColumn("FURRE", ColumnName, "TEXT");
            Assert.IsTrue(database.IsColumnExist(ColumnName, "FURRE"));
        }

        public void AddDataTo_FURRE_Table()
        {
            var data = new Dictionary<string, string>
            {
                { "Name", "gerolkae" },
                { "Access Level", "0" }
            };
            var rowCount = database.Insert("FURRE", data);
            Assert.That(rowCount > 0, $"Rows affected {rowCount}");
        }

        [Test]
        public void UpdateDataTo_FURRE_Table()
        {
            object result;
            var data = new Dictionary<string, string>
            {
                { "Name", "gerolkae" },
                { "Access Level", "222" }
            };
            var success = database.Update("FURRE", data, "Name == 'gerolkae'");
            Assert.Multiple(() =>
            {
                Assert.That(success == true, $"Update has '{success}'");
                result = database.ExecuteScalar("SELECT [Access Level] FROM FURRE WHERE Name='gerolkae'");
                Assert.That(result.ToString() == "222", $"Resut expected 222 but got {result}");
            });
            data = new Dictionary<string, string>
            {
                { "Name", "gerolkae" },
                { "Access Level", "5" }
            };
            success = database.Update("FURRE", data, "Name == 'gerolkae'");
            Assert.Multiple(() =>
            {
                Assert.That(success == true, $"Update has '{success}'");
                result = database.ExecuteScalar("SELECT [Access Level] FROM FURRE WHERE Name='gerolkae'");
                Assert.That(result.ToString() == "5", $"Resut expected 5 but got {result}");
            });
        }

        [Test]
        public void AddAndRemoveCollumnTest()
        {
            AddColumnTest("TestCollumn1");
            AddDataTo_FURRE_Table();
            RemoveColumnTest("TestCollumn1");
        }

        public void RemoveColumnTest(string ColumnName)
        {
            var rowCount = database.RemoveColumn("FURRE", ColumnName);
            Assert.Multiple(() =>
            {
                Assert.IsFalse(database.IsColumnExist(ColumnName, "FURRE"));
                Assert.That(rowCount > 0, $"Rows affected {rowCount}");
            });
        }

        [TearDown]
        public void TearDown()
        {
        }
    }
}