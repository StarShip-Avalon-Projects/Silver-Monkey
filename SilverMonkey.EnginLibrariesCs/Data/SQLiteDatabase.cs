using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyCore2.Data;
using Monkeyspeak;

namespace SilverMonkey.EnginLibraries.Data
{
    /// <summary>
    /// Designed with <see cref="Monkeyspeak.VariableTable"/> in mind
    /// </summary>
    /// <seealso cref="MonkeyCore2.Data.SQLiteDatabase" />
    public class SQLiteDatabase : MonkeyCore2.Data.SQLiteDatabase
    {
        public SQLiteDatabase() : base()
        {
        }

        public SQLiteDatabase(string DatabaseFile) : base(DatabaseFile)
        {
        }

        public VariableTable GetVariableTableFromTable(string str)
        {
            throw new NotImplementedException();
        }

        public override int Insert(string tableName, Dictionary<string, string> data)
        {
            throw new NotImplementedException();
        }
    }
}