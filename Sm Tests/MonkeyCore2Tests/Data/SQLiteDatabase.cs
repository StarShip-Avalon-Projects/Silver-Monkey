using NUnit.Framework;
using System.Collections.Generic;
using static MonkeyCoreTests.Data.DatabaseConfig;
using static MonkeyCoreTests.Utilities;

namespace MonkeyCoreTests.Data
{
    [TestFixture]
    public class SQLiteDatabaseTests
    {
        [SetUp]
        public void Initialize()
        {
        }

        public void AddColumnTest(string ColumnName)
        {
            var linesAffected = database.AddColumn("FURRE", ColumnName, "TEXT");
            Assert.Multiple(() =>
            {
                Assert.IsTrue(database.IsColumnExist(ColumnName, "FURRE"));
                Assert.That(linesAffected, Is.GreaterThanOrEqualTo(0));
            });
        }

        public void AddDataTo_FURRE_Table()
        {
            var FurreDataGero = new Dictionary<string, object>
            {
                { "Name", "gerolkae" },
                { "Access Level", "0" }
            };
            var FurreDataBill = new Dictionary<string, object>
            {
                { "Name", "bill" },
                { "Access Level", "3" }
            };
            var FurreDataJoe = new Dictionary<string, object>
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

        [Test] //Create Tables for VariableTable data
        public void AddDataTo_Settings_Table()
        {
            var SettingsTableMasterAdmins = new Dictionary<string, object>
            {
                { "SettingsTable", "Admins" },
            };
            var SettingsTableMasterSettings = new Dictionary<string, object>
            {
                { "SettingsTable", "Settings" },
            };
            var SettingsTableMasterFurres = new Dictionary<string, object>
            {
                { "SettingsTable", "Furres" },
            };
            var rowCountSettingsTableMasterAdmins = database.Insert("SettingsTableMaster", SettingsTableMasterAdmins);
            var rowCountSettingsTableMasterSettings = database.Insert("SettingsTableMaster", SettingsTableMasterSettings);
            var rowCountSettingsTableMasterFurres = database.Insert("SettingsTableMaster", SettingsTableMasterFurres);

            Assert.Multiple(() =>
            {
                Assert.That(rowCountSettingsTableMasterAdmins > -1, $"Admins Rows affected {rowCountSettingsTableMasterAdmins}");
                Assert.That(rowCountSettingsTableMasterSettings > -1, $"Settings Rows affected {rowCountSettingsTableMasterSettings}");
                Assert.That(rowCountSettingsTableMasterFurres > -1, $"Joe Rows affected {rowCountSettingsTableMasterFurres}");
            });
        }

        [Test]
        public void UpdateDataTo_FURRE_Table()
        {
            object result;
            var FurreTableDataRow = new Dictionary<string, object>
            {
                { "Name", "gerolkae" },
                { "Access Level", "222" }
            };
            var success = 0 <= database.Update("FURRE", FurreTableDataRow, "Name == 'gerolkae'");
            Assert.Multiple(() =>
            {
                Assert.That(success == true, $"Update has '{success}'");
                result = database.ExecuteScalar("SELECT [Access Level] FROM FURRE WHERE Name='gerolkae'");
                Assert.That(result.ToString() == "222", $"Resut expected 222 but got '{result}'");
            });
            FurreTableDataRow = new Dictionary<string, object>
            {
                { "Name", "gerolkae" },
                { "Access Level", "5" }
            };
            success = 0 <= database.Update("FURRE", FurreTableDataRow, "Name == 'gerolkae'");
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
                Assert.That(rowCount >= 0, $"Rows affected {rowCount}");
            });
        }

        [TearDown]
        public void TearDown()
        {
        }
    }
}