using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace BLTools.Diagnostic.Logging
{
    public class TFileLogger : ALogger
    {
        public string Filename { get; }

        private static readonly object _LockOutputStream = new object();

        #region --- Constructor(s) ---------------------------------------------------------------------------------
        public TFileLogger(string filename)
        {
            Filename = filename;
            _Initialize();
        }

        public TFileLogger(TFileLogger logger)
        {
            Filename = logger.Filename;
            SeverityLimit = logger.SeverityLimit;
            SourceWidth = logger.SourceWidth;
            _Initialize();
        }

        private bool _IsInitialized = false;

        protected override void _Initialize()
        {
            if ( _IsInitialized )
            {
                return;
            }

            base._Initialize();

            if ( string.IsNullOrWhiteSpace(Filename) )
            {
                throw new ArgumentException("Unable to create TFileLogger : filename is null or empty");
            }

            if ( !Directory.Exists(Path.GetDirectoryName(Filename)) )
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Filename));
            }

            _IsInitialized = true;
        }
        #endregion --- Constructor(s) ------------------------------------------------------------------------------

        public override void Log(string text, string source = "", ESeverity severity = ESeverity.Info)
        {
            #region === Validate parameters ===
            if ( string.IsNullOrEmpty(text) )
            {
                return;
            }

            if ( severity < SeverityLimit )
            {
                return;
            }
            #endregion === Validate parameters ===

            lock ( _LockOutputStream )
            {

                #region --- Ensure we can safely write --------------------------------------------
                DateTime StartTime = DateTime.Now;
                while ( _IsBusy && ( DateTime.Now - StartTime ).TotalMilliseconds < TIMEOUT )
                {
                    Thread.Sleep(1);
                }
                if ( _IsBusy )
                {
                    throw new TimeoutException($"Timeout writing to log file {Filename}");
                }
                #endregion --- Ensure we can safely write --------------------------------------------

                _IsBusy = true;
                using ( FileStream OutStream = new FileStream(Filename, FileMode.Append, FileAccess.Write, FileShare.ReadWrite) )
                {
                    using ( TextWriterTraceListener FileListener = new TextWriterTraceListener(OutStream) )
                    {
                        foreach ( string TextItem in text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries) )
                        {
                            FileListener.WriteLine(_BuildLogLine(TextItem, source, severity));
                        }
                        FileListener.Flush();
                        FileListener.Close();
                    }
                }
                _IsBusy = false;

            }
        }

        /// <summary>
        /// Delete the current log file.
        /// A new log file will be created the first time a new log line is added
        /// </summary>
        public void ResetLog()
        {
            lock ( _LockOutputStream )
            {
                try
                {
                    File.Delete(Filename);
                }
                catch ( Exception ex )
                {
                    Trace.WriteLine($"Unable to ResetLog for {Filename} : {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Will close the current log file, and rename it as oldfilename + datetime + .log.
        /// A new log file will be created the first time a new log line is added
        /// </summary>
        public string Rollover()
        {
            return Rollover($"{Path.GetFileNameWithoutExtension(Filename)}+{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")}{Path.GetExtension(Filename)}");
        }

        /// <summary>
        /// Will close the current log file, and rename it as newName.
        /// A new log file will be created the first time a new log line is added
        /// </summary>
        /// <param name="newName">Name of the renamed (only the filename+extension part, not the path) version</param>
        /// <returns>The name of the new log file or null if error</returns>
        public string Rollover(string newName)
        {
            #region Validate parameters
            if ( newName == null )
            {
                string Msg = $"Unable to rollover {Filename} because the new name is null";
                Trace.WriteLine(Msg, Severity.Fatal);
                throw new ArgumentNullException("newName", Msg);
            }
            #endregion Validate parameters

            lock ( _LockOutputStream )
            {
                try
                {
                    string Destination = Path.Combine(Path.GetDirectoryName(Filename), newName);
                    File.Move(Filename, Destination);
                    return Destination;
                }
                catch ( Exception ex )
                {
                    Trace.WriteLine($"Unable to Rollover for \"{Filename}\" to \"{newName}\" : {ex.Message}");
                    return null;
                }
            }
        }
    }
}
