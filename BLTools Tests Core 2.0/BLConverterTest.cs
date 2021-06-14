using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static BLTools.BLConverter;

namespace BLTools.UnitTest {

  [TestClass]
  public class BLConverterTest {
    private enum ETestEnum {
      Unknown = 0,
      Ok = 1,
      NotOk = 2
    }

    [TestMethod]
    public void ConvertEnum_EnumIsOk_AnswerIsOk() {
      ETestEnum Source = ETestEnum.Ok;
      string TempStorage = Source.ToString();
      ETestEnum Target = BLConvert(TempStorage, ETestEnum.Unknown);
      Assert.AreEqual(ETestEnum.Ok, Target);
    }

    [TestMethod]
    public void ConvertVersion_VersionIsOk_AnswerIsOk() {
      string Source = "1.2.3.4";
      Version Target = BLConvert(Source, Version.Parse("0.0.0.0"));
      Assert.AreEqual(Version.Parse(Source), Target);
    }

    [TestMethod]
    public void ConvertVersion_VersionIsBad_AnswerIsDefault() {
      string Source = "1..-2.3.4";
      Version Target = BLConvert(Source, Version.Parse("0.0.0.0"));
      Console.WriteLine(Target);
      Assert.AreEqual(Version.Parse("0.0.0.0"), Target);
    }

    [TestMethod]
    public void ConvertDoubleArray_InvariantCulture() {
      string Source = "6.2;3.4;1236.598";
      double[] Target = BLConvert<double[]>(Source, CultureInfo.InvariantCulture, null);
      Assert.IsNotNull(Target);
      Assert.AreEqual(3, Target.Count());
      Assert.AreEqual(6.2, Target.First());
      Assert.AreEqual(1236.598, Target.Last());
    }

    [TestMethod]
    public void ConvertDoubleArray_CultureFrFr() {
      string Source = "6,2;3,4;1236,598";
      double[] Target = BLConvert<double[]>(Source, CultureInfo.GetCultureInfo("FR-FR"), null);
      Assert.IsNotNull(Target);
      Assert.AreEqual(3, Target.Count());
      Assert.AreEqual(6.2, Target.First());
      Assert.AreEqual(1236.598, Target.Last());
    }

    [TestMethod]
    public void ConvertLongArrayInAString_LongArray() {
      string Source = "1862365;897564231;632581";
      long[] Target = BLConvert<long[]>(Source);
      Assert.IsNotNull(Target);
      Assert.AreEqual(3, Target.Count());
      Assert.AreEqual(1862365, Target.First());
      Assert.AreEqual(632581, Target.Last());
    }
  }
}
