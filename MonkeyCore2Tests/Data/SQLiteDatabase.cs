using NUnit.Framework;
using System.Collections.Generic;
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
            var FurreDataGero = new Dictionary<string, string>
            {
                { "Name", "gerolkae" },
                { "Access Level", "0" }
            };
            var FurreDataBill = new Dictionary<string, string>
            {
                { "Name", "bill" },
                { "Access Level", "3" }
            };
            var FurreDataJoe = new Dictionary<string, string>
            {
                { "Name", "joe" },
                { "Access Level", "3" }
            };
            var rowCountGero = database.Insert("FURRE", FurreDataGero);
            var rowCountBill = database.Insert("FURRE", FurreDataBill);
            var rowCountJoe = database.Insert("FURRE", FurreDataJoe);
            Assert.Multiple(() =>
            {
                Assert.That(rowCountGero > -1, $"Gero Rows affected {rowCountGero}");
                Assert.That(rowCountBill > -1, $"Bill Rows affected {rowCountBill}");
                Assert.That(rowCountJoe > -1, $"Joe Rows affected {rowCountJoe}");
            });
        }

        [Test]
        public void UpdateDataTo_FURRE_Table()
        {
            object result;
            var FurreTableDataRow = new Dictionary<string, string>
            {
                { "Name", "gerolkae" },
                { "Access Level", "222" }
            };
            var success = database.Update("FURRE", FurreTableDataRow, "Name == 'gerolkae'");
            Assert.Multiple(() =>
            {
                Assert.That(success == true, $"Update has '{success}'");
                result = database.ExecuteScalar("SELECT [Access Level] FROM FURRE WHERE Name='gerolkae'");
                Assert.That(result.ToString() == "222", $"Resut expected 222 but got '{result}'");
            });
            FurreTableDataRow = new Dictionary<string, string>
            {
                { "Name", "gerolkae" },
                { "Access Level", "5" }
            };
            success = database.Update("FURRE", FurreTableDataRow, "Name == 'gerolkae'");
            Assert.Multiple(() =>
            {
                Assert.That(success == true, $"Update has '{success}'");
                result = database.ExecuteScalar("SELECT [Access Level] FROM FURRE WHERE Name='gerolkae'");
                Assert.That(result.ToString() == "5", $"Resut expected 5 but got '{result}'");
            });
        }

        [Test]
        public void AddAndRemoveCollumnTest()
        {
            AddColumnTest("TestCollumn1");
            AddDataTo_FURRE_Table();
            RemoveColumnTest("TestCollumn1");
        }

        [Test]
        public void GetTableMetaDataTest()
        {
            var meta = database.GetAllCollumnNamesWithMetaData("FURRE");
            Assert.That(meta != null);
        }

        [Test]
        public void GetTableUniquekeysTest()
        {
            AddDataTo_FURRE_Table();
            var meta = database.GetTableUniqeKeys("FURRE");
            Assert.That(meta != null);
        }

        [Test]
        public void GetTablePrimarykeysTest()
        {
            var meta = database.GetTablePrimaryKeys("FURRE");
            Assert.That(meta != null);
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