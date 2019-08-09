using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using BLTools;

namespace BLTools.Diagnostic.Logging {
    public abstract class TLogger : ILogger
    {
        #region --- Trace limit --------------------------------------------
        public const ESeverity DEFAULT_TRACE_LIMIT = ESeverity.Info;

        public static ESeverity GlobalTraceLimit
        {
            get
            {
                if (_GlobalTraceLimit == ESeverity.Unknown)
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
                if (_SeverityLimit == ESeverity.Unknown)
                {
                    return GlobalTraceLimit;
                }
                return _SeverityLimit;
            }
            set
            {
                lock (_Lock)
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
                if (_SourceWidth <= 0)
                {
                    return DEFAULT_SOURCE_WIDTH;
                }
                return _SourceWidth;
            }
            set
            {
                lock (_Lock)
                {
                    _SourceWidth = value;
                }
            }
        }
        private int _SourceWidth;
        #endregion --- Source width -----------------------------------------

        #region --- Constructor(s) ---------------------------------------------------------------------------------
        public TLogger()
        {
            _Initialize();
        }
        public TLogger(ILogger logger)
        {
            _Initialize();
            SeverityLimit = logger.SeverityLimit;
            SourceWidth = logger.SourceWidth;
        }

        protected virtual void _Initialize()
        {
            Trace.AutoFlush = true;
            Trace.IndentSize = 2;
            Trace.UseGlobalLock = true;
        }
        public void Dispose()
        {
            if (Listener != null)
            {
                Listener.Dispose();
            }
        }
        #endregion --- Constructor(s) ------------------------------------------------------------------------------

        public TraceListener Listener { get; set; }

        protected bool _IsBusy;
        protected readonly object _Lock = new object();

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
            if (text == null)
            {
                return;
            }

            if (severity >= SeverityLimit)
            {
                lock (_Lock)
                {
                    while (_IsBusy)
                    { Thread.Sleep(1); }
                    _IsBusy = true;
                    foreach (string TextItem in text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                    {
                        Listener.WriteLine(_BuildLogLine(TextItem, source, severity));
                    }
                    Listener.Flush();
                    _IsBusy = false;
                }
            }
        }

        protected string _BuildLogLine(string text, string source = "", ESeverity severity = ESeverity.Info)
        {
            StringBuilder RetVal = new StringBuilder();

            RetVal.Append($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}");
            RetVal.Append($"|{severity.ToString().PadRight(10, '.')}");
            RetVal.Append("|");
            if (string.IsNullOrWhiteSpace(source))
            {
                RetVal.Append(new string('.', SourceWidth));
            }
            else
            {
                RetVal.Append(source.PadRight(SourceWidth, '.').Left(SourceWidth));
            }
            RetVal.Append("|");
            RetVal.Append(text);
            return RetVal.ToString();
        }


        public static ILogger Create(ILogger logger)
        {
            switch (logger)
            {
                case TFileLogger FileLogger:
                    return new TFileLogger(FileLogger);
                case TConsoleLogger ConsoleLogger:
                    return new TConsoleLogger(logger);
                default:
                    return new TTraceLogger(logger);
            }
        }

        public static ILogger DEFAULT_LOGGER => new TTraceLogger();
        public static ILogger SYSTEM_LOGGER
        {
            get
            {
                if (_SYSTEM_LOGGER == null)
                {
                    return DEFAULT_LOGGER;
                }
                return TLogger.Create(_SYSTEM_LOGGER);
            }
            set
            {
                _SYSTEM_LOGGER = value;
            }
        }
        private static ILogger _SYSTEM_LOGGER;

    }
}
