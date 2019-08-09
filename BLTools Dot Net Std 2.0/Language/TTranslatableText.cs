using BLTools.Diagnostic.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BLTools.Language
{
    public class TTranslatableText : TLoggable, ITranslatableText
    {

        #region --- Culture --------------------------------------------
        public readonly static CultureInfo DEFAULT_CULTURE = CultureInfo.CurrentCulture;
        public readonly static CultureInfo DEFAULT_FALLBACK_CULTURE = CultureInfo.InvariantCulture;

        public static CultureInfo GlobalCulture
        {
            get
            {
                if (_GlobalCulture == null)
                {
                    return DEFAULT_CULTURE;
                }
                return _GlobalCulture;
            }
            set
            {
                _GlobalCulture = value;
            }
        }
        private static CultureInfo _GlobalCulture;

        public CultureInfo CurrentCulture
        {
            get
            {
                if (_CurrentCulture == null)
                {
                    return GlobalCulture;
                }
                return _CurrentCulture;
            }
            set
            {
                _CurrentCulture = value;
            }
        }
        private CultureInfo _CurrentCulture;

        public static CultureInfo GlobalFallbackCulture
        {
            get
            {
                if (_GlobalFallbackCulture == null)
                {
                    return DEFAULT_CULTURE;
                }
                return _GlobalFallbackCulture;
            }
            set
            {
                _GlobalFallbackCulture = value;
            }
        }
        private static CultureInfo _GlobalFallbackCulture;

        public CultureInfo FallbackCulture
        {
            get
            {
                if (_FallbackCulture == null)
                {
                    return DEFAULT_FALLBACK_CULTURE;
                }
                return _FallbackCulture;
            }
            set
            {
                _FallbackCulture = value;
            }
        }
        private CultureInfo _FallbackCulture;
        #endregion --- Culture -----------------------------------------

        #region --- Public properties ------------------------------------------------------------------------------
        public string Key { get; private set; }
        #endregion --- Public properties ---------------------------------------------------------------------------

        #region Private variables
        private SortedDictionary<string, string> _Translations = new SortedDictionary<string, string>();

        private readonly object _Lock = new object();
        #endregion Private variables

        #region --- Constructor(s) ---------------------------------------------------------------------------------
        public TTranslatableText(string textKey) : this(textKey, TLogger.SYSTEM_LOGGER) { }
        public TTranslatableText(string textKey, ILogger logger) : base(logger)
        {
            Key = textKey;
        }

        public TTranslatableText(string textKey, CultureInfo culture) : this(textKey, culture, TLogger.SYSTEM_LOGGER) { }
        public TTranslatableText(string textKey, CultureInfo culture, ILogger logger) : base(logger)
        {
            Key = textKey;
            CurrentCulture = culture;
        }

        public TTranslatableText(string textKey, string cultureName) : this(textKey, cultureName, TLogger.SYSTEM_LOGGER) { }
        public TTranslatableText(string textKey, string cultureName, ILogger logger) : base(logger)
        {
            Key = textKey;
            try
            {
                CurrentCulture = CultureInfo.GetCultureInfo(cultureName);
            }
            catch (Exception ex)
            {
                LogError($"Error creating TTranslatableText with culture {cultureName} : {ex.Message}");
                CurrentCulture = CultureInfo.CurrentCulture;
            }
        }
        #endregion --- Constructor(s) ------------------------------------------------------------------------------

        #region --- Converters -------------------------------------------------------------------------------------
        public override string ToString()
        {
            StringBuilder RetVal = new StringBuilder();
            RetVal.Append($"{Key} : ");
            foreach (KeyValuePair<string, string> TranslationItem in _Translations)
            {
                RetVal.Append(TranslationItem.Key);
                RetVal.Append("=");
                RetVal.Append(TranslationItem.Value);
                RetVal.Append(", ");
            }
            if (_Translations.Any())
            {
                RetVal.Remove(RetVal.Length - 2, 2);
            }
            return RetVal.ToString();
        }
        #endregion --- Converters ----------------------------------------------------------------------------------

        #region --- Translate --------------------------------------------
        public string Translate()
        {
            return Translate(CurrentCulture);
        }
        public string Translate(CultureInfo culture)
        {
            if (culture == null)
            {
                LogError($"Error while translating {Key} : culture is missing");
                culture = FallbackCulture;
            }
            return Translate(culture.Name);
        }
        public string Translate(string cultureName)
        {
            #region === Validate parameters ===
            if (string.IsNullOrWhiteSpace(cultureName))
            {
                LogWarning($"Warning during translation of {Key} : culture name is missing or invalid, using fallback value {FallbackCulture.Name}");
                cultureName = FallbackCulture.Name;
            }
            #endregion === Validate parameters ===

            lock (_Lock)
            {
                if (_Translations.IsEmpty())
                {
                    LogError($"No translation available for message {Key}");
                    return Key;
                }

                if (!_Translations.ContainsKey(cultureName.ToUpperInvariant()))
                {
                    LogError($"Unable to translate message {Key} : Missing translation in culture {cultureName}");
                    return Key;
                }

                return _Translations[cultureName.ToUpperInvariant()];
            }
        }
        #endregion --- Translate -----------------------------------------

        #region --- TranslateFormat --------------------------------------------
        public string TranslateFormat(IEnumerable<object> values)
        {
            return TranslateFormat(CurrentCulture, values);
        }
        public string TranslateFormat(string cultureName, IEnumerable<object> values)
        {
            if (string.IsNullOrWhiteSpace(cultureName))
            {
                LogError($"Error while translating {Key} : culture name is missing or invalid");
                cultureName = FallbackCulture.Name;
            }

            CultureInfo TempCultureInfo;
            try
            {
                TempCultureInfo = CultureInfo.GetCultureInfo(cultureName);
            }
            catch (Exception ex)
            {
                LogError($"Error while translating {Key} : {ex.Message}");
                TempCultureInfo = FallbackCulture;
            }
            return TranslateFormat(TempCultureInfo, values);
        }
        public string TranslateFormat(CultureInfo culture, IEnumerable<object> values)
        {
            #region === Validate parameters ===
            if (culture == null)
            {
                LogWarning($"Warning during translation of {Key} : culture is missing or invalid, using default value");
                culture = FallbackCulture;
            }
            #endregion === Validate parameters ===

            lock (_Lock)
            {
                if (_Translations.IsEmpty())
                {
                    LogError($"No translation available for message {Key}");
                    return Key;
                }

                if (!_Translations.ContainsKey(culture.Name.ToUpperInvariant()))
                {
                    LogError($"Unable to translate message {Key} : Missing translation in culture {culture.Name}");
                    return Key;
                }

                try
                {
                    return string.Format(culture, _Translations[culture.Name.ToUpperInvariant()], values.ToArray());
                }
                catch (Exception ex)
                {
                    LogError($"Error while translating {Key} in culture {culture.Name} : {ex.Message}");
                    return Key;
                }
            }
        }
        #endregion --- TranslateFormat -----------------------------------------

        #region --- AddTranslation --------------------------------------------
        public void AddTranslation(string translation)
        {
            AddTranslation(CurrentCulture, translation);
        }
        public void AddTranslation(CultureInfo culture, string translation)
        {
            #region === Validate parameters ===
            if (culture == null)
            {
                LogError($"Error while adding \"{translation}\" : culture is missing");
                return;
            }
            #endregion === Validate parameters ===

            AddTranslation(culture.Name, translation);
        }
        public void AddTranslation(string cultureName, string translation)
        {
            #region === Validate parameters ===
            if (string.IsNullOrWhiteSpace(cultureName))
            {
                LogError($"Unable to add translation \"{translation}\" for {Key} : culture name is missing or invalid");
                return;
            }
            #endregion === Validate parameters ===

            lock (_Lock)
            {
                if (_Translations.ContainsKey(cultureName))
                {
                    LogError($"Unable to add translation \"{translation}\" for culture {cultureName} : the value \"{_Translations[cultureName]}\" is already present for this key");
                    return;
                }
                _Translations.Add(cultureName.ToUpperInvariant(), translation);
            }
        }
        #endregion --- AddTranslation -----------------------------------------

        #region --- ClearTranslations --------------------------------------------
        public void ClearTranslations()
        {
            lock (_Lock)
            {
                _Translations.Clear();
            }
        }
        #endregion --- ClearTranslations -----------------------------------------

        #region --- RemoveTranslation --------------------------------------------
        public void RemoveTranslation()
        {
            RemoveTranslation(CurrentCulture);
        }
        public void RemoveTranslation(CultureInfo culture)
        {
            #region === Validate parameters ===
            if (culture == null)
            {
                LogError($"Unable to remove translation for {Key} : culture name is missing or invalid");
                return;
            }
            #endregion === Validate parameters ===

            RemoveTranslation(culture.Name);
        }
        public void RemoveTranslation(string cultureName)
        {
            #region === Validate parameters ===
            if (string.IsNullOrWhiteSpace(cultureName))
            {
                LogError("Unable to remove translation : culture name is missing or invalid");
                return;
            }
            #endregion === Validate parameters ===

            lock (_Lock)
            {
                if (!_Translations.ContainsKey(cultureName.ToUpperInvariant()))
                {
                    LogError($"Unable to remove translation for culture {cultureName} : the key is missing");
                    return;
                }
                _Translations.Remove(cultureName);
            }
        }
        #endregion --- RemoveTranslation -----------------------------------------

    }
}
