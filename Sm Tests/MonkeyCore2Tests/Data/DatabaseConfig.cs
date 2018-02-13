using MonkeyCore2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyCore2Tests.Data
{
    public class DatabaseConfig
    {
        public static SQLiteDatabase database;
        public const string databaseFile = "MonkeyCore2Tests.db";
    }
}