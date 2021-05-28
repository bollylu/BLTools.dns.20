using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using BLTools.Diagnostic.Logging;
using BLTools.Text;

namespace BLTools.Debugging {
  /// <summary>
  /// Helper for obtaining runtime infos
  /// </summary>
  public static partial class ApplicationInfo {

    /// <summary>
    /// Generate a banner with startup information and send it to Logger
    /// </summary>
    [Obsolete("Use ApplicationStart(ILogger logger)")]
    public static void ApplicationStart() {
      ApplicationStart(null);
    }

    /// <summary>
    /// Generate a banner with startup information and send it to Logger
    /// </summary>
    public static void ApplicationStart(ILogger logger) {

      if (logger is null) {
        Trace.IndentSize = 2;
        Trace.AutoFlush = true;
        Trace.WriteLine(BuildStartupInfo().Box("Startup info"));
        Trace.WriteLine(BuildRuntimeInfo().Box("Runtime info"));
      } else {
        logger.Log(BuildStartupInfo().Box("Startup info"));
        logger.Log(BuildRuntimeInfo().Box("Runtime info"));
      }
    }

    /// <summary>
    /// Generate a banner to confirm that application is stopping and send it to Trace
    /// </summary>
    /// <param name="message">An optional message</param>
    public static void ApplicationStop(string message = "") {
      ApplicationStop(null, message);
    }

    /// <summary>
    /// Log a message indicating the application if completed
    /// </summary>
    /// <param name="logger">The logger for the message</param>
    /// <param name="message">An optional message</param>
    public static void ApplicationStop(ILogger logger, string message = "") {
      AssemblyName StartupApplication = null;
      StringBuilder Text = new StringBuilder();

      try {
#if NETSTANDARD2_0 || NETSTANDARD2_1
        StartupApplication = Assembly.GetExecutingAssembly().GetName();
#else
        StartupApplication = Assembly.GetEntryAssembly().GetName();
#endif

      } catch (Exception ex) {
        Text.AppendLine($"Unable to gather application information : {ex.Message}");
      }

      Text.AppendLine($"{StartupApplication?.Name} is stopping");
      if (!string.IsNullOrWhiteSpace(message)) {
        Text.AppendLine(message);
      }
      string Message = Text.ToString().TrimEnd(Environment.NewLine.ToCharArray());

      if (logger is null) {
        Trace.WriteLine(Message.Box(80));
        Trace.Flush();
      } else {
        logger.Log(Message.Box(80));
      }

    }

    /// <summary>
    /// Send a banner with runtime info to Trace
    /// </summary>
    [Obsolete]
    public static void TraceRuntimeInfo() {
      try {
        Trace.WriteLine(BuildRuntimeInfo());
      } catch { }
    }

    /// <summary>
    /// List of prefixes to ignore when asking for list of referenced assemblies
    /// </summary>
    public static List<string> ExcludedAssemblies = new List<string>() {
      "System",
      "Microsoft",
      "netstandard",
      "Newtonsoft.json",
      "Nuget"
    };

    /// <summary>
    /// Builds a string with startup information
    /// </summary>
    /// <returns>Startup info</returns>
    public static string BuildStartupInfo(bool fullDetails = false) {
      StringBuilder Text = new StringBuilder();
      try {

#if NETSTANDARD2_0 || NETSTANDARD2_1
        AssemblyName StartupApplication = Assembly.GetExecutingAssembly().GetName();
        Text.AppendLine($"Startup of {StartupApplication.Name}, {StartupApplication.ProcessorArchitecture}, {StartupApplication.Version.ToString()}");
        Text.AppendLine($".NET Runtime version: {Assembly.GetExecutingAssembly().ImageRuntimeVersion}");
        Text.AppendLine("Assemblies:");
#else
        AssemblyName StartupApplication = Assembly.GetEntryAssembly().GetName();
        Text.AppendLine($"Startup of {StartupApplication.Name}, {StartupApplication.ProcessorArchitecture}, {StartupApplication.Version.ToString()}");
        Text.AppendLine($".NET Runtime version: {Assembly.GetEntryAssembly().ImageRuntimeVersion}");
        Text.AppendLine("Assemblies:");
#endif

        AppDomain Domain = AppDomain.CurrentDomain;

        IEnumerable<Assembly> Assemblies;

        if (fullDetails) {
          Assemblies = Domain.GetAssemblies()
                             .OrderBy(a => a.GetName().Name);
        } else {
          Assemblies = Domain.GetAssemblies()
                             .Where(a => !ExcludedAssemblies.Any(x => a.GetName().Name.StartsWith(x, StringComparison.InvariantCultureIgnoreCase)))
                             .OrderBy(a => a.GetName().Name);
        }

        foreach (Assembly AssemblyItem in Assemblies) {
          Text.AppendLine($"+ {AssemblyItem.GetName().Name} - {AssemblyItem.GetName().Version}");
          string AssembliesList = GetReferencedAssembliesList(AssemblyItem);
          if (AssembliesList.Trim().Any()) {
            Text.AppendLine(AssembliesList);
          }
        }
      } catch (Exception ex) {
        Text.AppendLine($"Unable to gather startup information : {ex.Message}");
      }
      return Text.ToString().TrimEnd(Environment.NewLine.ToCharArray());
    }

    /// <summary>
    /// Get a list of referenced assemblies, filtered or not
    /// </summary>
    /// <param name="assembly">The parent assembly</param>
    /// <param name="fullDetails">true to obtain full list, false for a filtered list (default is false)</param>
    /// <returns></returns>
    public static string GetReferencedAssembliesList(Assembly assembly, bool fullDetails = false) {
      StringBuilder RetVal = new StringBuilder();

      try {

        IEnumerable<AssemblyName> AssemblyNames;

        if (fullDetails) {
          AssemblyNames = assembly.GetReferencedAssemblies()
                                  .OrderBy(a => a.Name);
        } else {
          AssemblyNames = assembly.GetReferencedAssemblies()
                                  .Where(a => !ExcludedAssemblies.Any(x => a.Name.StartsWith(x, StringComparison.InvariantCultureIgnoreCase)))
                                  .OrderBy(a => a.Name);
        }

        foreach (AssemblyName AssemblyItem in AssemblyNames) {
          RetVal.AppendLine($"  - {AssemblyItem.Name} - {AssemblyItem.Version}");
        }
      } catch (Exception ex) {
        RetVal.AppendLine($"Unable to gather startup information : {ex.Message}");
      }
      return RetVal.ToString().TrimEnd();
    }

    /// <summary>
    /// Builds a banner with runtime infos
    /// </summary>
    /// <returns></returns>
    public static string BuildRuntimeInfo() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendLine($"Working folder = \"{Environment.CurrentDirectory}\"");
      RetVal.AppendLine($"Operating system = \"{Environment.OSVersion.VersionString}\"");
      RetVal.AppendLine($"Current user = \"{Environment.UserName}\"");
      RetVal.AppendLine($"Current domain user = \"{Environment.UserDomainName}\"");
      RetVal.AppendLine($"Is 64 bits OS = {Environment.Is64BitOperatingSystem}");
      return TextBox.BuildDynamic(RetVal.ToString().TrimEnd(Environment.NewLine.ToCharArray()), 0, TextBox.EStringAlignment.Left);

    }

  }
}
