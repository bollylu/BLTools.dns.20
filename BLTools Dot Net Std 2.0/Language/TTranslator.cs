using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using BLTools.Diagnostic.Logging;
using BLTools.Text;

namespace BLTools.Language {
  public class TTranslator : TLoggable, ITranslator {
    #region --- XML constants --------------------------------------------
    public const string XML_MESSAGE_ELEMENT = "Message";
    public const string XML_MESSAGE_ATTRIBUTE_ID = "Id";
    public const string XML_TRANSLATION_ELEMENT = "Translation";
    public const string XML_TRANSLATION_ATTRIBUTE_CULTURE = "Culture";
    #endregion --- XML constants -----------------------------------------

    #region --- Culture --------------------------------------------
    public readonly static CultureInfo DEFAULT_CULTURE = CultureInfo.CurrentCulture;
    public readonly static CultureInfo DEFAULT_FALLBACK_CULTURE = CultureInfo.InvariantCulture;

    public static CultureInfo GlobalCulture {
      get {
        if (_GlobalCulture == null) {
          return DEFAULT_CULTURE;
        }
        return _GlobalCulture;
      }
      set {
        _GlobalCulture = value;
      }
    }
    private static CultureInfo _GlobalCulture;

    public CultureInfo CurrentCulture {
      get {
        if (_CurrentCulture == null) {
          return GlobalCulture;
        }
        return _CurrentCulture;
      }
      set {
        _CurrentCulture = value;
      }
    }
    private CultureInfo _CurrentCulture;

    public static CultureInfo GlobalFallbackCulture {
      get {
        if (_GlobalFallbackCulture == null) {
          return DEFAULT_CULTURE;
        }
        return _GlobalFallbackCulture;
      }
      set {
        _GlobalFallbackCulture = value;
      }
    }
    private static CultureInfo _GlobalFallbackCulture;

    public CultureInfo FallbackCulture {
      get {
        if (_FallbackCulture == null) {
          return DEFAULT_FALLBACK_CULTURE;
        }
        return _FallbackCulture;
      }
      set {
        _FallbackCulture = value;
      }
    }
    private CultureInfo _FallbackCulture;
    #endregion --- Culture -----------------------------------------

    public List<ITranslatableText> Translations { get; } = new List<ITranslatableText>();

    private object _Lock = new object();

    public string FileName { get; set; }
    public const string DEFAULT_TRANSLATIONS_PATHNAME = "Translations";
    public string TranslationsPathName {
      get {
        if (string.IsNullOrWhiteSpace(_TranslationsPathName)) {
          return DEFAULT_TRANSLATIONS_PATHNAME;
        }
        return _TranslationsPathName;
      }
      set {
        _TranslationsPathName = value;
      }
    }
    private string _TranslationsPathName;

    public string FullName => Path.Combine(TranslationsPathName, FileName);

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TTranslator() : base(TLogger.SYSTEM_LOGGER) { }
    public TTranslator(XElement xmlSource) : base(TLogger.SYSTEM_LOGGER) {
      Translations.AddRange(_ParseXml(xmlSource));
    }
    public TTranslator(string filename) : base(TLogger.SYSTEM_LOGGER) {
      FileName = filename;
      Load(filename);
    }
    public TTranslator(Stream stream) : base(TLogger.SYSTEM_LOGGER) {
      Load(stream);
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    protected IEnumerable<ITranslatableText> _ParseXml(XElement xmlSource) {
      if (xmlSource == null) {
        yield break;
      }

      foreach (XElement MessageItem in xmlSource.Elements(XML_MESSAGE_ELEMENT)) {
        ITranslatableText TranslatableMessage = new TTranslatableText(MessageItem.Attribute(XML_MESSAGE_ATTRIBUTE_ID).Value);
        foreach (XElement TranslationItem in MessageItem.Elements(XML_TRANSLATION_ELEMENT)) {
          string CultureName = TranslationItem.Attribute(XML_TRANSLATION_ATTRIBUTE_CULTURE).Value;
          string TranslatedMessage = TranslationItem.Value;
          TranslatableMessage.AddTranslation(CultureInfo.GetCultureInfo(CultureName), TranslatedMessage);
        }
        yield return TranslatableMessage;
      }
    }

    #region --- GetTranslation --------------------------------------------
    public string GetTranslation(string key) {
      return GetTranslation(key, CurrentCulture);
    }
    public string GetTranslation(string key, CultureInfo culture) {
      return GetTranslation(key, culture.Name);
    }
    public string GetTranslation(string key, string cultureName) {
      if (string.IsNullOrEmpty(key)) {
        return null;
      }
      if (string.IsNullOrEmpty(cultureName)) {
        cultureName = CurrentCulture.Name;
      }

      ITranslatableText TranslatableText;
      lock (_Lock) {
        if (!Translations.Any()) {
          return null;
        }

        string UpperKey = key.ToUpperInvariant();
        TranslatableText = Translations.FirstOrDefault(x => x.Key.ToUpperInvariant() == UpperKey);
        if (TranslatableText == null) {
          return null;
        }
      }

      return TranslatableText.Translate(cultureName);

    }
    #endregion --- GetTranslation -----------------------------------------

    #region --- GetTranslationFormat --------------------------------------------
    public string GetTranslationFormat(string key, IEnumerable<object> values) {
      return GetTranslationFormat(key, CurrentCulture, values);
    }
    public string GetTranslationFormat(string key, CultureInfo culture, IEnumerable<object> values) {
      return GetTranslationFormat(key, culture.Name, values);
    }
    public string GetTranslationFormat(string key, string cultureName, IEnumerable<object> values) {
      if (string.IsNullOrEmpty(key)) {
        return null;
      }

      if (string.IsNullOrEmpty(cultureName)) {
        cultureName = CurrentCulture.Name;
      }

      ITranslatableText TranslatableText;
      lock (_Lock) {
        if (!Translations.Any()) {
          return null;
        }

        TranslatableText = Translations.FirstOrDefault(x => x.Key.ToUpperInvariant() == key.ToUpperInvariant());
        if (TranslatableText == null) {
          return null;
        }
      }

      return TranslatableText.TranslateFormat(cultureName, values);

    }
    #endregion --- GetTranslationFormat -----------------------------------------

    public void Clear() {
      lock (_Lock) {
        Translations.Clear();
      }
    }

    public void AddTranslatable(ITranslatableText translatableText) {
      lock (_Lock) {
        if (!Translations.Exists(x => x.Key == translatableText.Key)) {
          Translations.Add(translatableText);
        }
      }
    }

    #region --- Load & merge --------------------------------------------
    public ITranslator Load(string filename) {
      FileName = filename;
      Clear();
      #region === Validate parameters ===
      if (string.IsNullOrWhiteSpace(filename)) {
        LogError("Unable to load Translator file : filename is missing or invalid");
        return this;
      }

      if (!File.Exists(filename)) {
        LogError($"Unable to load Translator file {filename}: filename is missing or access is denied");
        return this;
      }
      #endregion === Validate parameters ===

      try {
        LogDebug($"Loading translation file {FullName}");
        XDocument Doc = XDocument.Load(FullName);
        XElement Root = Doc.Root;
        Translations.AddRange(_ParseXml(Root));
      } catch (Exception ex) {
        LogError($"Error while reading translator file {FullName} : {ex.Message}");
        return null;
      }

      return this;
    }

    public ITranslator Load(Stream stream) {
      Clear();
      #region === Validate parameters ===
      if (stream == null || stream.CanRead == false || stream.Length == 0) {
        LogError($"Unable to load Translator from stream : the stream is null or invalid");
        return this;
      }
      #endregion === Validate parameters ===

      try {
        LogDebug($"Loading translation file {FullName}");
        XDocument Doc = XDocument.Load(stream);
        XElement Root = Doc.Root;
        Translations.AddRange(_ParseXml(Root));
      } catch (Exception ex) {
        LogError($"Error while reading translator from stream : {ex.Message}");
        return null;
      }

      return this;
    }


    public ITranslator Merge(string filename, string pathname = "") {
      #region === Validate parameters ===
      if (string.IsNullOrWhiteSpace(filename)) {
        LogError("Unable to merge Translator file : filename is missing or invalid");
        return this;
      }

      string MergeFullName = pathname == "" ? Path.Combine(TranslationsPathName, filename) : Path.Combine(pathname, filename);

      if (!File.Exists(MergeFullName)) {
        LogError($"Unable to merge Translator file {filename}: filename is missing or access is denied");
        return this;
      }
      #endregion === Validate parameters ===

      try {
        LogDebug($"Merging translator file {MergeFullName}");
        XDocument Doc = XDocument.Load(MergeFullName);
        XElement Root = Doc.Root;
        Translations.AddRange(_ParseXml(Root));
      } catch (Exception ex) {
        LogError($"Error while reading translator file {MergeFullName} : {ex.Message}");
        return this;
      }

      return this;
    }

    public ITranslator Merge(Stream stream) {
      #region === Validate parameters ===
      if (stream == null || stream.CanRead == false || stream.Length == 0) {
        LogError($"Unable to merge Translator from stream : the stream is null or invalid");
        return this;
      }
      #endregion === Validate parameters ===

      try {
        LogDebug($"Merging translator file from stream");
        XDocument Doc = XDocument.Load(stream);
        XElement Root = Doc.Root;
        Translations.AddRange(_ParseXml(Root));
      } catch (Exception ex) {
        LogError($"Error while merging translator file from stream : {ex.Message}");
        return this;
      }

      return this;
    }

    protected Stream _LoadResourceFile(string name) {
      try {
        LogDebug($"Loading resource {name}");

        string CompleteName = Assembly.GetExecutingAssembly().GetManifestResourceNames().SingleOrDefault(x => x.EndsWith(name));
        if (!string.IsNullOrWhiteSpace(CompleteName)) {
          return Assembly.GetExecutingAssembly().GetManifestResourceStream(CompleteName);
        }

        CompleteName = Assembly.GetCallingAssembly().GetManifestResourceNames().SingleOrDefault(x => x.EndsWith(name));
        if (!string.IsNullOrWhiteSpace(CompleteName)) {
          return Assembly.GetCallingAssembly().GetManifestResourceStream(CompleteName);
        }

        CompleteName = Assembly.GetEntryAssembly().GetManifestResourceNames().SingleOrDefault(x => x.EndsWith(name));
        if (!string.IsNullOrWhiteSpace(CompleteName)) {
          return Assembly.GetEntryAssembly().GetManifestResourceStream(CompleteName);
        }

        throw new ApplicationException("Missing resource");
      } catch (Exception ex) {
        LogError($"Unable to load resource {name} : {ex.Message}");
        return null;
      }
    }
    #endregion --- Load & merge -----------------------------------------


  }
}
