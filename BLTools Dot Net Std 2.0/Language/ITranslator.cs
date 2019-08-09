using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BLTools.Language {
    public interface ITranslator
    {
        List<ITranslatableText> Translations { get; }

        CultureInfo CurrentCulture { get; set; }

        string GetTranslation(string key);
        string GetTranslation(string key, CultureInfo culture);
        string GetTranslation(string key, string cultureName);

        string GetTranslationFormat(string key, IEnumerable<object> values);
        string GetTranslationFormat(string key, CultureInfo culture, IEnumerable<object> values);
        string GetTranslationFormat(string key, string cultureName, IEnumerable<object> values);


        void AddTranslatable(ITranslatableText translatableText);
        void Clear();

        ITranslator Load(string filename);
        ITranslator Merge(string filename, string pathname = "");

    }
}
