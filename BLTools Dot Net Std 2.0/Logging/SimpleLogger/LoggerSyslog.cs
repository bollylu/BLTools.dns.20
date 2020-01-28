using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BLTools
{
    [Obsolete("Use ILogger instead")]
    public class LoggerSyslog : ISimpleLogger, IDisposable
    {

        private const int DEFAULT_SYSLOG_PORT = 514;
        private const int MESSAGE_BLOCK_LENGTH = 950;

        public string Name { get; set; }

        public IPAddress TargetIP
        {
            get
            {
                return _TargetIP;
            }
            set
            {
                _TargetIP = value;
                TargetIPEndPoint = new IPEndPoint(_TargetIP, _TargetPort);
                Name = $"{_TargetIP.ToString()}:{_TargetPort}";
            }
        }
        private IPAddress _TargetIP;

        public int TargetPort
        {
            get
            {
                return _TargetPort;
            }
            set
            {
                if ( value < 1 || value > 65535 )
                {
                    Log.Write($"Syslog port is out of range : {value} : default value 514 is used instead", ErrorLevel.Warning);
                    _TargetPort = 514;
                }
                else
                {
                    _TargetPort = value;
                }
                TargetIPEndPoint = new IPEndPoint(_TargetIP, _TargetPort);
                Name = $"{_TargetIP.ToString()}:{_TargetPort}";
            }
        }
        private int _TargetPort;


        private readonly UdpClient CurrentUdpClient = new UdpClient();
        private IPEndPoint TargetIPEndPoint;

        #region --- Constructor(s) ---------------------------------------------------------------------------------
        public LoggerSyslog()
        {
            TargetIP = IPAddress.Loopback;
            TargetPort = DEFAULT_SYSLOG_PORT;
            CurrentUdpClient = new UdpClient();
        }

        public LoggerSyslog(int targetPort)
          : this()
        {
            TargetPort = targetPort;
        }

        public LoggerSyslog(string hostname)
          : this()
        {
            TargetIP = Dns.GetHostAddresses(hostname).First(i => i.AddressFamily == AddressFamily.InterNetwork);
        }

        public LoggerSyslog(string hostname, int targetPort)
          : this()
        {
            TargetPort = targetPort;
            TargetIP = Dns.GetHostAddresses(hostname).First(i => i.AddressFamily == AddressFamily.InterNetwork);
        }
        public LoggerSyslog(IPAddress targetAddress)
          : this()
        {
            TargetIP = targetAddress;
        }

        public LoggerSyslog(IPAddress targetAddress, int targetPort)
          : this()
        {
            TargetPort = targetPort;
            TargetIP = targetAddress;
        }

        public void Dispose()
        {
            if ( CurrentUdpClient != null )
            {
                CurrentUdpClient.Close();
            }
        }
        #endregion --- Constructor(s) ------------------------------------------------------------------------------

        public void Write(string message)
        {
            WriteSyslog(message);
        }

        public void Write(string message, ErrorLevel severity)
        {
            WriteSyslog(message, 6);
        }

        private void WriteSyslog(string textLine, int syslogSeverity = 6, int syslogFacility = 1)
        {
            #region === Validate parameters ===
            if ( string.IsNullOrEmpty(textLine) )
            {
                return;
            }
            #endregion === Validate parameters ===

            int PRI = syslogFacility * 8 + syslogSeverity;

            #region --- Header --------------------------------------------
            StringBuilder sbHeader = new StringBuilder();
            sbHeader.Append($"<{PRI.ToString("0##")}>");
            string CurrentDateTime = DateTime.Now.ToString("MMM dd HH: mm:ss", CultureInfo.GetCultureInfo("en - us").DateTimeFormat);
            sbHeader.Append($"{CurrentDateTime} ");
            sbHeader.Append($"{Dns.GetHostName().ToLower()} ");
            #endregion --- Header --------------------------------------------

            while ( textLine.Length > 0 )
            {

                //if ( textLine.IndexOf(' ') > 32 || textLine.IndexOf(' ') < 0 ) {
                //  textLine = textLine.Left(32) + " " + textLine.Substring(32);
                //}

                string sBuffer = string.Concat(sbHeader.ToString(), textLine.Substring(0, Math.Min(textLine.Length, MESSAGE_BLOCK_LENGTH)));
                byte[] bBuffer = Encoding.ASCII.GetBytes(sBuffer);
                //Buffer.BlockCopy(sbHeader., 0, bBuffer, 0, sbHeader.Length);

                CurrentUdpClient.Send(bBuffer, bBuffer.Length, TargetIPEndPoint);

                if ( textLine.Length > MESSAGE_BLOCK_LENGTH )
                {
                    textLine = textLine.Substring(MESSAGE_BLOCK_LENGTH);
                }
                else
                {
                    textLine = "";
                }
            }

        }


    }
}
