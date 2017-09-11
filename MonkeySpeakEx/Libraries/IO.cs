﻿using System.Text;

namespace Monkeyspeak.Libraries
{
    /// <summary>
    /// IO Monkey Speak Library
    /// </summary>
    internal class IO : AbstractBaseLibrary
    {
        #region Public Constructors

        public IO()
        {
            // (1:200) and the file {...} exists,
            Add(new Trigger(TriggerCategory.Condition, 200), FileExists,
                "(1:200) and the file {...} exists,");

            // (1:201) and the file {...} does not exist,
            Add(new Trigger(TriggerCategory.Condition, 201), FileNotExists,
                "(1:201) and the file {...} does not exist,");

            // (1:202) and the file {...} can be read from,
            Add(new Trigger(TriggerCategory.Condition, 202), CanReadFile,
                "(1:202) and the file {...} can be read from,");

            // (1:203) and the file {...} can be written to,
            Add(new Trigger(TriggerCategory.Condition, 203), CanWriteFile,
                "(1:203) and the file {...} can be written to,");

            // (5:200) append {...} to file {...}.
            Add(new Trigger(TriggerCategory.Effect, 200), AppendToFile,
                "(5:200) append {...} to file {...}.");

            // (5:201) read from file {...} and put it into variable %Variable.
            Add(new Trigger(TriggerCategory.Effect, 201), ReadFileIntoVariable,
                "(5:201) read from file {...} and put it into variable %Variable.");

            // (5:202) delete file {...}.
            Add(new Trigger(TriggerCategory.Effect, 202), DeleteFile,
                "(5:202) delete file {...}.");

            //(5:203) create file {...}.
            Add(new Trigger(TriggerCategory.Effect, 203), CreateFile,
                "(5:203) create file {...}.");
        }

        #endregion Public Constructors

        #region Private Methods

        /// <summary>
        /// (5:200) append {...} to file {...}.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool AppendToFile(TriggerReader reader)
        {
            string data = reader.ReadString();
            string file = reader.ReadString();

            using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(file, true))
            {
                streamWriter.WriteLine(data);
            }

            return true;
        }

        /// <summary>
        /// (1:202) and the file {...} can be read from,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool CanReadFile(TriggerReader reader)
        {
            string file = reader.ReadString();
            try
            {
                using (var stream = System.IO.File.Open(file, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    return true;
                }
            }
            catch // (UnauthorizedAccessException ex)
            {
                return false;
            }
        }

        /// <summary>
        /// (1:203) and the file {...} can be written to,"
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool CanWriteFile(TriggerReader reader)
        {
            string file = reader.ReadString();
            try
            {
                using (var stream = System.IO.File.Open(file, System.IO.FileMode.Open, System.IO.FileAccess.Write))
                {
                    return true;
                }
            }
            catch // (UnauthorizedAccessException ex)
            {
                return false;
            }
        }

        /// <summary>
        /// (5:203) create file {...}.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool CreateFile(TriggerReader reader)
        {
            if (reader.PeekString() == false) return false;
            string file = reader.ReadString();
            System.IO.File.CreateText(file).Close();
            return true;
        }

        /// <summary>
        /// (5:202) delete file {...}.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool DeleteFile(TriggerReader reader)
        {
            if (reader.PeekString() == false) return false;
            string file = reader.ReadString();
            System.IO.File.Delete(file);
            return true;
        }

        /// <summary>
        /// (1:200) and the file {...} exists,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool FileExists(TriggerReader reader)
        {
            string file = (reader.PeekString()) ? reader.ReadString() : "";
            return System.IO.File.Exists(file);
        }

        /// <summary>
        /// (1:201) and the file {...} does not exist,
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool FileNotExists(TriggerReader reader)
        {
            return FileExists(reader) == false;
        }

        /// <summary>
        /// (5:201) read from file {...} and put it into variable %Variable.
        /// </summary>
        /// <param name="reader">
        /// <see cref="TriggerReader"/>
        /// </param>
        /// <returns>
        /// </returns>
        private bool ReadFileIntoVariable(TriggerReader reader)
        {
            string file = reader.ReadString();
            Variable var = reader.ReadVariable(true);
            StringBuilder sb = new StringBuilder();
            using (var stream = System.IO.File.Open(file, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using (var streamReader = new System.IO.StreamReader(stream))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                        sb.AppendLine(line);
                }
            }
            var.Value = sb.ToString();
            return true;
        }

        #endregion Private Methods
    }
}