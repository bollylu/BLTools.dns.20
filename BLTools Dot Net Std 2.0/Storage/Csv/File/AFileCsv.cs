using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BLTools.Diagnostic.Logging;
using BLTools.Storage.Csv;

namespace BLTools.Storage.Csv {
  /// <summary>
  /// Base implementation for a file that contains data in csv format
  /// </summary>
  public abstract class AFileCsv : ALoggable, IFileCsv {

    /// <inheritdoc/>
    public string Filename { get; protected set; }

#if NETSTANDARD2_0 || NETSTANDARD2_1
    /// <inheritdoc/>
    public Encoding FileEncoding { get; set; } = Encoding.UTF8;
#else
    /// <inheritdoc/>
    public Encoding FileEncoding { get; init; } = Encoding.UTF8;
#endif


    /// <summary>
    /// The list of rows from the file
    /// </summary>
    protected List<IRowCsv> _Content = new List<IRowCsv>();

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    protected AFileCsv() { }
    protected AFileCsv(string filename) {
      Filename = filename;
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    /// <inheritdoc/>
    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendLine($"Csv file : {Filename}, {_Content.Count} rows");
      foreach (IRowCsv RowItem in _Content) {
        RetVal.AppendLine(RowItem.ToString());
      }
      return RetVal.ToString();
    }

    #region --- Sync I/O methods --------------------------------------------
    /// <inheritdoc/>
    public virtual bool Load() {
      return Load(Filename);
    }

    /// <inheritdoc/>
    public virtual bool Load(string filename) {
      #region === Validate parameters ===
      _EnsureFilename(filename);
      _EnsureFileExists(filename);
      if (Filename != filename) {
        Filename = filename;
      }
      #endregion === Validate parameters ===

      _Content.Clear();

      try {
        using (FileStream SourceStream = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.Read)) {
          using (TCsvReader Reader = new TCsvReader(SourceStream) { ReaderEncoding = FileEncoding }) {
            _Content.AddRange(Reader.ReadAll());
            return true;
          }
        }
      } catch (Exception ex) {
        LogError($"Unable to read content of {Filename} : {ex.Message}");
        return false;
      }

    }

    /// <inheritdoc/>
    public virtual bool LoadHeader() {
      return LoadHeader(Filename);
    }

    /// <inheritdoc/>
    public virtual bool LoadHeader(string filename) {
      #region === Validate parameters ===
      _EnsureFilename(filename);
      _EnsureFileExists(filename);

      if (Filename != filename) {
        Filename = filename;
      }
      #endregion === Validate parameters ===

      _Content.Clear();

      try {
        using (FileStream ReadStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read)) {
          using (TCsvReader Reader = new TCsvReader(ReadStream) { ReaderEncoding = FileEncoding }) {
            IRowCsv NextRow = Reader.ReadHeader(true);
            while (NextRow != null) {
              _Content.Add(NextRow);
              NextRow = Reader.ReadHeader(true);
            }
          }
        }
        return true;
      } catch (Exception ex) {
        LogError($"Unable to load header for file {filename} : {ex.Message}");
        throw;
      }
    }

    /// <inheritdoc/>
    public virtual bool Save(bool overwrite = true) {
      return SaveAs(Filename, overwrite);
    }

    /// <inheritdoc/>
    public virtual bool SaveAs(string filename, bool overwrite = true) {
      #region === Validate parameters ===
      _EnsureFilename(filename);

      if (Filename != filename) {
        Filename = filename;
      }

      if (_Content.IsEmpty()) {
        return true;
      }
      #endregion === Validate parameters ===

      try {
        if (overwrite && File.Exists(Filename)) {
          File.Delete(Filename);
        }

        using (FileStream TargetStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None)) {
          using (TCsvWriter Writer = new TCsvWriter(TargetStream, true) { WriterEncoding = FileEncoding }) {
            foreach (IRowCsv RowItem in _Content) {
              Writer.WriteRow(RowItem);
            }
          }
        }

      } catch (Exception ex) {
        LogError($"Unable to save csv to {Filename} : {ex.Message}");
        return false;
      }

      return true;
    }
    #endregion --- Sync I/O methods --------------------------------------------

    #region --- Async I/O methods --------------------------------------------
    /// <inheritdoc/>
    public virtual async Task<bool> LoadAsync() {
      return await LoadAsync(Filename).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual Task<bool> LoadAsync(string filename) {
      #region === Validate parameters ===
      _EnsureFilename(filename);
      _EnsureFileExists(filename);

      if (Filename != filename) {
        Filename = filename;
      }
      #endregion === Validate parameters ===

      _Content.Clear();

      try {
        using (FileStream ReadStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 16000, true)) {
          using (TCsvReader Reader = new TCsvReader(ReadStream, Encoding.UTF8)) {
            IRowCsv NextRow = Reader.ReadRow();
            while (NextRow != null) {
              _Content.Add(NextRow);
              NextRow = Reader.ReadRow();
            }
          }
        }
        return Task.FromResult(true);
      } catch (Exception ex) {
        LogError($"Unable to load header for file {filename} : {ex.Message}");
        throw;
      }
    }

    /// <inheritdoc/>
    public virtual async Task<bool> LoadHeaderAsync() {
      return await LoadHeaderAsync(Filename).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual Task<bool> LoadHeaderAsync(string filename) {
      #region === Validate parameters ===
      _EnsureFilename(filename);
      _EnsureFileExists(filename);

      if (Filename != filename) {
        Filename = filename;
      }
      #endregion === Validate parameters ===

      _Content.Clear();

      try {
        using (FileStream ReadStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 16000, true)) {
          using (TCsvReader Reader = new TCsvReader(ReadStream, Encoding.UTF8)) {
            IRowCsv NextRow = Reader.ReadHeader(true);
            while (NextRow != null) {
              _Content.Add(NextRow);
              NextRow = Reader.ReadHeader(true);
            }
          }
        }
        return Task.FromResult(true);
      } catch (Exception ex) {
        LogError($"Unable to load header for file {filename} : {ex.Message}");
        throw;
      }
    }

    /// <inheritdoc/>
    public virtual async Task<bool> SaveAsync(bool overwrite = true) {
      return await SaveAsAsync(Filename, overwrite).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual Task<bool> SaveAsAsync(string filename, bool overwrite = true) {
      #region === Validate parameters ===
      _EnsureFilename(filename);

      if (Filename != filename) {
        Filename = filename;
      }

      if (_Content.IsEmpty()) {
        return Task.FromResult(true);
      }
      #endregion === Validate parameters ===

      try {
        if (overwrite && File.Exists(Filename)) {
          File.Delete(Filename);
        }

        using (FileStream TargetStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 16000, true)) {
          using (TCsvWriter Writer = new TCsvWriter(TargetStream, true) { WriterEncoding = FileEncoding }) {
            foreach (IRowCsv RowItem in _Content) {
              Writer.WriteRow(RowItem);
            }
          }
        }

      } catch (Exception ex) {
        LogError($"Unable to save csv to {Filename} : {ex.Message}");
        return Task.FromResult(false);
      }

      return Task.FromResult(true);
    }
    #endregion --- Async I/O methods --------------------------------------------

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

    #region --- Memory content --------------------------------------------
    /// <inheritdoc/>
    public IRowCsv[] GetAll() {
      if (_Content.IsEmpty()) {
        return new IRowCsv[0];
      }
      return _Content.ToArray();
    }

    /// <inheritdoc/>
    public IRowCsv[] GetHeaders() {
      if (_Content.IsEmpty()) {
        return new IRowCsv[0];
      }
      return _Content.OfType<TRowCsvHeader>().ToArray();
    }

    /// <inheritdoc/>
    public IRowCsv[] GetData() {
      if (_Content.IsEmpty()) {
        return new IRowCsv[0];
      }
      return _Content.OfType<TRowCsvData>().ToArray();
    }

    /// <inheritdoc/>
    public IRowCsv[] GetFooters() {
      if (_Content.IsEmpty()) {
        return new IRowCsv[0];
      }
      return _Content.OfType<TRowCsvFooter>().ToArray();
    }

    /// <inheritdoc/>
    public IRowCsv GetRow(ERowCsvType rowType, string id) {
      if (_Content.IsEmpty()) {
        return null;
      }
      return _Content.FirstOrDefault(r => r.RowType == rowType && r.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));
    }

    /// <inheritdoc/>
    public void AddRow(IRowCsv row) {
      _Content.Add(row);
    }
    #endregion --- Memory content --------------------------------------------
  }
}
