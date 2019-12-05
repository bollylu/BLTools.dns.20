using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace BLTools.Diagnostic.Logging
{
    public class TFileLogger : TLogger
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
    }
}
