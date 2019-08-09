using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace BLTools.Diagnostic.Logging {
    public class TFileLogger : TLogger
    {
        public string Filename { get; }

        private static readonly object _LockOutputStream = new object();

        #region --- Constructor(s) ---------------------------------------------------------------------------------
        public TFileLogger(string filename) : base()
        {
            Filename = filename;
            _CheckParameters();
        }

        public TFileLogger(TFileLogger logger) : base(logger)
        {
            Filename = logger.Filename;
            _CheckParameters();
        }
        #endregion --- Constructor(s) ------------------------------------------------------------------------------

        private void _CheckParameters()
        {
            if (string.IsNullOrWhiteSpace(Filename))
            {
                throw new ArgumentException("Unable to create TFileLogger : filename is null or empty");
            }

            if (!Directory.Exists(Path.GetDirectoryName(Filename)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Filename));
            }
        }

        public override void Log(string text, string source = "", ESeverity severity = ESeverity.Info)
        {
            if (text == null)
            {
                return;
            }

            if (severity >= SeverityLimit)
            {
                lock (_LockOutputStream)
                {
                    while (_IsBusy)
                    { Thread.Sleep(1); }
                    _IsBusy = true;

                    using (FileStream OutStream = new FileStream(Filename, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    {
                        using (TextWriterTraceListener Listener = new TextWriterTraceListener(OutStream))
                        {
                            foreach (string TextItem in text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                            {
                                Listener.WriteLine(_BuildLogLine(TextItem, source, severity));
                            }
                            Listener.Flush();
                            Listener.Close();
                        }
                    }
                    _IsBusy = false;
                }
            }
        }
    }
}
