using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BLTools.Security.Authorization {
  public class TSecurityUserIdCollection : List<string>, IToXml {
    public TSecurityUserIdCollection() {}

    public TSecurityUserIdCollection(TSecurityUserIdCollection userIds) {
      foreach (string UserIdItem in userIds) {
        Add(UserIdItem);
      }
    }

    public TSecurityUserIdCollection(IEnumerable<string> userIds) {
      foreach (string UserIdItem in userIds) {
        Add(UserIdItem);
      }
    }

    public TSecurityUserIdCollection(IEnumerable<XElement> userIds) {
      foreach (XElement UserIdItem in userIds) {
        Add(UserIdItem.SafeReadAttribute<string>("id"));
      }
    }

    public TSecurityUserIdCollection(XElement userIds) {
      FromXml(userIds);
    }

    public void FromXml(XElement source) {
      Clear();
      foreach (XElement UserIdItem in source.Elements("userid")) {
        Add(UserIdItem.SafeReadAttribute<string>("id"));
      }
    }

    public XElement ToXml() {
      XElement RetVal = new XElement("userids");
      foreach (string UserIdItem in this) {
        RetVal.Add(new XElement("userid", new XAttribute("id", UserIdItem)));
      }
      return RetVal;
    }
  }
}
