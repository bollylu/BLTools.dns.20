using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

using BLTools.Diagnostic.Logging;

namespace BLTools.Storage.Xml {
  /// <summary>
  /// Handle reading/writing to/from an xml file
  /// </summary>
  public abstract class AFileXml : ALoggable, IFileXml {

    #region --- Public properties ------------------------------------------------------------------------------
    /// <summary>
    /// The name of the root in xml file
    /// </summary>
    public const string XML_ROOT_ELEMENT = "Root";

    /// <summary>
    /// Timeout for reading/writing the file content
    /// </summary>
    public const int TIMEOUT = 1000;

    /// <summary>
    /// A predefinition of the XDeclaration
    /// </summary>
    static protected XDeclaration _XmlDeclaration = new XDeclaration("1.0", "utf-8", "true");


    /// <inheritdoc/>
    public string Filename { get; protected set; }

#if NETSTANDARD2_0 || NETSTANDARD2_1
    /// <inheritdoc/>
    public Encoding FileEncoding { get; set; } = Encoding.UTF8;
#else
    /// <inheritdoc/>
    public Encoding FileEncoding { get; init; } = Encoding.UTF8;
#endif

    /// <inheritdoc/>
    public XElement Root { get; set; }
    #endregion --- Public properties ---------------------------------------------------------------------------

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    /// <summary>
    /// Create a new FileXml
    /// </summary>
    protected AFileXml() { }

    /// <summary>
    /// Create a new FileXml with a name
    /// </summary>
    /// <param name="filename">The filename for storing the data</param>
    protected AFileXml(string filename) {
      Filename = filename;
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    /// <inheritdoc/>
    public virtual XElement Load() {
      return Load(Filename);
    }

    /// <inheritdoc/>
    public virtual XElement Load(string filename) {
      _EnsureFilename(filename);
      _EnsureFileExists(filename);

      Filename = filename;

      try {
        using (FileStream InputStream = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.Read)) {
          XDocument XmlDoc = XDocument.Load(InputStream, LoadOptions.None);
          Root = XmlDoc.Root;
          return Root;
        }
      } catch (Exception ex) {
        LogError($"Unable to load xml content : {ex.Message}");
        return null;
      }
    }

    /// <inheritdoc/>
    public virtual async Task<XElement> LoadAsync() {
      return await LoadAsync(Filename);
    }

    /// <inheritdoc/>
    public virtual async Task<XElement> LoadAsync(string filename) {
      _EnsureFilename(filename);
      _EnsureFileExists(filename);

      Filename = filename;

#if NETSTANDARD2_0
      try {
        using (FileStream InputStream = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.Read)) {
          XDocument XmlDoc = XDocument.Load(InputStream, LoadOptions.None);
          await Task.Yield();
          Root = XmlDoc.Root;
          return Root;
        }
      } catch (Exception ex) {
        LogError($"Unable to load xml content : {ex.Message}");
        return null;
      }

#else
      try {
        using (FileStream InputStream = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.Read)) {
          XDocument XmlDoc = await XDocument.LoadAsync(InputStream, LoadOptions.None, new CancellationTokenSource(TIMEOUT).Token).ConfigureAwait(false);
          Root = XmlDoc.Root;
          return Root;
        }
      } catch (Exception ex) {
        LogError($"Unable to load xml content : {ex.Message}");
        return null;
      }
#endif
    }

    /// <inheritdoc/>
    public virtual bool Save(bool overwrite = true) {
      return Save(Filename, Root, overwrite);
    }

    /// <inheritdoc/>
    public virtual bool Save(string filename, bool overwrite = true) {
      return Save(filename, Root, overwrite);
    }

    /// <inheritdoc/>
    public virtual bool Save(XElement root, bool overwrite = true) {
      return Save(Filename, root, overwrite);
    }

    /// <inheritdoc/>
    public virtual bool Save(string filename, XElement root, bool overwrite = true) {
      _EnsureFilename(filename);
      _EnsureRoot(root);

      Filename = filename;
      Root = root;

      try {
        XDocument XmlDoc = new XDocument(_XmlDeclaration);
        XmlDoc.Add(new XElement(XML_ROOT_ELEMENT));
        XmlDoc.Root.Add(Root);
        using (FileStream OutputStream = new FileStream(Filename, FileMode.Create, FileAccess.Write, FileShare.None)) {
          XmlDoc.Save(OutputStream, SaveOptions.None);
          OutputStream.Flush();
        }
        return true;
      } catch (Exception ex) {
        LogError($"Unable to save parameter set : {ex.Message}");
        return false;
      }


    }

    /// <inheritdoc/>
    public virtual async Task<bool> SaveAsync(string filename, bool overwrite = true) {
      return await SaveAsync(filename, Root, overwrite).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<bool> SaveAsync(bool overwrite = true) {
      return await SaveAsync(Filename, Root, overwrite).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<bool> SaveAsync(XElement root, bool overwrite = true) {
      return await SaveAsync(Filename, root, overwrite).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<bool> SaveAsync(string filename, XElement root, bool overwrite = true) {
      _EnsureFilename(filename);
      _EnsureRoot(root);

      Filename = filename;
      Root = root;

#if NETSTANDARD2_0
      try {
        XDocument XmlDoc = new XDocument(_XmlDeclaration);
        XmlDoc.Add(new XElement(XML_ROOT_ELEMENT));
        XmlDoc.Root.Add(Root);
        using (FileStream OutputStream = new FileStream(Filename, FileMode.Create, FileAccess.Write, FileShare.None)) {
          XmlDoc.Save(OutputStream, SaveOptions.None);
          OutputStream.Flush();
          await Task.Yield();
        }
        return true;
      } catch (Exception ex) {
        LogError($"Unable to save parameter set : {ex.Message}");
        return false;
      }
#else
      try {
        XDocument XmlDoc = new XDocument(_XmlDeclaration);
        XmlDoc.Add(new XElement(XML_ROOT_ELEMENT));
        XmlDoc.Root.Add(Root);
        using (FileStream OutputStream = new FileStream(Filename, FileMode.Create, FileAccess.Write, FileShare.None)) {
          await XmlDoc.SaveAsync(OutputStream, SaveOptions.None, new CancellationTokenSource(TIMEOUT).Token).ConfigureAwait(false);
          await OutputStream.FlushAsync().ConfigureAwait(false);
        }
        return true;
      } catch (Exception ex) {
        LogError($"Unable to save parameter set : {ex.Message}");
        return false;
      }
#endif

    }

    /************************************************************************************/

    /// <summary>
    /// Ensure that the filename is passed
    /// </summary>
    /// <param name="filename"></param>
    protected void _EnsureFilename(string filename) {
      if (string.IsNullOrWhiteSpace(filename)) {
        const string MESSAGE = "Unable to work with file : filename is missing";
        LogError(MESSAGE);
        throw new ArgumentException(MESSAGE, nameof(filename));
      }
    }

    /// <summary>
    /// Ensure that the xml root is passed
    /// </summary>
    /// <param name="root">The root to validate</param>
    protected void _EnsureRoot(XElement root) {
      if (root is null) {
        const string MESSAGE = "Unable to work with xml data : root is missing";
        LogError(MESSAGE);
        throw new ArgumentException(MESSAGE, nameof(root));
      }
    }

    /// <summary>
    /// Ensure that the filename exists
    /// </summary>
    /// <param name="filename"></param>
    protected void _EnsureFileExists(string filename) {
      if (!File.Exists(filename)) {
        string Message = $"Unable to load file {filename} : filename is invalid or access is denied";
        LogError(Message);
        throw new ArgumentException(Message, nameof(filename));
      }
    }

    /************************************************************************************/
  }
}
