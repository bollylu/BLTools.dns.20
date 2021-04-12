using System;
using System.Collections.Generic;
using System.Text;

using BLTools.Storage.Csv;

namespace BLTools.UnitTest.Storage {
  public static class HCsvStorageGenerator {

    public static IEnumerable<string> HeaderOnlyContentArray() {
      string[] RetVal = new string[] {
      $"{ERowCsvType.Header.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Title".WithQuotes()}{ARowCsv.SEPARATOR}{"Here's a title".WithQuotes()}",
      $"{ERowCsvType.Header.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Name".WithQuotes()}{ARowCsv.SEPARATOR}{"My name".WithQuotes()}",
      $"{ERowCsvType.Header.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Description".WithQuotes()}{ARowCsv.SEPARATOR}{"My description".WithQuotes()}"
      };
      return RetVal;
    }

    public static string HeaderOnlyContentFile() {
      return string.Join(Environment.NewLine, HeaderOnlyContentArray()) + Environment.NewLine;
    }

    public static IEnumerable<string> HeaderAndDataContentArray() {
      string[] RetVal = new string[] {
      $"{ERowCsvType.Header.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Title".WithQuotes()}{ARowCsv.SEPARATOR}{"Here's a title".WithQuotes()}",
      $"{ERowCsvType.Header.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Name".WithQuotes()}{ARowCsv.SEPARATOR}{"My name".WithQuotes()}",
      $"{ERowCsvType.Header.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Description".WithQuotes()}{ARowCsv.SEPARATOR}{"My description".WithQuotes()}",
      $"{ERowCsvType.Data.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Channels".WithQuotes()}{ARowCsv.SEPARATOR}{"1;2;3;4;5;6"}",
      $"{ERowCsvType.Data.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Energies".WithQuotes()}{ARowCsv.SEPARATOR}{"25;36;43;84;65;26"}"
      };
      return RetVal;
    }

    public static string HeaderAndDataContentFile() {
      return string.Join(Environment.NewLine, HeaderAndDataContentArray()) + Environment.NewLine;
    }

    public static IEnumerable<string> MultipleHeaderAndDataContent() {
      string[] RetVal = new string[] {
      $"{ERowCsvType.Header.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Title".WithQuotes()}{ARowCsv.SEPARATOR}{"18F".WithQuotes()}",
      $"{ERowCsvType.Header.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Name".WithQuotes()}{ARowCsv.SEPARATOR}{"Curve1".WithQuotes()}",
      $"{ERowCsvType.Header.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Description".WithQuotes()}{ARowCsv.SEPARATOR}{"18F curve 1".WithQuotes()}",
      $"{ERowCsvType.Data.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Channels".WithQuotes()}{ARowCsv.SEPARATOR}{"1;2;3;4;5;6"}",
      $"{ERowCsvType.Data.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Energies".WithQuotes()}{ARowCsv.SEPARATOR}{"25;36;43;84;65;26"}",
      $"{ERowCsvType.Header.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Title".WithQuotes()}{ARowCsv.SEPARATOR}{"18F".WithQuotes()}",
      $"{ERowCsvType.Header.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Name".WithQuotes()}{ARowCsv.SEPARATOR}{"Curve2".WithQuotes()}",
      $"{ERowCsvType.Header.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Description".WithQuotes()}{ARowCsv.SEPARATOR}{"18F curve 2".WithQuotes()}",
      $"{ERowCsvType.Data.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Channels".WithQuotes()}{ARowCsv.SEPARATOR}{"1;2;3;4;5;6"}",
      $"{ERowCsvType.Data.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Energies".WithQuotes()}{ARowCsv.SEPARATOR}{"28;34;47;92;66;26"}"
      };
      return RetVal;
    }

    public static IEnumerable<string> MultipleHeaderAndDataAndFooterArray() {
      string[] RetVal = new string[] {
      $"{ERowCsvType.Header.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Title".WithQuotes()}{ARowCsv.SEPARATOR}{"18F".WithQuotes()}",
      $"{ERowCsvType.Header.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Name".WithQuotes()}{ARowCsv.SEPARATOR}{"Curve1".WithQuotes()}",
      $"{ERowCsvType.Header.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Description".WithQuotes()}{ARowCsv.SEPARATOR}{"18F curve 1".WithQuotes()}",
      $"{ERowCsvType.Data.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Channels".WithQuotes()}{ARowCsv.SEPARATOR}{"1;2;3;4;5;6"}",
      $"{ERowCsvType.Data.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Energies".WithQuotes()}{ARowCsv.SEPARATOR}{"25;36;43;84;65;26"}",
      $"{ERowCsvType.Header.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Title".WithQuotes()}{ARowCsv.SEPARATOR}{"18F".WithQuotes()}",
      $"{ERowCsvType.Header.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Name".WithQuotes()}{ARowCsv.SEPARATOR}{"Curve2".WithQuotes()}",
      $"{ERowCsvType.Header.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Description".WithQuotes()}{ARowCsv.SEPARATOR}{"18F curve 2".WithQuotes()}",
      $"{ERowCsvType.Data.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Channels".WithQuotes()}{ARowCsv.SEPARATOR}{"1;2;3;4;5;6"}",
      $"{ERowCsvType.Data.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Energies".WithQuotes()}{ARowCsv.SEPARATOR}{"28;34;47;92;66;26"}",
      $"{ERowCsvType.Footer.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Channels".WithQuotes()}{ARowCsv.SEPARATOR}{"1;2;3;4;5;6"}",
      $"{ERowCsvType.Footer.ToString().WithQuotes()}{ARowCsv.SEPARATOR}{"Energies".WithQuotes()}{ARowCsv.SEPARATOR}{"53;70;90;176;121;52"}",
      };
      return RetVal;
    }

    public static string MultipleHeaderAndDataAndFooterFile() {
      return string.Join(Environment.NewLine, MultipleHeaderAndDataAndFooterArray()) + Environment.NewLine;
    }
  }
}
