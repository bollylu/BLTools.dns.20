using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using BLTools;

namespace BLTools {
  public static class BinaryReaderExtensions {

    #region --- 48 bits (6 bytes) data (from Pascal/Delphi) --------------------------------------------
    /// <summary>
    /// Read a 48 bits (6 bytes) uint48 into an uint64
    /// </summary>
    /// <param name="reader">The reader being extended</param>
    /// <returns>An unsigned 64 bits integer containing a 48 bits value</returns>
    public static UInt64 ReadUInt48(this BinaryReader reader) {
      try {
        byte[] TempValue = new byte[8];
        Buffer.BlockCopy(reader.ReadBytes(6), 0, TempValue, 0, 6);
        return BitConverter.ToUInt64(TempValue, 0);
      } catch {
        return 0;
      }
    }

    /// <summary>
    /// Read a 48 bits (6 bytes) int48 into an int64
    /// </summary>
    /// <param name="reader">The reader being extended</param>
    /// <returns>An signed 64 bits integer containing a 48 bits value</returns>
    public static Int64 ReadInt48(this BinaryReader reader) {
      try {
        byte[] TempValue = new byte[8];
        byte[] BytesBuffer = reader.ReadBytes(6);

        Buffer.BlockCopy(BytesBuffer, 0, TempValue, 0, 6);

        if (((BytesBuffer.Last() & (byte)0b1000_0000) >> 7) == 1) {
          TempValue[7] = 0b1111_1111;
          TempValue[6] = 0b1111_1111;

        } else {
          TempValue[6] = 0b0000_0000;
          TempValue[7] = 0b0000_0000;
        }

        return BitConverter.ToInt64(TempValue, 0);
      } catch {
        return 0;
      }
    }
    #endregion --- 48 bits (6 bytes) data (from Pascal/Delphi) -----------------------------------------

    #region --- Multiple int16/uint16 values --------------------------------------------
    /// <summary>
    /// Read multiple int16 values
    /// </summary>
    /// <param name="reader">The reader being extended</param>
    /// <param name="count">The maximum quantity of int16 values to read</param>
    /// <returns>The int16 values up to the maximum quantity requested</returns>
    public static IEnumerable<Int16> ReadInt16(this BinaryReader reader, int count) {

      if (count <= 0) {
        yield break;
      }

      int i = 0;
      do {
        Int16 TempValue;
        try {
          TempValue = reader.ReadInt16();
        } catch (EndOfStreamException) {
          yield break;
        }
        yield return TempValue;
      } while (++i < count);
    }

    /// <summary>
    /// Read multiple uint16 values
    /// </summary>
    /// <param name="reader">The reader being extended</param>
    /// <param name="count">The maximum quantity of uint16 values to read</param>
    /// <returns>The uint16 values up to the maximum quantity requested</returns>
    public static IEnumerable<UInt16> ReadUInt16(this BinaryReader reader, int count) {

      if (count <= 0) {
        yield break;
      }

      int i = 0;
      do {
        UInt16 TempValue;
        try {
          TempValue = reader.ReadUInt16();
        } catch (EndOfStreamException) {
          yield break;
        }
        yield return TempValue;
      } while (++i < count);
    }
    #endregion --- Multiple int16/uint16 values -----------------------------------------

    #region --- Multiple int32/uint32 values --------------------------------------------
    /// <summary>
    /// Read multiple int32 values
    /// </summary>
    /// <param name="reader">The reader being extended</param>
    /// <param name="count">The maximum quantity of int32 values to read</param>
    /// <returns>The int32 values up to the maximum quantity requested</returns>
    public static IEnumerable<Int32> ReadInt32(this BinaryReader reader, int count) {
      if (count <= 0) {
        yield break;
      }

      int i = 0;
      do {
        Int32 TempValue;
        try {
          TempValue = reader.ReadInt32();
        } catch (EndOfStreamException) {
          yield break;
        }
        yield return TempValue;
      } while (++i < count);
    }

    /// <summary>
    /// Read multiple uint32 values
    /// </summary>
    /// <param name="reader">The reader being extended</param>
    /// <param name="count">The maximum quantity of uint32 values to read</param>
    /// <returns>The uint32 values up to the maximum quantity requested</returns>
    public static IEnumerable<UInt32> ReadUInt32(this BinaryReader reader, int count) {

      if (count <= 0) {
        yield break;
      }

      int i = 0;
      do {
        UInt32 TempValue;
        try {
          TempValue = reader.ReadUInt32();
        } catch (EndOfStreamException) {
          yield break;
        }
        yield return TempValue;
      } while (++i < count);
    }
    #endregion --- Multiple int32/uint32 values -----------------------------------------

    #region --- Multiple int64/uint64 values --------------------------------------------
    /// <summary>
    /// Read multiple int64 values
    /// </summary>
    /// <param name="reader">The reader being extended</param>
    /// <param name="count">The maximum quantity of int64 values to read</param>
    /// <returns>The int64 values up to the maximum quantity requested</returns>
    public static IEnumerable<Int64> ReadInt64(this BinaryReader reader, int count) {

      if (count <= 0) {
        yield break;
      }

      int i = 0;
      do {
        Int64 TempValue;
        try {
          TempValue = reader.ReadInt64();
        } catch (EndOfStreamException) {
          yield break;
        }
        yield return TempValue;
      } while (++i < count);
    }

    /// <summary>
    /// Read multiple uint64 values
    /// </summary>
    /// <param name="reader">The reader being extended</param>
    /// <param name="count">The maximum quantity of uint64 values to read</param>
    /// <returns>The uint64 values up to the maximum quantity requested</returns>
    public static IEnumerable<UInt64> ReadUInt64(this BinaryReader reader, int count) {

      if (count <= 0) {
        yield break;
      }

      int i = 0;
      do {
        UInt64 TempValue;
        try {
          TempValue = reader.ReadUInt64();
        } catch (EndOfStreamException) {
          yield break;
        }
        yield return TempValue;
      } while (++i < count);
    } 
    #endregion --- Multiple int64/uint64 values -----------------------------------------

    /// <summary>
    /// Read multiple float values
    /// </summary>
    /// <param name="reader">The reader being extended</param>
    /// <param name="count">The maximum quantity of float values to read</param>
    /// <returns>The float values up to the maximum quantity requested</returns>
    public static IEnumerable<float> ReadFloat(this BinaryReader reader, int count) {

      if (count <= 0) {
        yield break;
      }

      int i = 0;
      do {
        float TempValue;
        try {
          TempValue = reader.ReadSingle();
        } catch (EndOfStreamException) {
          yield break;
        }
        yield return TempValue;
      } while (++i < count);
    }

    /// <summary>
    /// Read multiple double values
    /// </summary>
    /// <param name="reader">The reader being extended</param>
    /// <param name="count">The maximum quantity of double values to read</param>
    /// <returns>The double values up to the maximum quantity requested</returns>
    public static IEnumerable<double> ReadDouble(this BinaryReader reader, int count) {

      if (count <= 0) {
        yield break;
      }

      int i = 0;
      do {
        double TempValue;
        try {
          TempValue = reader.ReadDouble();
        } catch (EndOfStreamException) {
          yield break;
        }
        yield return TempValue;
      } while (++i < count);
    }

    /// <summary>
    /// Read multiple sbyte values
    /// </summary>
    /// <param name="reader">The reader being extended</param>
    /// <param name="count">The maximum quantity of sbyte values to read</param>
    /// <returns>The sbyte values up to the maximum quantity requested</returns>
    public static IEnumerable<sbyte> ReadSByte(this BinaryReader reader, int count) {

      if (count <= 0) {
        yield break;
      }

      int i = 0;
      do {
        sbyte TempValue;
        try {
          TempValue = reader.ReadSByte();
        } catch (EndOfStreamException) {
          yield break;
        }
        yield return TempValue;
      } while (++i < count);
    }
  }
}
