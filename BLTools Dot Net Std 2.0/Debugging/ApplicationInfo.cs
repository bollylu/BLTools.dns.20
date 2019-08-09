using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BLTools.Text;

namespace BLTools.Debugging {
  /// <summary>
  /// Helper for obtaining runtime infos
  /// </summary>
  public static partial class ApplicationInfo {

    /// <summary>
    /// Generate a banner with startup information and send it to Trace
    /// </summary>
    public static void ApplicationStart() {
      Trace.IndentSize = 2;
      Trace.AutoFlush = true;
      try {
        AssemblyName StartupApplication = Assembly.GetEntryAssembly().GetName();
        StringBuilder Text = new StringBuilder();
        Text.AppendLine($"Startup of {StartupApplication.Name}, {StartupApplication.ProcessorArchitecture}, {StartupApplication.Version.ToString()}");
        Text.AppendLine($".NET Runtime version: {Assembly.GetEntryAssembly().ImageRuntimeVersion}");
        Text.AppendLine($"BLTools version: {Assembly.GetExecutingAssembly().GetName().Version.ToString()}");
        Trace.WriteLine(TextBox.BuildFixedWidth(Text.ToString(), 80, TextBox.StringAlignmentEnum.Left));
      } catch { }
      ApplicationInfo.TraceRuntimeInfo();
    }
    /// <summary>
    /// Generate a banner to confirm that application is stopping and send it to Trace
    /// </summary>
    public static void ApplicationStop() {
      try {
        AssemblyName StartupApplication = Assembly.GetEntryAssembly().GetName();
        StringBuilder Text = new StringBuilder();
        Text.AppendLine($"{StartupApplication.Name} is stopping");
        Trace.WriteLine(TextBox.BuildFixedWidth(Text.ToString(), 80, TextBox.StringAlignmentEnum.Left));
        Trace.Flush();
      } catch { }
    }

    /// <summary>
    /// Send a banner with runtime info to Trace
    /// </summary>
    public static void TraceRuntimeInfo() {
      try {
        Trace.WriteLine(BuildRuntimeInfo());
      } catch { }
    }

    /// <summary>
    /// Builds a banner with runtime infos
    /// </summary>
    /// <returns></returns>
    public static string BuildRuntimeInfo() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendLine($"Working folder = \"{Environment.CurrentDirectory}\"");
      RetVal.AppendLine($"Operating system = \"{Environment.OSVersion.VersionString}\"");
      RetVal.AppendLine($"Current user = \"{Environment.UserName}\"" );
      RetVal.AppendLine($"Current domain user = \"{Environment.UserDomainName}\"");
      RetVal.AppendLine($"Is 64 bits OS = {Environment.Is64BitOperatingSystem}");
      return TextBox.BuildDynamic(RetVal.ToString(), 0, TextBox.StringAlignmentEnum.Left);

    }

  }
}
