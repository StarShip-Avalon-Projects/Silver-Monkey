using Libraries.Variables;
using Monkeyspeak;
using System;
using System.Collections.Generic;

namespace Libraries.Data
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
        public int Update(string tableName, IDictionary<string, object> Variable)
        {
            var data = new Dictionary<string, string>();
            string name = null;
            foreach (var variable in Variable)
            {
                data.Add(variable.Key.Substring(1), variable.Value.ToString());
                if (variable.Key.Substring(1) == "Name")
                    name = variable.Value.ToString();
            }

            return Update(tableName, data, $"Vame={name}");
        }

        ///// <summary>
        ///// Updates the specified table with a <see cref="Dictionary{string, object}"/>.
        ///// </summary>
        ///// <param name="tableName">Name of the table.</param>
        ///// <param name="Variable">The variable.</param>
        ///// <returns></returns>
        //public int Update(string tableName, Dictionary<string, object> Variable)
        //{
        //    var data = new Dictionary<string, string>();
        //    string name = null;
        //    foreach (var variable in Variable)
        //    {
        //        data.Add(variable.Key.Substring(1), variable.Value.ToString());
        //        if (variable.Key == "Name")
        //            name = variable.Value.ToString();
        //    }

        //    return Update(tableName, data, $"Vame={name}");
        //}
    }
}