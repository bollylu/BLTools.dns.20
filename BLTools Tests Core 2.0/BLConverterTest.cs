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
  }
}
