using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BLTools.Diagnostic.Logging;

namespace BLTools.Storage.Csv {

  /// <summary>
  /// Reader for csv
  /// </summary>
  public class TCsvReader : BinaryReader, ILoggable {

    public Encoding ReaderEncoding { get; private set; }

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    /// <summary>
    /// Create a new csv reader
    /// </summary>
    /// <param name="inputStream">The stream to read from</param>
    public TCsvReader(Stream inputStream) : base(inputStream, Encoding.UTF8) {
      ReaderEncoding = Encoding.UTF8;
    }

    /// <summary>
    /// Create a new csv reader
    /// </summary>
    /// <param name="inputStream">The stream to read from</param>
    /// <param name="encoding">the encoding of the stream</param>
    public TCsvReader(Stream inputStream, Encoding encoding) : base(inputStream, encoding) {
      ReaderEncoding = encoding;
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------


    #region --- Read any row --------------------------------------------
    /// <summary>
    /// Obtain the next row
    /// </summary>
    /// <returns></returns>
    public IRowCsv ReadRow() {
      string RawData = ReadLine();
      if (RawData is null) {
        return null;
      }
      return _Parse(RawData);
    }

    ///// <summary>
    ///// Obtain the next row
    ///// </summary>
    ///// <returns></returns>
    //public async Task<IRowCsv> ReadRowAsync() {
    //  string RawData = await ReadLineAsync().WithTimeout(1000).ConfigureAwait(false);
    //  if (RawData is null) {
    //    return null;
    //  }
    //  return _Parse(RawData);
    //}
    #endregion --- Read any row --------------------------------------------

    #region --- Read header --------------------------------------------
    /// <summary>
    /// Obtain the next row header
    /// </summary>
    /// <returns></returns>
    public IRowCsv ReadHeader(bool findIt = false) {
      Logger.LogError($"Read header with find it = {findIt}");
      ERowCsvType RowType = PeekRowType();
      while (RowType != ERowCsvType.None) {
        switch (RowType) {
          case ERowCsvType.Header: {
              string RawData = ReadLine();
              return _Parse(RawData);
            }
          case ERowCsvType.Data:
          case ERowCsvType.Footer:
          case ERowCsvType.Unknown: {
              if (findIt) {
                ReadLine();
              } else {
                return null;
              }
              break;
            }
        }
        RowType = PeekRowType();
      }
      return null;

    }

    /// <summary>
    /// Read a whole header section
    /// </summary>
    /// <param name="findIt">true to search for the section</param>
    /// <returns></returns>
    public IRowCsv[] ReadHeaderSection(bool findIt = false) {
      Logger.Log("Read header section");
      List<IRowCsv> RetVal = new List<IRowCsv>();
      IRowCsv NextRow = ReadHeader(findIt);
      while (NextRow != null && NextRow is TRowCsvHeader) {
        Logger.Log(NextRow.ToString());
        RetVal.Add(NextRow);
        NextRow = ReadHeader(false);
      }
      return RetVal.ToArray();
    }

    ///// <summary>
    ///// Obtain the next row header
    ///// </summary>
    ///// <returns></returns>
    //public async Task<IRowCsv> ReadHeaderAsync(bool findIt = false) {
    //  ERowCsvType RowType = PeekNextRow();
    //  while (RowType != ERowCsvType.None) {
    //    switch (RowType) {
    //      case ERowCsvType.Header:
    //        string RawData = await ReadLineAsync().WithTimeout(1000).ConfigureAwait(false);
    //        return _Parse(RawData);
    //      case ERowCsvType.Data:
    //      case ERowCsvType.Footer:
    //      case ERowCsvType.Unknown:
    //        if (findIt) {
    //          await ReadLineAsync().WithTimeout(1000).ConfigureAwait(false);
    //        } else {
    //          return null;
    //        }
    //        break;
    //    }
    //    RowType = PeekNextRow();
    //  }
    //  return null;
    //}
    #endregion --- Read header --------------------------------------------

    #region --- Read footer --------------------------------------------
    /// <summary>
    /// Obtain the next row footer
    /// </summary>
    /// <returns></returns>
    public IRowCsv ReadFooter(bool findIt = false) {
      ERowCsvType RowType = PeekRowType();
      while (RowType != ERowCsvType.None) {
        switch (RowType) {
          case ERowCsvType.Footer: {
              string RawData = ReadLine();
              return _Parse(RawData);
            }
          case ERowCsvType.Data:
          case ERowCsvType.Header:
          case ERowCsvType.Unknown: {
              if (findIt) {
                ReadLine();
              } else {
                return null;
              }
              break;
            }
        }
        RowType = PeekRowType();
      }
      return null;

    }

    /// <summary>
    /// Read a whole footer section
    /// </summary>
    /// <param name="findIt">true to search for the section</param>
    /// <returns></returns>
    public IRowCsv[] ReadFooterSection(bool findIt = false) {
      Logger.Log("Read footer section");
      List<IRowCsv> RetVal = new List<IRowCsv>();
      IRowCsv NextRow = ReadFooter(findIt);
      while (NextRow != null && NextRow is TRowCsvFooter) {
        Logger.Log(NextRow.ToString());
        RetVal.Add(NextRow);
        NextRow = ReadFooter(false);
      }
      return RetVal.ToArray();
    }
    ///// <summary>
    ///// Obtain the next row footer
    ///// </summary>
    ///// <returns></returns>
    //public async Task<IRowCsv> ReadFooterAsync(bool findIt = false) {
    //  ERowCsvType RowType = PeekNextRow();
    //  while (RowType != ERowCsvType.None) {
    //    switch (RowType) {
    //      case ERowCsvType.Footer:
    //        string RawData = await ReadLineAsync().WithTimeout(1000).ConfigureAwait(false);
    //        return _Parse(RawData);
    //      case ERowCsvType.Data:
    //      case ERowCsvType.Header:
    //      case ERowCsvType.Unknown:
    //        if (findIt) {
    //          await ReadLineAsync().WithTimeout(1000).ConfigureAwait(false);
    //        } else {
    //          return null;
    //        }
    //        break;
    //    }
    //    RowType = PeekNextRow();
    //  }
    //  return null;
    //}
    #endregion --- Read footer --------------------------------------------

    #region --- Read data --------------------------------------------
    /// <summary>
    /// Obtain the next row of data
    /// If not in data section, read until end or first data row
    /// </summary>
    /// <param name="findIt">true to continue reading until next data or EOS</param>
    /// <returns></returns>
    public IRowCsv ReadData(bool findIt = false) {
      ERowCsvType RowType = PeekRowType();
      while (RowType != ERowCsvType.None) {
        switch (RowType) {
          case ERowCsvType.Data: {
              string RawData = ReadLine();
              return _Parse(RawData);
            }
          case ERowCsvType.Header:
          case ERowCsvType.Footer:
          case ERowCsvType.Unknown: {
              if (findIt) {
                ReadLine();
              } else {
                return null;
              }
              break;
            }
        }
        RowType = PeekRowType();
      }
      return null;
    }

    /// <summary>
    /// Read a whole header section
    /// </summary>
    /// <param name="findIt">true to search for the section</param>
    /// <returns></returns>
    public IRowCsv[] ReadDataSection(bool findIt = false) {
      Logger.Log("Read data section");
      List<IRowCsv> RetVal = new List<IRowCsv>();
      IRowCsv NextRow = ReadData(findIt);
      while (NextRow != null && NextRow is TRowCsvData) {
        Logger.Log(NextRow.ToString());
        RetVal.Add(NextRow);
        NextRow = ReadData(false);
      }
      return RetVal.ToArray();
    }
    ///// <summary>
    ///// Obtain the next row of data
    ///// </summary>
    ///// <param name="findIt">true to continue reading until next data or EOS</param>
    ///// <returns></returns>
    //public async Task<IRowCsv> ReadRowDataAsync(bool findIt = false) {
    //  ERowCsvType RowType = PeekNextRow();
    //  while (RowType != ERowCsvType.None) {
    //    switch (RowType) {
    //      case ERowCsvType.Data:
    //        string RawData = await ReadLineAsync().WithTimeout(1000).ConfigureAwait(false);
    //        return _Parse(RawData);
    //      case ERowCsvType.Header:
    //      case ERowCsvType.Footer:
    //      case ERowCsvType.Unknown:
    //        if (findIt) {
    //          await ReadLineAsync().WithTimeout(1000).ConfigureAwait(false);
    //        } else {
    //          return null;
    //        }
    //        break;
    //    }
    //    RowType = PeekNextRow();
    //  }
    //  return null;
    //}
    #endregion --- Read data --------------------------------------------

    #region --- Peek row type --------------------------------------------
    ///// <summary>
    ///// Return the next row type without moving the stream pointer
    ///// </summary>
    ///// <returns></returns>
    //public ERowCsvType PeekNextRow() {
    //  Logger.Log($"Position = {BaseStream.Position}");
    //  long SavedPosition = BaseStream.Position;
    //  try {
    //    string RawData = ReadLine();
    //    if (RawData is null) {
    //      return ERowCsvType.None;
    //    }
    //    ERowCsvType RowType = (ERowCsvType)Enum.Parse(typeof(ERowCsvType), RawData.Before(ARowCsv.SEPARATOR).RemoveExternalQuotes());
    //    Logger.Log($"Next row is {RawData}");
    //    Logger.Log($"Next row type is {RowType}");
    //    return RowType;
    //  } catch (Exception ex) {
    //    Logger.LogError($"Unable to peek next row : {ex.Message}");
    //    return ERowCsvType.Unknown;
    //  } finally {
    //    BaseStream.Position = SavedPosition;
    //  }
    //}

    /// <summary>
    /// Read the next row type without moving the stream pointer
    /// </summary>
    /// <returns></returns>
    public ERowCsvType PeekRowType() {
      Logger.Log($"Position = {BaseStream.Position}");

      if (BaseStream.Position == BaseStream.Length) {
        return ERowCsvType.None;
      }

      long SavedPosition = BaseStream.Position;
      try {

        using (MemoryStream TempStream = new MemoryStream()) {

          // Read up to EOL or EOF
          while (PeekChar() != -1) {
            TempStream.WriteByte(ReadByte());
            byte[] Buffer = TempStream.ToArray();

            if (Buffer.EndsWith(FIELD_SEPARATOR)) {
              byte[] Bytes = Buffer.Take(Buffer.Length - 1).ToArray();
              string Keyword = ReaderEncoding.GetString(Bytes);
              ERowCsvType RetVal = (ERowCsvType)Enum.Parse(typeof(ERowCsvType), Keyword.RemoveExternalQuotes());
              Logger.LogDebug(Keyword);
              return RetVal;
            }
          }

          return ERowCsvType.None;
        }

      } catch (Exception ex) {
        Logger.LogError($"Unable to read rowType from stream : {ex.Message}");
        return ERowCsvType.Unknown;
      } finally {
        BaseStream.Position = SavedPosition;
      }
    }

    ///// <summary>
    ///// Return the next row type without moving the stream pointer
    ///// </summary>
    ///// <returns></returns>
    //public async Task<ERowCsvType> PeekNextRowAsync() {
    //  long SavedPosition = BaseStream.Position;
    //  try {
    //    string RawData = ReadLine();
    //    if (RawData is null) {
    //      return ERowCsvType.None;
    //    }
    //    ERowCsvType RowType = (ERowCsvType)Enum.Parse(typeof(ERowCsvType), RawData.Before(ARowCsv.SEPARATOR).RemoveExternalQuotes());
    //    Logger.Log($"Next row is {RawData}");
    //    Logger.Log($"Next row type is {RowType}");
    //    return RowType;
    //  } catch (Exception ex) {
    //    Logger.LogError($"Unable to peek next row : {ex.Message}");
    //    return ERowCsvType.Unknown;
    //  } finally {
    //    BaseStream.Position = SavedPosition;
    //  }
    //}
    #endregion --- Peek row type --------------------------------------------

    #region --- ILoggable --------------------------------------------
    /// <summary>
    /// Output logs
    /// </summary>
    public ILogger Logger { get; set; } = ALogger.DEFAULT_LOGGER;

    /// <summary>
    /// Assign the logger to the item
    /// </summary>
    /// <param name="logger">The logger to assign</param>
    public void SetLogger(ILogger logger) {
      Logger = ALogger.Create(logger);
    }
    #endregion --- ILoggable --------------------------------------------

    private static readonly byte[] _EOL = Environment.NewLine.ToByteArray();
    private static readonly int _EOL_LENGTH = _EOL.Length;

    /// <summary>
    /// Read a line from stream
    /// </summary>
    /// <returns></returns>
    public string ReadLine() {
      try {
        using (MemoryStream TempStream = new MemoryStream()) {

          // Read up to EOL or EOF
          while (PeekChar() != -1) {
            TempStream.WriteByte(ReadByte());
            byte[] Buffer = TempStream.ToArray();
            if (Buffer.Length >= _EOL_LENGTH) {
              IEnumerable<byte> LastBytes = Buffer.TakeLast(_EOL_LENGTH);
              if (LastBytes.SequenceEqual(_EOL)) {
                byte[] Line = Buffer.Take(Buffer.Length - _EOL_LENGTH).ToArray();
                string RetVal = ReaderEncoding.GetString(Line);
                Logger.LogDebug(RetVal);
                return RetVal;
              }
            }
          }

          // If here, EOL was not reached. Any byte in buffer ?
          if (TempStream.Length > 0) {
            return ReaderEncoding.GetString(TempStream.ToArray());
          } else {
            return null;
          }

        }
      } catch (EndOfStreamException) {
        return null;

      } catch (Exception ex) {
        Logger.LogError($"Unable to read line from stream : {ex.Message}");
        throw;
      }

    }

    static private byte[] FIELD_SEPARATOR = new byte[] { (byte)ARowCsv.SEPARATOR };

    /// <summary>
    /// Read a the row type from stream
    /// </summary>
    /// <returns></returns>
    public string ReadRowType() {
      try {
        using (MemoryStream TempStream = new MemoryStream()) {

          // Read up to EOL or EOF
          while (PeekChar() != -1) {
            TempStream.WriteByte(ReadByte());
            byte[] Buffer = TempStream.ToArray();

            if (Buffer.EndsWith(FIELD_SEPARATOR)) {
              byte[] Line = Buffer.Take(Buffer.Length - 1).ToArray();
              string RetVal = ReaderEncoding.GetString(Line);
              Logger.LogDebug(RetVal);
              return RetVal;
            }
          }

          // If here, EOL was not reached. Any byte in buffer ?
          if (TempStream.Length > 0) {
            return ReaderEncoding.GetString(TempStream.ToArray());
          } else {
            return null;
          }

        }
      } catch (EndOfStreamException) {
        return null;

      } catch (Exception ex) {
        Logger.LogError($"Unable to read rowType from stream : {ex.Message}");
        throw;
      }

    }

    /**************************************************************************************/

    private IRowCsv _Parse(string rawData) {
      if (rawData is null) {
        return null;
      }

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
    }

  }
}
