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

    /// <summary>
    /// The list of rows from the file
    /// </summary>
    protected List<IRowCsv> _Content = new List<IRowCsv>();

    /// <inheritdoc/>
    public virtual bool Load(string filename) {
      throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public virtual Task<bool> LoadAsync(string filename) {
      throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public virtual bool LoadHeader(string filename) {
      _EnsureFilename(filename);
      _EnsureFileExists(filename);

      _Content.Clear();

      try {
        using (FileStream ReadStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read)) {
          using (TCsvReader Reader = new TCsvReader(ReadStream, Encoding.UTF8)) {
            IRowCsv NextRow = Reader.ReadHeader(true);
            while(NextRow != null) {
              _Content.Add(NextRow);
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
    public virtual Task<bool> LoadHeaderAsync(string filename) {
      _EnsureFilename(filename);
      _EnsureFileExists(filename);

      _Content.Clear();

      try {
        using (FileStream ReadStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read)) {
          using (TCsvReader Reader = new TCsvReader(ReadStream, Encoding.UTF8)) {
            IRowCsv NextRow = Reader.ReadHeader(true);
            while (NextRow != null) {
              _Content.Add(NextRow);
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
    public virtual bool Save(string filename, bool overwrite = true) {
      throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public virtual Task<bool> SaveAsync(string filename, bool overwrite = true) {
      throw new NotImplementedException();
    }


    /************************************************************************************/

    /// <summary>
    /// Ensure that the filename is passed
    /// </summary>
    /// <param name="filename"></param>
    protected void _EnsureFilename(string filename) {
      if (string.IsNullOrWhiteSpace(filename)) {
        const string Message = "Unable to work with file : filename is missing";
        LogError(Message);
        throw new ArgumentException(Message, nameof(filename));
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
  }
}
