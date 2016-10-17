/*
* This file is part of TuryUtilCs, the free C# utility collection.
* Copyright (C) 2016 Till Fischer aka Turysaz
*
* This program is free software; you can redistribute it and/or modify
* it under the terms of the GNU General Public License v2 as published
* by the Free Software Foundation.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License along
* with this program; if not, write to the Free Software Foundation, Inc.,
* 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
*/

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuryUtilCs.Logger
{
    /// <summary>
    /// The logger is a singleton-like implementation. It shall be
    /// used to write debug messages. The Logger can be configured
    /// to write to the console and/or to a specific logfile.
    /// If the log shall be written to a file, the method Finish()
    /// must be called at the end of the program.
    /// </summary>
    public class Logger
    {
        #region singleton pattern
        /// <summary>
        /// Dictionary of all so far created Logger objects.
        /// </summary>
        private static Dictionary<string, Logger> _loggerList;

        /// <summary>
        /// Returns the default Logger object.
        /// </summary>
        /// <returns>default Logger</returns>
        public static Logger GetInstance()
        {
            return GetInstance("defaultLog");
        }

        /// <summary>
        /// Returns a specific Logger object. It it doesn't exist,
        /// it will be created.
        /// </summary>
        /// <param name="name">Name of the Logger to be returned.</param>
        /// <returns></returns>
        public static Logger GetInstance(string name)
        {
            Logger instance;

            // If there are not any Loggers yet (not even the
            // dictionary), create the dictionary and the 
            // desired Logger object.
            if (_loggerList == null)
            {
                _loggerList = new Dictionary<string, Logger>();
                instance = new Logger(name);
                _loggerList.Add(name, instance);
            }
            else
            {
                // If the requested Logger exist, return it!
                if (_loggerList.ContainsKey(name))
                {
                    instance = _loggerList[name];
                }
                // else create it! And then return it!
                else
                {
                    instance = new Logger(name);
                    _loggerList.Add(name, instance);
                }
            }

            return instance;
        }

        /// <summary>
        /// Remove a specific Logger object from the Dictionary.
        /// </summary>
        /// <param name="rem">The Logger object that shall be removed</param>
        protected static void Remove(Logger rem)
        {
            foreach(string k in _loggerList.Keys)
            {
                if(_loggerList[k] == rem)
                {
                    _loggerList.Remove(k);
                    return;
                }
            }
            throw new ArgumentException("No such logger instance!");
        }

        /// <summary>
        /// Finish all existing loggers.
        /// </summary>
        public static void FinishAll()
        {
            Logger[] allLoggers = _loggerList.Values.ToArray();
            foreach(Logger l in allLoggers)
            {
                l.Finish();
            }
        }
        #endregion

        /// <summary>
        /// List of all messages that have been thrown yet.
        /// </summary>
        protected List<string> _msgs = new List<string>();

        /// <summary>
        /// Timestamp of the logger's creation time.
        /// </summary>
        private static DateTime starttime = DateTime.Now;

        /// <summary>
        /// Determines if the log shall be written to a file.
        /// </summary>
        public bool LogToFile { get; set; }

        /// <summary>
        /// Determines if the log shall be written to the console.
        /// </summary>
        public bool LogToConsole { get; set; }

        /// <summary>
        /// Defines if the time stamps will be added to the log or not.
        /// </summary>
        public bool AddTimeStamps { get; set; }

        /// <summary>
        /// Path of the file that the log will be written
        /// if LogToFile is true.
        /// </summary>
        public string LogFilePath { get; set; }
        
        /// <summary>
        /// Creates a new logger.
        /// </summary>
        /// <param name="name">
        /// Name of the specific logger.
        /// Will be used to set the path of it's log file.
        /// </param>
        protected Logger(string name)
        {
            // default settings, please change after creation of the logger
            string folder = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            this.LogFilePath = folder + @"log_" + name + ".txt";
            this.LogToConsole = false;
            this.LogToFile = true;
            this.AddTimeStamps = true;
        }

        /// <summary>
        /// Create string that contains the time elapsed since starttime
        /// in seconds and surrounded by square brackets (like the linux kernel)
        /// </summary>
        /// <returns>elapsed time in milliseconds. E.g. "[3.1415926535]" after pi seconds</returns>
        protected string RuntimeString()
        {
            TimeSpan runtime = DateTime.Now - starttime;
            return ("[" + runtime.TotalSeconds.ToString().PadLeft(12) + "]");
        }

        /// <summary>
        /// Writes a message to the log.
        /// </summary>
        /// <param name="msg">The message, that shall be written
        /// to the log</param>
        public void Log(object msg)
        {
            string logstring = msg.ToString();

            if (AddTimeStamps)
            {
                logstring = RuntimeString() + " " + logstring;
            }
            _msgs.Add(logstring);

            if (LogToConsole)
            {
                Console.WriteLine(logstring);
            }
        }
        
        /// <summary>
        /// Appends a message to the previous log message.
        /// </summary>
        /// <param name="msg">The message, that shall be
        /// appended to the previous message.</param>
        public void LogAppend(object msg)
        {
            string appendix = msg.ToString();
            if (AddTimeStamps)
            {
                appendix += " (" + RuntimeString() + ")";
            }

            _msgs[_msgs.Count - 1] += appendix;

            if (LogToConsole)
            {
                Console.WriteLine(appendix);
            }

        }

        /// <summary>
        /// Writes all messages to the specific log file, if
        /// LogToFile is set to true.
        /// </summary>
        public void Finish()
        {
            if (LogToFile)
            {
                WriteToFile();
            }
            Logger.Remove(this);
        }

        /// <summary>
        /// Writes all messages to a file.
        /// </summary>
        protected void WriteToFile()
        {
            System.IO.File.WriteAllLines(LogFilePath, _msgs.ToArray());
        }
    }
}
