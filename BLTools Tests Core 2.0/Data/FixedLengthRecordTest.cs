using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTools;
using BLTools.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Diagnostics;

namespace BLTools.UnitTest.Data {
  [TestClass()]
  public class FixedLengthRecordTest {

    public string RecordString;
    public Encoding RecordEncoding = Encoding.ASCII;

    public string SupplierCode;
    public string Name;
    public string Customer;
    public long OrderNumber;
    public int Quantity;
    public int NegativeQuantity;
    public float FloatCost;
    public float FloatPrice;
    public double DoubleCost;
    public double DoublePrice;
    public decimal DecimalCost;
    public decimal DecimalPrice;
    public bool IsGoodRecord;
    public bool IsAuthentic;
    public bool GoodOrBad;
    public DateTime OrderDate;
    public DateTime DeliveryDate;

    public TFixedLengthRecordTest SourceRecord;

    [TestInitialize()]
    public void MyTestInitialize() {
      #region Comparison values
      SupplierCode = "024";
      Name = "Company name";
      Customer = "Société à vapeur (socvap@test.be)";
      OrderNumber = 123456;
      Quantity = 45;
      NegativeQuantity = -64;
      FloatCost = 56.36f;
      FloatPrice = 67.47f;
      DoubleCost = 56.36d;
      DoublePrice = 67.47d;
      DecimalCost = 56.36m;
      DecimalPrice = 67.47m;
      IsGoodRecord = true;
      IsAuthentic = false;
      GoodOrBad = true;
      OrderDate = new DateTime(2015, 6, 18);
      DeliveryDate = new DateTime(2016, 9, 19);
      #endregion Comparison values

      #region RawData building as string
      StringBuilder sb = new StringBuilder();
      sb.AppendFormat("{0}", SupplierCode.PadRight(3));
      sb.AppendFormat("{0}", Name.PadRight(15));
      sb.AppendFormat("{0}", Customer.PadRight(50));
      sb.AppendFormat("{0}", OrderNumber.ToString().PadLeft(6));
      sb.AppendFormat("{0}", Quantity.ToString().PadLeft(4));
      sb.AppendFormat("{0}{1}", NegativeQuantity >= 0 ? " " : "-", Math.Abs(NegativeQuantity).ToString().PadLeft(3));
      sb.AppendFormat("{0}", FloatCost.ToString("F2", CultureInfo.InvariantCulture).PadLeft(7));
      sb.AppendFormat("{0}", FloatPrice.ToString("F2", CultureInfo.InvariantCulture).Replace(CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator, "").PadLeft(7));
      sb.AppendFormat("{0}", DoubleCost.ToString("F2", CultureInfo.InvariantCulture).PadLeft(7));
      sb.AppendFormat("{0}", DoublePrice.ToString("F2", CultureInfo.InvariantCulture).Replace(CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator, "").PadLeft(7));
      sb.AppendFormat("{0}", DecimalCost.ToString("F2", CultureInfo.InvariantCulture).PadLeft(7));
      sb.AppendFormat("{0}", DecimalPrice.ToString("F2", CultureInfo.InvariantCulture).Replace(CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalSeparator, "").PadLeft(7));
      sb.AppendFormat("{0}", IsGoodRecord ? "Y" : "N");
      sb.AppendFormat("{0}", IsAuthentic ? "T" : "F");
      sb.AppendFormat("{0}", GoodOrBad ? "Good" : "Bad ");
      sb.AppendFormat("{0}", OrderDate.ToString("yyyyMMdd"));
      sb.AppendFormat("{0}", DeliveryDate.ToString("ddMMyyyy"));
      sb.Append("\r\n");
      //RecordString = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(sb.ToString()));
      RecordString = sb.ToString();
      #endregion RawData building as string

      #region SourceRecord fields filling
      SourceRecord = new TFixedLengthRecordTest();
      Trace.WriteLine(SourceRecord.RecLen);
      TFixedLengthRecordTest.IsDebug = true;

      SourceRecord.SupplierCode = SupplierCode;
      SourceRecord.Name = Name;
      SourceRecord.Customer = Customer;
      SourceRecord.OrderNumber = OrderNumber;
      SourceRecord.Quantity = Quantity;
      SourceRecord.NegativeQuantity = NegativeQuantity;
      SourceRecord.FloatCost = FloatCost;
      SourceRecord.FloatPrice = FloatPrice;
      SourceRecord.DoubleCost = DoubleCost;
      SourceRecord.DoublePrice = DoublePrice;
      SourceRecord.DecimalCost = DecimalCost;
      SourceRecord.DecimalPrice = DecimalPrice;
      SourceRecord.IsGoodRecord = IsGoodRecord;
      SourceRecord.IsAuthentic = IsAuthentic;
      SourceRecord.GoodOrBad = GoodOrBad;
      SourceRecord.OrderDate = OrderDate;
      SourceRecord.DeliveryDate = DeliveryDate;
      #endregion SourceRecord fields filling
    }

    #region FromRawData
    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_FromRawData_String_ResultOK() {
      TFixedLengthRecordTest Target = new TFixedLengthRecordTest();
      Target.FromRawData(RecordString, RecordEncoding);
      Assert.AreEqual(SupplierCode, Target.SupplierCode);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_FromRawData_StringWithTrailingSpaces_ResultOK() {
      TFixedLengthRecordTest Target = new TFixedLengthRecordTest();
      Target.FromRawData(RecordString, RecordEncoding);
      Assert.AreEqual(Name, Target.Name);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_FromRawData_StringWithAccentAndSpecials_ResultOK() {
      TFixedLengthRecordTest Target = new TFixedLengthRecordTest();
      Target.FromRawData(RecordString, Encoding.UTF8);
      Assert.AreEqual(Customer, Target.Customer);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_FromRawData_Int_ResultOK() {
      TFixedLengthRecordTest Target = new TFixedLengthRecordTest();
      Target.FromRawData(RecordString, RecordEncoding);
      Assert.AreEqual(Quantity, Target.Quantity);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_FromRawData_Long_ResultOK() {
      TFixedLengthRecordTest Target = new TFixedLengthRecordTest();
      Target.FromRawData(RecordString, RecordEncoding);
      Assert.AreEqual(OrderNumber, Target.OrderNumber);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_FromRawData_FloatWithSeparator_ResultOK() {
      TFixedLengthRecordTest Target = new TFixedLengthRecordTest();
      Target.FromRawData(RecordString, RecordEncoding);
      Assert.AreEqual(FloatCost, Target.FloatCost);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_FromRawData_FloatWithoutSeparator_ResultOK() {
      TFixedLengthRecordTest Target = new TFixedLengthRecordTest();
      Target.FromRawData(RecordString, RecordEncoding);
      Assert.AreEqual(FloatPrice, Target.FloatPrice);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_FromRawData_DoubleWithSeparator_ResultOK() {
      TFixedLengthRecordTest Target = new TFixedLengthRecordTest();
      Target.FromRawData(RecordString, RecordEncoding);
      Assert.AreEqual(DoubleCost, Target.DoubleCost);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_FromRawData_DoubleWithoutSeparator_ResultOK() {
      TFixedLengthRecordTest Target = new TFixedLengthRecordTest();
      Target.FromRawData(RecordString, RecordEncoding);
      Assert.AreEqual(DoublePrice, Target.DoublePrice);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_FromRawData_DecimalWithSeparator_ResultOK() {
      TFixedLengthRecordTest Target = new TFixedLengthRecordTest();
      Target.FromRawData(RecordString, RecordEncoding);
      Assert.AreEqual(DecimalCost, Target.DecimalCost);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_FromRawData_DecimalWithoutSeparator_ResultOK() {
      TFixedLengthRecordTest Target = new TFixedLengthRecordTest();
      Target.FromRawData(RecordString, RecordEncoding);
      Assert.AreEqual(DecimalPrice, Target.DecimalPrice);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_FromRawData_BoolYN_ResultOK() {
      TFixedLengthRecordTest Target = new TFixedLengthRecordTest();
      Target.FromRawData(RecordString, RecordEncoding);
      Assert.AreEqual(IsGoodRecord, Target.IsGoodRecord);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_FromRawData_BoolTF_ResultOK() {
      TFixedLengthRecordTest Target = new TFixedLengthRecordTest();
      Target.FromRawData(RecordString, RecordEncoding);
      Assert.AreEqual(IsAuthentic, Target.IsAuthentic);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_FromRawData_BoolGoodOrBad_ResultOK() {
      TFixedLengthRecordTest Target = new TFixedLengthRecordTest();
      Target.FromRawData(RecordString, RecordEncoding);
      Assert.AreEqual(GoodOrBad, Target.GoodOrBad);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_FromRawData_DateTimeDateOnly_ResultOK() {
      TFixedLengthRecordTest Target = new TFixedLengthRecordTest();
      Target.FromRawData(RecordString, RecordEncoding);
      Assert.AreEqual(OrderDate, Target.OrderDate);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_FromRawData_DateTimeCustom_ResultOK() {
      TFixedLengthRecordTest Target = new TFixedLengthRecordTest();
      Target.FromRawData(RecordString, RecordEncoding);
      Assert.AreEqual(DeliveryDate, Target.DeliveryDate);
    }
    #endregion FromRawData

    #region ToRawData
    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_ToRawData_String_ResultOK() {
      byte[] RawData = SourceRecord.ToRawData(Encoding.ASCII);
      TFixedLengthRecordTest ActualRecord = new TFixedLengthRecordTest();
      try {
        ActualRecord.FromRawData(RawData, Encoding.ASCII);
      } catch { }
      Assert.AreEqual(SupplierCode, ActualRecord.SupplierCode);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_ToRawData_StringWithTrailingSpaces_ResultOK() {
      byte[] RawData = SourceRecord.ToRawData(Encoding.ASCII);
      TFixedLengthRecordTest ActualRecord = new TFixedLengthRecordTest();
      try {
        ActualRecord.FromRawData(RawData, Encoding.ASCII);
      } catch { }
      Assert.AreEqual(Name, ActualRecord.Name);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_ToRawData_StringWithAccentAndSpecials_ResultOK() {
      byte[] RawData = SourceRecord.ToRawData(Encoding.UTF8);
      TFixedLengthRecordTest ActualRecord = new TFixedLengthRecordTest();
      try {
        ActualRecord.FromRawData(RawData, Encoding.UTF8);
      } catch { }
      Assert.AreEqual(Name, ActualRecord.Name);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_ToRawData_Int_ResultOK() {
      byte[] RawData = SourceRecord.ToRawData(Encoding.ASCII);
      TFixedLengthRecordTest ActualRecord = new TFixedLengthRecordTest();
      try {
        ActualRecord.FromRawData(RawData, Encoding.ASCII);
      } catch { }
      Assert.AreEqual(Quantity, ActualRecord.Quantity);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_ToRawData_Long_ResultOK() {
      byte[] RawData = SourceRecord.ToRawData(Encoding.ASCII);
      TFixedLengthRecordTest ActualRecord = new TFixedLengthRecordTest();
      try {
        ActualRecord.FromRawData(RawData, Encoding.ASCII);
      } catch { }
      Assert.AreEqual(OrderNumber, ActualRecord.OrderNumber);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_ToRawData_FloatWithSeparator_ResultOK() {
      byte[] RawData = SourceRecord.ToRawData(Encoding.ASCII);
      TFixedLengthRecordTest ActualRecord = new TFixedLengthRecordTest();
      try {
        ActualRecord.FromRawData(RawData, Encoding.ASCII);
      } catch { }
      Assert.AreEqual(FloatCost, ActualRecord.FloatCost);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_ToRawData_FloatWithoutSeparator_ResultOK() {
      byte[] RawData = SourceRecord.ToRawData(Encoding.ASCII);
      TFixedLengthRecordTest ActualRecord = new TFixedLengthRecordTest();
      try {
        ActualRecord.FromRawData(RawData, Encoding.ASCII);
      } catch { }
      Assert.AreEqual(FloatPrice, ActualRecord.FloatPrice);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_ToRawData_DoubleWithSeparator_ResultOK() {
      byte[] RawData = SourceRecord.ToRawData(Encoding.ASCII);
      TFixedLengthRecordTest ActualRecord = new TFixedLengthRecordTest();
      try {
        ActualRecord.FromRawData(RawData, Encoding.ASCII);
      } catch { }
      Assert.AreEqual(DoubleCost, ActualRecord.DoubleCost);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_ToRawData_DoubleWithoutSeparator_ResultOK() {
      byte[] RawData = SourceRecord.ToRawData(Encoding.ASCII);
      TFixedLengthRecordTest ActualRecord = new TFixedLengthRecordTest();
      try {
        ActualRecord.FromRawData(RawData, Encoding.ASCII);
      } catch { }
      Assert.AreEqual(DoublePrice, ActualRecord.DoublePrice);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_ToRawData_DecimalWithSeparator_ResultOK() {
      byte[] RawData = SourceRecord.ToRawData(Encoding.ASCII);
      TFixedLengthRecordTest ActualRecord = new TFixedLengthRecordTest();
      try {
        ActualRecord.FromRawData(RawData, Encoding.ASCII);
      } catch { }
      Assert.AreEqual(DecimalCost, ActualRecord.DecimalCost);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_ToRawData_DecimalWithoutSeparator_ResultOK() {
      byte[] RawData = SourceRecord.ToRawData(Encoding.ASCII);
      TFixedLengthRecordTest ActualRecord = new TFixedLengthRecordTest();
      try {
        ActualRecord.FromRawData(RawData, Encoding.ASCII);
      } catch { }
      Assert.AreEqual(DecimalPrice, ActualRecord.DecimalPrice);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_ToRawData_BoolYN_ResultOK() {
      byte[] RawData = SourceRecord.ToRawData(Encoding.ASCII);
      TFixedLengthRecordTest ActualRecord = new TFixedLengthRecordTest();
      try {
        ActualRecord.FromRawData(RawData, Encoding.ASCII);
      } catch { }
      Assert.AreEqual(IsGoodRecord, ActualRecord.IsGoodRecord);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_ToRawData_BoolTF_ResultOK() {
      byte[] RawData = SourceRecord.ToRawData(Encoding.ASCII);
      TFixedLengthRecordTest ActualRecord = new TFixedLengthRecordTest();
      try {
        ActualRecord.FromRawData(RawData, Encoding.ASCII);
      } catch { }
      Assert.AreEqual(IsAuthentic, ActualRecord.IsAuthentic);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_ToRawData_BoolGoodBad_ResultOK() {
      byte[] RawData = SourceRecord.ToRawData(Encoding.ASCII);
      TFixedLengthRecordTest ActualRecord = new TFixedLengthRecordTest();
      try {
        ActualRecord.FromRawData(RawData, Encoding.ASCII);
      } catch { }
      Assert.AreEqual(GoodOrBad, ActualRecord.GoodOrBad);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_ToRawData_DateTimeDateOnly_ResultOK() {
      byte[] RawData = SourceRecord.ToRawData(Encoding.ASCII);
      TFixedLengthRecordTest ActualRecord = new TFixedLengthRecordTest();
      try {
        ActualRecord.FromRawData(RawData, Encoding.ASCII);
      } catch { }
      Assert.AreEqual(OrderDate, ActualRecord.OrderDate);
    }

    [TestMethod(), TestCategory("Data"), TestCategory("Fixed length record")]
    public void FixedLengthRecord_ToRawData_DateTimeCustom_ResultOK() {
      byte[] RawData = SourceRecord.ToRawData(Encoding.ASCII);
      TFixedLengthRecordTest ActualRecord = new TFixedLengthRecordTest();
      try {
        ActualRecord.FromRawData(RawData, Encoding.ASCII);
      } catch { }
      Assert.AreEqual(DeliveryDate, ActualRecord.DeliveryDate);
    }




    #endregion ToRawData
  }
}
