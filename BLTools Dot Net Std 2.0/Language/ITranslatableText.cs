using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BLTools.Language
{
    public interface ITranslatableText
    {
        string Key { get; }

        CultureInfo CurrentCulture { get; set; }
        CultureInfo FallbackCulture { get; set; }

        string Translate();
        string Translate(CultureInfo culture);
        string Translate(string cultureName);

        string TranslateFormat(CultureInfo culture, IEnumerable<object> values);
        string TranslateFormat(string cultureName, IEnumerable<object> values);

        void AddTranslation(string translation);
        void AddTranslation(CultureInfo culture, string translation);
        void AddTranslation(string cultureName, string translation);

        void ClearTranslations();

        void RemoveTranslation();
        void RemoveTranslation(CultureInfo culture);
        void RemoveTranslation(string cultureName);
    }
}
