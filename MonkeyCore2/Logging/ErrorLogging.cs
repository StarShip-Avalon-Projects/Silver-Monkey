using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

// Imports SilverMonkey.BugTraqConnect.Libs

namespace Logging
{
    /// <summary>
    ///Error Logging Class
    ///<para>Author: Tim Wilson</para>
    ///<para>Created: Sept 10, 2011</para>
    ///<para>Updated and maintained by Gerolkae</para>
    ///<para>To Call Class </para>
    ///<para>Example of calling Custom Error Logging Code</para>
    ///<example>
    ///   Try
    ///        Throw New Exception("This is an example exception demonstrating the Error
    ///Logging Exception Routine") 'Don't require this... this is just manually throwing an error
    /// to demo the class, actual code you'd just have the try/catch
    ///    Catch ex As Exception
    ///        Dim logError As New ErrorLogging(ex, Me) 'Passes the new constructor in the Error Logging Class the exception and 'me' (the calling object)
    ///    End Try</example>
    ///<para>To provide more details for individual object types create a new constructor by copying and pasting one below and then adjusting the argument. </para>
    /// </summary>
    ///
    public class ErrorLogging
    {
        private string strErrorFilePath;

        /// <summary>
        /// </summary>
        /// <param name="Ex">
        /// </param>
        /// <param name="ObjectThrowingError">
        /// </param>
        public ErrorLogging(Exception Ex, object ObjectThrowingError)
        {
            // Call Log Error
            // CHANGE FILEPATH/STRUCTURE HERE TO CHANGE FILE NAME & SAVING LOCATION
            strErrorFilePath = Path.Combine(IO.Paths.SilverMonkeyErrorLogPath, $"{ Assembly.GetExecutingAssembly().GetName().Name } _Error_ {DateTime.Now.ToString("MM_dd_yyyy_H-mm-ss") }.txt");
            LogError(Ex, ObjectThrowingError.ToString());
        }

        /// <summary>
        /// Fullpath the error document was written to.
        /// </summary>
        /// <returns>
        /// </returns>
        public string LogFile
        {
            get
            {
                return strErrorFilePath;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="ex">
        /// </param>
        /// <param name="ObjectThrowingError">
        /// </param>
        public void LogError(Exception ex, object ObjectThrowingError)
        {
            //   BugReport = New ProjectReport
            using (var LogFile = new System.IO.StreamWriter(strErrorFilePath, false))
            {
                // ***********************************************************
                // * Error Log Formatting
                // ***********************************************************
                LogFile.WriteLine("-------------------------------------------------------");
                LogFile.WriteLine("");
                LogFile.WriteLine($"{ Assembly.GetExecutingAssembly().FullName} Error Report");
                LogFile.WriteLine($"{Assembly.GetExecutingAssembly().GetName().Version.ToString()} Product Version");
                LogFile.WriteLine("");
                LogFile.WriteLine($"Date: { DateTime.Now.ToString("d")}");
                LogFile.WriteLine("");
                LogFile.WriteLine("System Details");
                LogFile.WriteLine("-------------------------------------------------------");
                LogFile.WriteLine($"Windows Version:  { Environment.OSVersion}");
                //   LogFile.WriteLine($"Version Number: { My.Computer.Info.OSVersion}");
                // Determine if 64-bit
                if (Environment.Is64BitOperatingSystem)
                {
                    LogFile.WriteLine("64-Bit OS");
                }
                else
                {
                    LogFile.WriteLine("32-Bit OS");
                }

                if ((Environment.Is64BitProcess == true))
                {
                    LogFile.WriteLine("64-Bit Process");
                }
                else
                {
                    LogFile.WriteLine("32-Bit Process");
                }

                LogFile.WriteLine("");
                LogFile.WriteLine($"Program Location: { IO.Paths.ApplicationPath}");
                LogFile.WriteLine("");
                LogFile.WriteLine("");
                LogFile.WriteLine("Error Details");
                LogFile.WriteLine("-------------------------------------------------------");
                LogFile.WriteLine(("Error: " + ex.Message));
                LogFile.WriteLine("");
                if (!(ex.InnerException == null))
                {
                    LogFile.WriteLine(("Inner Error: " + ex.InnerException.Message));
                    LogFile.WriteLine("");
                }

                LogFile.WriteLine($"Source: { ObjectThrowingError.ToString()}");
                LogFile.WriteLine("");
                StackTrace st = new StackTrace(ex, true);
                LogFile.WriteLine("-------------------------------------------------------");
                LogFile.WriteLine(("Stack Trace: " + st.ToString()));
                LogFile.WriteLine("");
                LogFile.WriteLine("-------------------------------------------------------");
                if (!(ex.InnerException == null))
                {
                    StackTrace stInner = new StackTrace(ex.InnerException, true);
                    LogFile.WriteLine(("Inner Stack Trace: " + stInner.ToString()));
                    LogFile.WriteLine("");
                    LogFile.WriteLine("-------------------------------------------------------");
                }
            }

            // ***********************************************************
        }
    }
}