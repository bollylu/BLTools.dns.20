using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using BLTools.Diagnostic.Logging;

namespace BLTools.Storage.Csv {

  /// <summary>
  /// Reader for csv
  /// </summary>
  public class TCsvReader : BinaryReader, ILoggable {

    static private readonly byte[] _FIELD_SEPARATOR = new byte[] { (byte)ARowCsv.SEPARATOR };

    private const char DOUBLE_QUOTE = '\"';

#if NETSTANDARD2_0 || NETSTANDARD2_1
    /// <summary>
    /// The encoding for the reader
    /// </summary>
    public Encoding ReaderEncoding { get; set; } = Encoding.UTF8;

    /// <summary>
    /// The end-of-line definition (default to Environment.NewLine)
    /// </summary>
    public string EOL {
      get {
        return string.Join("", _EOL.Cast<char>());
      }
      set {
        _EOL = value.ToByteArray();
        _EOL_Length = _EOL.Length;
      }
    }
#else
    /// <summary>
    /// The encoding for the reader
    /// </summary>
    public Encoding ReaderEncoding { get; init; } = Encoding.UTF8;

    /// <summary>
    /// The end-of-line definition (default to Environment.NewLine)
    /// </summary>
    public string EOL {
      get {
        return string.Join("", _EOL.Cast<char>());
      }
      init {
        _EOL = value.ToByteArray();
        _EOL_Length = _EOL.Length;
      }
    }
#endif

    private byte[] _EOL;
    private int _EOL_Length;

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    /// <summary>
    /// Create a new csv reader
    /// </summary>
    /// <param name="inputStream">The stream to read from</param>
    /// <param name="leaveOpen">tue to leave the stream open when closing the reader</param>
    public TCsvReader(Stream inputStream, bool leaveOpen = true) : base(inputStream, Encoding.UTF8, leaveOpen) {
      ReaderEncoding = Encoding.UTF8;
      EOL = Environment.NewLine;
    }

    /// <summary>
    /// Create a new csv reader
    /// </summary>
    /// <param name="inputStream">The stream to read from</param>
    /// <param name="encoding">the encoding of the stream</param>
    /// <param name="leaveOpen">tue to leave the stream open when closing the reader</param>
    public TCsvReader(Stream inputStream, Encoding encoding, bool leaveOpen = true) : base(inputStream, encoding, leaveOpen) {
      ReaderEncoding = encoding;
      EOL = Environment.NewLine;
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
      return ARowCsv.Parse(RawData);
    }

    /// <summary>
    /// Read all the up to EOF
    /// </summary>
    /// <returns></returns>
    public IRowCsv[] ReadAll() {
      List<IRowCsv> RetVal = new List<IRowCsv>();
      string RawData = ReadLine();
      while (RawData != null) {
        IRowCsv NewRow = ARowCsv.Parse(RawData);
        RetVal.Add(NewRow);
        RawData = ReadLine();
      }
      return RetVal.ToArray();
    }
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
              return ARowCsv.Parse(RawData);
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

    // <summary>
    /// Obtain the next row header
    /// </summary>
    /// <returns></returns>
    public async Task<IRowCsv> ReadHeaderAsync(bool findIt = false) {
      ERowCsvType RowType = await PeekRowTypeAsync().ConfigureAwait(false);
      while (RowType != ERowCsvType.None) {
        switch (RowType) {
          case ERowCsvType.Header:
            string RawData = await ReadLineAsync().ConfigureAwait(false);
            return ARowCsv.Parse(RawData);
          case ERowCsvType.Data:
          case ERowCsvType.Footer:
          case ERowCsvType.Unknown:
            if (findIt) {
              await ReadLineAsync().ConfigureAwait(false);
            } else {
              return null;
            }
            break;
        }
        RowType = await PeekRowTypeAsync().ConfigureAwait(false);
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
              return ARowCsv.Parse(RawData);
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

    #endregion --- Read footer --------------------------------------------

    #region --- Read data --------------------------------------------
    /// <summary>
    /// Obtain the next row of data
    /// If not in data section, read until end or first data row
    /// </summary>
    /// <param name="findIt">true to continue reading until next data or EOS</param>
    /// <returns>A csv formatted row</returns>
    public IRowCsv ReadData(bool findIt = false) {
      ERowCsvType RowType = PeekRowType();
      while (RowType != ERowCsvType.None) {
        switch (RowType) {
          case ERowCsvType.Data: {
              string RawData = ReadLine();
              return ARowCsv.Parse(RawData);
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

            if (Buffer.EndsWith(_FIELD_SEPARATOR)) {
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

    /// <summary>
    /// Read the next row type without moving the stream pointer
    /// </summary>
    /// <returns></returns>
    public async Task<ERowCsvType> PeekRowTypeAsync() {
      return await Task.Run<ERowCsvType>(() => PeekRowType(), new CancellationTokenSource(1000).Token).ConfigureAwait(false);
    }
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



    /// <summary>
    /// Read a line from stream
    /// </summary>
    /// <returns></returns>
    public string ReadLine() {
      // Test for EOS
      if (PeekChar() == -1) {
        return null;
      }

      try {
        using (MemoryStream TempStream = new MemoryStream()) {

          // Read up to EOL or EOF
          while (PeekChar() != -1) {
            TempStream.WriteByte(ReadByte());
            byte[] Buffer = TempStream.ToArray();
            if (Buffer.Length >= _EOL_Length) {
              IEnumerable<byte> LastBytes = Buffer.TakeLast(_EOL_Length);
              if (LastBytes.SequenceEqual(_EOL)) {
                byte[] Line = Buffer.Take(Buffer.Length - _EOL_Length).ToArray();
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

    /// <summary>
    /// Read a line from stream
    /// </summary>
    /// <returns></returns>
    public async Task<string> ReadLineAsync() {
      return await Task.Run<string>(() => ReadLine(), new CancellationTokenSource(1000).Token).ConfigureAwait(false);
    }


    ///// <summary>
    ///// Read a line from stream
    ///// </summary>
    ///// <returns></returns>
    //public async Task<string> ReadLineAsync() {
    //  // Test for EOS
    //  if (PeekChar() == -1) {
    //    return null;
    //  }

    //  try {


    //    using (MemoryStream TempStream = new MemoryStream()) {
    //      await BaseStream.CopyToAsync(TempStream);
    //      ReadOnlyMemory<byte> Data = new ReadOnlyMemory<byte>(TempStream.ToArray());

    //      int MaxLength = Data.Length;
    //      int Counter = 0;

    //      while (Counter < MaxLength) {
    //        ReadOnlyMemory<byte> TestEOL = Data.Slice(0, Counter);
    //        ReadOnlyMemory<byte> PossibleEOL = TestEOL.Slice(TestEOL.Length - _EOL_Length, TestEOL.Length);
    //        if (PossibleEOL.ToArray().SequenceEqual(_EOL)) {
    //          string RetVal = ReaderEncoding.GetString(TestEOL.Slice(0, TestEOL.Length-_EOL_Length).ToArray());
    //          return RetVal;
    //        }
    //        Counter++;
    //      }

    //      //// Read up to EOL or EOF
    //      //while (PeekChar() != -1) {
    //      //  TempStream.WriteByte(ReadByte());
    //      //  byte[] Buffer = TempStream.ToArray();
    //      //  if (Buffer.Length >= _EOL_Length) {
    //      //    IEnumerable<byte> LastBytes = Buffer.TakeLast(_EOL_Length);
    //      //    if (LastBytes.SequenceEqual(_EOL)) {
    //      //      byte[] Line = Buffer.Take(Buffer.Length - _EOL_Length).ToArray();
    //      //      string RetVal = ReaderEncoding.GetString(Line);
    //      //      Logger.LogDebug(RetVal);
    //      //      return RetVal;
    //      //    }
    //      //  }
    //      //}

    //      // If here, EOL was not reached. Any byte in buffer ?
    //      if (TempStream.Length > 0) {
    //        return ReaderEncoding.GetString(TempStream.ToArray());
    //      } else {
    //        return null;
    //      }

    //    }
    //  } catch (EndOfStreamException) {
    //    return null;

    //  } catch (Exception ex) {
    //    Logger.LogError($"Unable to read line from stream : {ex.Message}");
    //    throw;
    //  }

    //}


    /// <summary>
    /// Read next item in current line
    /// </summary>
    /// <returns></returns>
    public string ReadNextItem() {
      // Test for EOS
      if (PeekChar() == -1) {
        return null;
      }

      // Ok, something left to read
      try {
        using (MemoryStream TempStream = new MemoryStream()) {
          bool IsAString = false;
          if (PeekChar() == DOUBLE_QUOTE) {
            IsAString = true;
          }

#if NETSTANDARD2_0
          // Read up to SEPARATOR or EOL or EOF
          while (PeekChar() != -1) {

            TempStream.WriteByte(ReadByte());
            byte[] Bytes = TempStream.ToArray();

            if (Bytes.EndsWith(_FIELD_SEPARATOR)) {
              int UsefulDataCount = Bytes.Length - 1;
              byte[] Item = new byte[UsefulDataCount];
              Buffer.BlockCopy(Bytes, 0, Item, 0, UsefulDataCount);
              string RetVal = ReaderEncoding.GetString(Item);
              Logger.LogDebug(RetVal);
              if (IsAString) {
                return RetVal.RemoveExternalQuotes();
              } else {
                return RetVal;
              }
            }

            if (Bytes.EndsWith(_FIELD_SEPARATOR)) {
              int UsefulDataCount = Bytes.Length - 1;
              byte[] Item = new byte[UsefulDataCount];
              Buffer.BlockCopy(Bytes, 0, Item, 0, UsefulDataCount);
              string RetVal = ReaderEncoding.GetString(Item);
              Logger.LogDebug(RetVal);
              if (IsAString) {
                return RetVal.RemoveExternalQuotes();
              } else {
                return RetVal;
              }
            }
          }

#else
          // Read up to SEPARATOR or EOL or EOF
          while (PeekChar() != -1) {

            TempStream.WriteByte(ReadByte());
            byte[] Bytes = TempStream.ToArray();

            if (Bytes.EndsWith(_FIELD_SEPARATOR)) {
              int UsefulDataCount = Bytes.Length - 1;
              string RetVal = ReaderEncoding.GetString(Bytes.AsSpan(0, UsefulDataCount));
              Logger.LogDebug(RetVal);
              if (IsAString) {
                return RetVal.RemoveExternalQuotes();
              } else {
                return RetVal;
              }
            }

            if (Bytes.EndsWith(_EOL)) {
              int UsefulDataCount = Bytes.Length - _EOL_Length;
              byte[] Item = new byte[UsefulDataCount];
              string RetVal = ReaderEncoding.GetString(Bytes.AsSpan(0, UsefulDataCount));
              Logger.LogDebug(RetVal);
              if (IsAString) {
                return RetVal.RemoveExternalQuotes();
              } else {
                return RetVal;
              }
            }
          }
#endif
          // If here, EOL was not reached and we are EOS. Any byte in buffer ?
          if (TempStream.Length > 0) {
            string RetVal = ReaderEncoding.GetString(TempStream.ToArray());
            if (IsAString) {
              return RetVal.RemoveExternalQuotes();
            } else {
              return RetVal;
            }
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

    /// <summary>
    /// Read a the row type from stream
    /// </summary>
    /// <returns></returns>
    public string ReadRowType() {

      return ReadNextItem();

    }

    /**************************************************************************************/



  }
}
