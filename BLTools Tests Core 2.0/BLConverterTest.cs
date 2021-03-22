using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static BLTools.BLConverter;

namespace BLTools.UnitTest {
  
  [TestClass]
  public class BLConverterTest {
    private enum ETestEnum {
      Unknown=0,
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
      Version Source = Version.Parse("1.2.3.4");
      string TempStorage = Source.ToString();
      Version Target = BLConvert(TempStorage, Version.Parse("0.0.0.0"));
      Assert.AreEqual(Source, Target);
    }
  }
}
