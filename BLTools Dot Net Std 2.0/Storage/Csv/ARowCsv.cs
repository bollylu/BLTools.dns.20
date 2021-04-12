using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Storage.Csv {
  /// <summary>
  /// Basic implementation of IRowCsv
  /// </summary>
  public abstract class ARowCsv : IRowCsv {

    /// <summary>
    /// Csv separator between fields
    /// </summary>
    public static char SEPARATOR = ';';

#if NETSTANDARD2_0 || NETSTANDARD2_1
    /// <summary>
    /// The raw content of the row
    /// </summary>
    public string RawContent {
      get {
        return _RawContent;
      }
      set {
        _RawContent = value;
      }
    }
#else
    /// <summary>
    /// The raw content of the row
    /// </summary>
    public string RawContent {
      get {
        return _RawContent;
      }
      init {
        _RawContent = value;
      }
    }
#endif
    private string _RawContent;


#if NETSTANDARD2_0 || NETSTANDARD2_1
    /// <inheritdoc/>
    public string Id {
      get {
        return (_Id ?? "").RemoveExternalQuotes();
      }
      set {
        _Id = value.WithQuotes();
      }
    }

#else
    /// <inheritdoc/>
    public string Id {
      get {
        return (_Id ?? "").RemoveExternalQuotes();
      }
      init {
        _Id = value.WithQuotes();
      }
    }

#endif
    private string _Id;

    /// <inheritdoc/>
    public ERowCsvType RowType { get; protected set; }

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    /// <summary>
    /// Create a new row
    /// </summary>
    /// <param name="rowType">The type of row to create</param>
    protected ARowCsv(ERowCsvType rowType) {
      RowType = rowType;
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    /// <summary>
    /// Convert to a string
    /// </summary>
    /// <returns>A string representation of the object</returns>
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append($"[{RowType}] ");
      RetVal.Append($"{_Id}{SEPARATOR}{RawContent}");
      return RetVal.ToString();
    }

    /// <inheritdoc/>
    public string Render() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append($"{RowType.ToString().WithQuotes()}");
      RetVal.Append($"{SEPARATOR}{_Id}{SEPARATOR}{RawContent}");
      return RetVal.ToString();
    }

    #region --- Get --------------------------------------------
    /// <inheritdoc/>
    public string Get() {
      return RawContent.RemoveExternalQuotes();
    }

    /// <inheritdoc/>
    public T Get<T>() {
      return Get<T>(CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public T Get<T>(CultureInfo cultureInfo) {
      return Get<T>(default(T), cultureInfo);
    }

    /// <inheritdoc/>
    public T Get<T>(T defaultValue) {
      return Get<T>(defaultValue, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public T Get<T>(T defaultValue, CultureInfo cultureInfo) {
      switch (typeof(T).Name.ToLowerInvariant()) {
        case "string":
          return (T)Convert.ChangeType(RawContent.RemoveExternalQuotes(), typeof(T));
        default:
          return BLConverter.BLConvert<T>(RawContent, cultureInfo, defaultValue);
      }

    }

    /// <inheritdoc/>
    public T Get<T>(int fieldNumber) {
      return Get<T>(fieldNumber, default(T));
    }

    /// <inheritdoc/>
    public T Get<T>(int fieldNumber, T defaultValue) {
      return Get<T>(fieldNumber, defaultValue, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public T Get<T>(int fieldNumber, CultureInfo cultureInfo) {
      return Get<T>(fieldNumber, default(T), cultureInfo);
    }

    /// <inheritdoc/>
    public T Get<T>(int fieldNumber, T defaultValue, CultureInfo cultureInfo) {
      if (fieldNumber < 0) {
        return defaultValue;
      }
      string[] Components = RawContent.Split(SEPARATOR);
      if (fieldNumber >= Components.Length) {
        return defaultValue;
      }
      switch (typeof(T).Name.ToLowerInvariant()) {
        case "string":
          return (T)Convert.ChangeType(Components[fieldNumber].RemoveExternalQuotes(), typeof(T));
        default:
          return BLConverter.BLConvert<T>(Components[fieldNumber], cultureInfo, defaultValue);
      }
    }
    #endregion --- Get --------------------------------------------


    #region --- Set --------------------------------------------
    /// <inheritdoc/>
    public void Set(params object[] content) {
      if (content is null) {
        return;
      }
      StringBuilder Data = new StringBuilder();
      foreach (object ContentItem in content) {

#if NETSTANDARD2_0 || NETSTANDARD2_1
        switch (ContentItem.GetType().Name.ToLowerInvariant()) {
          case "int":
          case "long":
          case "boolean":
            Data.Append(ContentItem.ToString());
            break;
          case "single":
            Data.Append(((float)ContentItem).ToString(CultureInfo.InvariantCulture));
            break;
          case "double":
            Data.Append(((double)ContentItem).ToString(CultureInfo.InvariantCulture));
            break;
          case "decimal":
            Data.Append(((decimal)ContentItem).ToString(CultureInfo.InvariantCulture));
            break;
          case "datetime":
            Data.Append(((DateTime)ContentItem).ToString(CultureInfo.InvariantCulture));
            break;
          case "string":
            Data.Append(((string)ContentItem).WithQuotes());
            break;
          default:
            Data.Append(ContentItem.ToString().WithQuotes());
            break;
        }
        Data.Append(SEPARATOR);

#else
        switch (ContentItem) {
          case int:
          case long:
          case bool:
            Data.Append(ContentItem.ToString());
            break;
          case float Value:
            Data.Append(Value.ToString(CultureInfo.InvariantCulture));
            break;
          case double Value:
            Data.Append(Value.ToString(CultureInfo.InvariantCulture));
            break;
          case decimal Value:
            Data.Append(Value.ToString(CultureInfo.InvariantCulture));
            break;
          case DateTime Value:
            Data.Append(Value.ToString(CultureInfo.InvariantCulture));
            break;
          case string Value:
            Data.Append(Value.WithQuotes());
            break;
          default:
            Data.Append(ContentItem.ToString().WithQuotes());
            break;
        }

        Data.Append(SEPARATOR);

#endif
      }
      Data.Truncate(1);
      _RawContent = Data.ToString();
    }

    /// <inheritdoc/>
    public void Set(int value) {
      _RawContent = value.ToString();
    }

    /// <inheritdoc/>
    public void Set(long value) {
      _RawContent = value.ToString();
    }

    /// <inheritdoc/>
    public void Set(float value) {
      _RawContent = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public void Set(float value, CultureInfo cultureInfo) {
      _RawContent = value.ToString(cultureInfo);
    }

    /// <inheritdoc/>
    public void Set(double value) {
      _RawContent = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public void Set(double value, CultureInfo cultureInfo) {
      _RawContent = value.ToString(cultureInfo);
    }

    /// <inheritdoc/>
    public void Set(decimal value) {
      _RawContent = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public void Set(decimal value, CultureInfo cultureInfo) {
      _RawContent = value.ToString(cultureInfo);
    }

    /// <inheritdoc/>
    public void Set(DateTime value) {
      _RawContent = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public void Set(DateTime value, CultureInfo cultureInfo) {
      _RawContent = value.ToString(cultureInfo);
    }

    /// <inheritdoc/>
    public void Set(bool value) {
      _RawContent = value.ToString();
    }

    /// <inheritdoc/>
    public void Set(string value) {
      _RawContent = value.WithQuotes();
    }

    /// <inheritdoc/>
    public void Set<T>(IEnumerable<T> value) {
      Set(value, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc/>
    public void Set<T>(IEnumerable<T> value, CultureInfo cultureInfo) {
      StringBuilder Data = new StringBuilder();

#if NETSTANDARD2_0 || NETSTANDARD2_1

      switch (typeof(T).Name.ToLowerInvariant()) {
        case "int":
        case "long":
        case "boolean":
          foreach (T ValueItem in value) {
            Data.Append(ValueItem.ToString());
            Data.Append(SEPARATOR);
          }
          break;
        case "single":
          foreach (float ValueItem in value as IEnumerable<float>) {
            Data.Append(ValueItem.ToString(cultureInfo));
            Data.Append(SEPARATOR);
          }
          break;
        case "double":
          foreach (double ValueItem in value as IEnumerable<double>) {
            Data.Append(ValueItem.ToString(cultureInfo));
            Data.Append(SEPARATOR);
          }
          break;
        case "decimal":
          foreach (decimal ValueItem in value as IEnumerable<decimal>) {
            Data.Append(ValueItem.ToString(cultureInfo));
            Data.Append(SEPARATOR);
          }
          break;
        case "datetime":
          foreach (DateTime ValueItem in value as IEnumerable<DateTime>) {
            Data.Append(ValueItem.ToString(cultureInfo));
            Data.Append(SEPARATOR);
          }
          break;
        case "string":
          foreach (string ValueItem in value as IEnumerable<string>) {
            Data.Append(ValueItem.WithQuotes());
            Data.Append(SEPARATOR);
          }
          break;

      }
#else

      switch (value) {
        case IEnumerable<int>:
        case IEnumerable<long>:
        case IEnumerable<bool>:
          foreach (T ValueItem in value) {
            Data.Append(ValueItem.ToString());
            Data.Append(SEPARATOR);
          }
          break;
        case IEnumerable<float> Values:
          foreach (float ValueItem in Values) {
            Data.Append(ValueItem.ToString(cultureInfo));
            Data.Append(SEPARATOR);
          }
          break;
        case IEnumerable<double> Values:
          foreach (double ValueItem in Values) {
            Data.Append(ValueItem.ToString(cultureInfo));
            Data.Append(SEPARATOR);
          }
          break;
        case IEnumerable<decimal> Values:
          foreach (decimal ValueItem in Values) {
            Data.Append(ValueItem.ToString(cultureInfo));
            Data.Append(SEPARATOR);
          }
          break;
        case IEnumerable<DateTime> Values:
          foreach (DateTime ValueItem in Values) {
            Data.Append(ValueItem.ToString(cultureInfo));
            Data.Append(SEPARATOR);
          }
          break;
        case IEnumerable<string> Values:
          foreach (string ValueItem in Values) {
            Data.Append(ValueItem.WithQuotes());
            Data.Append(SEPARATOR);
          }
          break;

      }

#endif

      if (Data.Length > 0) {
        Data.Truncate(1);
      }

      _RawContent = Data.ToString();
    }
    #endregion --- Set --------------------------------------------

    /// <summary>
    /// Convert raw data to the appropriate IRowCsv (i.e. Header, Data, Footer)
    /// </summary>
    /// <param name="rawData">The raw data</param>
    /// <returns>A correct IRowCsv, null in case of trouble</returns>
    public static IRowCsv Parse(string rawData) {
      if (rawData is null) {
        return null;
      }

      try {
        ERowCsvType RowType = (ERowCsvType)Enum.Parse(typeof(ERowCsvType), rawData.Before(ARowCsv.SEPARATOR).RemoveExternalQuotes());
        string RowId = rawData.After(ARowCsv.SEPARATOR).Before(ARowCsv.SEPARATOR).RemoveExternalQuotes();
        string Content = rawData.After(ARowCsv.SEPARATOR).After(ARowCsv.SEPARATOR);

        switch (RowType) {
          case ERowCsvType.Header:
            return new TRowCsvHeader() { Id = RowId, RawContent = Content };
          case ERowCsvType.Footer:
            return new TRowCsvFooter() { Id = RowId, RawContent = Content };
          case ERowCsvType.Data:
            return new TRowCsvData() { Id = RowId, RawContent = Content };
          default:
            return null;
        }
      } catch {
        return null;

      }
    }
  }
}
