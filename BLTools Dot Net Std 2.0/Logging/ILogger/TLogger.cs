using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using BLTools;

namespace BLTools.Diagnostic.Logging
{
    public abstract class TLogger : ILogger
    {

        /// <summary>
        /// Maximum amount of ms to wait for a log line to be written
        /// </summary>
        public static int TIMEOUT = 1000;

        #region --- Trace limit --------------------------------------------
        public const ESeverity DEFAULT_TRACE_LIMIT = ESeverity.Info;

        public static ESeverity GlobalTraceLimit
        {
            get
            {
                if ( _GlobalTraceLimit == ESeverity.Unknown )
                {
                    return DEFAULT_TRACE_LIMIT;
                }
                return _GlobalTraceLimit;
            }
            set
            {
                _GlobalTraceLimit = value;
            }
        }
        private static ESeverity _GlobalTraceLimit = ESeverity.Unknown;

        public ESeverity SeverityLimit
        {
            get
            {
                if ( _SeverityLimit == ESeverity.Unknown )
                {
                    return GlobalTraceLimit;
                }
                return _SeverityLimit;
            }
            set
            {
                lock ( _Lock )
                {
                    _SeverityLimit = value;
                }
            }
        }
        private ESeverity _SeverityLimit = ESeverity.Unknown;
        #endregion --- Trace limit -----------------------------------------

        #region --- Source width --------------------------------------------
        public const int DEFAULT_SOURCE_WIDTH = 35;

        public int SourceWidth
        {
            get
            {
                if ( _SourceWidth <= 0 )
                {
                    return DEFAULT_SOURCE_WIDTH;
                }
                return _SourceWidth;
            }
            set
            {
                lock ( _Lock )
                {
                    _SourceWidth = value;
                }
            }
        }
        private int _SourceWidth;

        public bool IncludeSource { get; set; } = true;
        #endregion --- Source width -----------------------------------------

        #region --- Date format --------------------------------------------
        public static string DEFAULT_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss:fff";

        /// <summary>
        /// How to format the date and time in the first column
        /// </summary>
        public string DateFormat
        {
            get
            {
                if ( string.IsNullOrWhiteSpace(_DateFormat) )
                {
                    return DEFAULT_DATE_FORMAT;
                }
                return _DateFormat;
            }
            set
            {
                lock ( _Lock )
                {
                    _DateFormat = value;
                }
            }
        }
        private string _DateFormat;

        public bool IncludeDateTime { get; set; } = true;
        #endregion --- Date format -----------------------------------------

        #region --- Constructor(s) ---------------------------------------------------------------------------------
        private bool _IsInitialized = false;

        protected virtual void _Initialize()
        {
            if ( _IsInitialized )
            {
                return;
            }
            Trace.AutoFlush = true;
            Trace.IndentSize = 2;
            Trace.UseGlobalLock = true;
            _IsInitialized = true;
        }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if ( !disposedValue )
            {
                if ( disposing )
                {
                    Listener?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
        #endregion --- Constructor(s) ------------------------------------------------------------------------------

        public TraceListener Listener { get; set; }

        protected bool _IsBusy;
        protected readonly object _Lock = new object();

        #region --- Log actions --------------------------------------------
        public virtual void LogError(string text, string source = "")
        {
            Log(text, source, ESeverity.Error);
        }

        public virtual void LogWarning(string text, string source = "")
        {
            Log(text, source, ESeverity.Warning);
        }

        public virtual void LogFatal(string text, string source = "")
        {
            Log(text, source, ESeverity.Fatal);
        }

        public virtual void LogDebug(string text, string source = "")
        {
            Log(text, source, ESeverity.Debug);
        }

        public virtual void LogDebugEx(string text, string source = "")
        {
            Log(text, source, ESeverity.DebugEx);
        }

        public virtual void Log(string text, string source = "", ESeverity severity = ESeverity.Info)
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

            lock ( _Lock )
            {

                #region --- Ensure we can safely write --------------------------------------------
                DateTime StartTime = DateTime.Now;
                while ( _IsBusy && ( DateTime.Now - StartTime ).TotalMilliseconds < TIMEOUT )
                {
                    Thread.Sleep(1);
                }
                if ( _IsBusy )
                {
                    throw new TimeoutException($"Timeout writing to log");
                }
                #endregion --- Ensure we can safely write --------------------------------------------

                _IsBusy = true;
                foreach ( string TextItem in text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries) )
                {
                    Listener.WriteLine(_BuildLogLine(TextItem, source, severity));
                }
                Listener.Flush();
                _IsBusy = false;
            }
        }
        #endregion --- Log actions --------------------------------------------

        protected virtual string _BuildLogLine(string text, string source = "", ESeverity severity = ESeverity.Info)
        {
            StringBuilder RetVal = new StringBuilder();

            if ( IncludeDateTime )
            {
                RetVal.Append($"{DateTime.Now.ToString(DateFormat)}|");
            }

            RetVal.Append($"{severity.ToString().PadRight(10, '.')}|");

            if ( IncludeSource )
            {
                if ( string.IsNullOrWhiteSpace(source) )
                {
                    RetVal.Append(new string('.', SourceWidth));
                }
                else
                {
                    RetVal.Append(source.PadRight(SourceWidth, '.').Left(SourceWidth));
                }
                RetVal.Append("|");
            }

            RetVal.Append(text);
            return RetVal.ToString();
        }

        public static ILogger Create(ILogger logger)
        {
            switch ( logger )
            {
                case TFileLogger FileLogger:
                    return new TFileLogger(FileLogger);
                case TConsoleLogger ConsoleLogger:
                    return new TConsoleLogger(logger);
                default:
                    return new TTraceLogger(logger);
            }
        }

        /// <summary>
        /// The default logger, when nothing is configured.
        /// </summary>
        public static ILogger DEFAULT_LOGGER => SYSTEM_LOGGER;

        /// <summary>
        /// When configured, can be used as a system wide logger
        /// </summary>
        public static ILogger SYSTEM_LOGGER
        {
            get
            {
                if ( _SYSTEM_LOGGER == null )
                {
                    _SYSTEM_LOGGER = new TTraceLogger();
                }
                return _SYSTEM_LOGGER;
            }
            set
            {
                _SYSTEM_LOGGER = value;
            }
        }
        private static ILogger _SYSTEM_LOGGER;

        

    }
}
