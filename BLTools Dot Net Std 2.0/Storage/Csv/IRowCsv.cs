using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLTools.Storage.Csv {

  /// <summary>
  /// Represent a metadata row
  /// </summary>
  public interface IRowCsv {

    #region --- Public properties ------------------------------------------------------------------------------
#if NETSTANDARD2_0 || NETSTANDARD2_1
#else
    /// <summary>
    /// Csv separator between fields
    /// </summary>
    static char SEPARATOR { get; set; }

#endif

    /// <summary>
    /// The type of row (e.g. header, data, footer)
    /// </summary>
    ERowCsvType RowType { get; }

#if NETSTANDARD2_0 || NETSTANDARD2_1
    /// <summary>
    /// The identifier of the row
    /// </summary>
    string Id { get; set; }
#else
    /// <summary>
    /// The identifier of the row
    /// </summary>
    string Id { get; init; }
#endif
    #endregion --- Public properties ---------------------------------------------------------------------------


    #region Public methods
    /// <summary>
    /// Output the csv row for a storage file
    /// </summary>
    /// <returns>The row as it is stored</returns>
    string Render();

    /// <summary>
    /// Get the content of the metadata row as string
    /// </summary>
    /// <returns>A string with all the content of the row without parsing</returns>
    string Get();

    #region --- Get data from one field only row --------------------------------------------
    /// <summary>
    /// Get the content of the metadata row in the right type (T) (i.e. the content is only one value)
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <returns>The value already converted to T, otherwise default value for T</returns>
    T Get<T>();

    /// <summary>
    /// Get the content of the metadata row in the right type (T) (i.e. the content is only one value)
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="cultureInfo">The culture to use for conversion</param>
    /// <returns>The value already converted to T, otherwise default value for T</returns>
    T Get<T>(CultureInfo cultureInfo);

    /// <summary>
    /// Get the content of the metadata row in the right type (T) (i.e. the content is only one value)
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="defaultValue">The default value to return in case of trouble</param>
    /// <returns>The value already converted to T, otherwise default value specified as parameter</returns>
    T Get<T>(T defaultValue);

    /// <summary>
    /// Get the content of the metadata row in the right type (T) (i.e. the content is only one value)
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="defaultValue">The default value to return in case of trouble</param>
    /// <param name="cultureInfo">The culture to use for conversion</param>
    /// <returns>The value already converted to T, otherwise default value specified as parameter</returns>
    T Get<T>(T defaultValue, CultureInfo cultureInfo);
    #endregion --- Get data from one field only row --------------------------------------------

    #region --- Get data from multifields row --------------------------------------------
    /// <summary>
    /// Get the content of a field from the metadata row in the right type (T)
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="fieldNumber">The number of the field to retrieve</param>
    /// <returns>The value already converted to T, otherwise default value for T</returns>
    T Get<T>(int fieldNumber);

    /// <summary>
    /// Get the content of a field from the metadata row in the right type (T)
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="fieldNumber">The number of the field to retrieve</param>
    /// <param name="defaultValue">The default value to return in case of trouble</param>
    /// <returns>The value already converted to T, otherwise default value specified as parameter</returns>
    T Get<T>(int fieldNumber, T defaultValue);

    /// <summary>
    /// Get the content of a field from the metadata row in the right type (T)
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="fieldNumber">The number of the field to retrieve</param>
    /// <param name="cultureInfo">The culture to use for conversion</param>
    /// <returns>The value already converted to T, otherwise default value for T</returns>
    T Get<T>(int fieldNumber, CultureInfo cultureInfo);

    /// <summary>
    /// Get the content of a field from the metadata row in the right type (T)
    /// </summary>
    /// <typeparam name="T">The type of the returned value</typeparam>
    /// <param name="fieldNumber">The number of the field to retrieve</param>
    /// <param name="defaultValue">The default value to return in case of trouble</param>
    /// <param name="cultureInfo">The culture to use for conversion</param>
    /// <returns>The value already converted to T, otherwise default value specified as parameter</returns>
    T Get<T>(int fieldNumber, T defaultValue, CultureInfo cultureInfo);
    #endregion --- Get data from multifields row --------------------------------------------


    /// <summary>
    /// Assign values to the row
    /// </summary>
    /// <param name="content">The values in string format</param>
    void Set(params object[] content);

    #region --- int and long --------------------------------------------
    /// <summary>
    /// Assign one int value as the content
    /// </summary>
    /// <param name="value">The value to assign</param>
    void Set(int value);

    /// <summary>
    /// Assign one long value as the content
    /// </summary>
    /// <param name="value">The value to assign</param>
    void Set(long value);
    #endregion --- int and long --------------------------------------------

    #region --- float --------------------------------------------
    /// <summary>
    /// Assign one float value as the content. CultureInfo is CultureInfo.InvariantCulture.
    /// </summary>
    /// <param name="value">The value to assign</param>
    void Set(float value);

    /// <summary>
    /// Assign one float value as the content.
    /// </summary>
    /// <param name="value">The value to assign</param>
    /// <param name="cultureInfo">the CultureInfo for string conversion</param>
    void Set(float value, CultureInfo cultureInfo);
    #endregion --- float --------------------------------------------

    #region --- double --------------------------------------------
    /// <summary>
    /// Assign one double value as the content. CultureInfo is CultureInfo.InvariantCulture.
    /// </summary>
    /// <param name="value">The value to assign</param>
    void Set(double value);

    /// <summary>
    /// Assign one double value as the content.
    /// </summary>
    /// <param name="value">The value to assign</param>
    /// <param name="cultureInfo">the CultureInfo for string conversion</param>
    void Set(double value, CultureInfo cultureInfo);

    #endregion --- double --------------------------------------------

    #region --- decimal --------------------------------------------
    /// <summary>
    /// Assign one decimal value as the content. CultureInfo is CultureInfo.InvariantCulture.
    /// </summary>
    /// <param name="value">The value to assign</param>
    void Set(decimal value);

    /// <summary>
    /// Assign one decimal value as the content.
    /// </summary>
    /// <param name="value">The value to assign</param>
    /// <param name="cultureInfo">the CultureInfo for string conversion</param>
    void Set(decimal value, CultureInfo cultureInfo);

    #endregion --- decimal --------------------------------------------

    #region --- DateTime --------------------------------------------
    /// <summary>
    /// Assign one DateTime value as the content. CultureInfo is CultureInfo.InvariantCulture.
    /// </summary>
    /// <param name="value">The value to assign</param>
    void Set(DateTime value);

    /// <summary>
    /// Assign one DateTime value as the content.
    /// </summary>
    /// <param name="value">The value to assign</param>
    /// <param name="cultureInfo">the CultureInfo for string conversion</param>
    void Set(DateTime value, CultureInfo cultureInfo);
    #endregion --- DateTime --------------------------------------------

    #region --- bool --------------------------------------------
    /// <summary>
    /// Assign one DateTime value as the content.
    /// </summary>
    /// <param name="value">The value to assign</param>
    void Set(bool value);
    #endregion --- bool --------------------------------------------

    #region --- string --------------------------------------------
    /// <summary>
    /// Assign one string value as the content.
    /// </summary>
    /// <param name="value">The value to assign</param>
    void Set(string value);
    #endregion --- string --------------------------------------------

    #region --- IEnumerable<T> --------------------------------------------
    /// <summary>
    /// Assign IEnumerable&lt;T&gt; values as the content. The CultureInfo is CultureInfo.InvariantCulture
    /// </summary>
    /// <param name="value">The IEnumerable of values to assign</param>
    void Set<T>(IEnumerable<T> value);

    /// <summary>
    /// Assign IEnumerable&lt;T&gt; values as the content.
    /// </summary>
    /// <param name="value">The IEnumerable of values to assign</param>
    /// <param name="cultureInfo">The CultureInfo for the string conversion</param>
    void Set<T>(IEnumerable<T> value, CultureInfo cultureInfo);
    #endregion --- IEnumerable<T> -------------------------------------------- 
    #endregion Public methods

  }
}
