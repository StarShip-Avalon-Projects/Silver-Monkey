using Monkeyspeak;
using System;
using System.Collections.Generic;

namespace Engine.Libraries.Data
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

        /// <summary>
        /// Inserts the specified VariableTable into the specified table
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="Variable">The variable.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Insert(string tableName, VariableTable Variable)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Upates the specified table name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="Variable">The variable.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Upate(string tableName, VariableTable Variable)
        {
            throw new NotImplementedException();
        }
    }
}