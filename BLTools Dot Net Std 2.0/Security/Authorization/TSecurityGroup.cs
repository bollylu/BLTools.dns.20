using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Diagnostics;

namespace BLTools.Security.Authorization {
  public class TSecurityGroup : IToXml {
    public string Id { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public TSecurityUserIdCollection UserIds { get; } = new TSecurityUserIdCollection();
    public TSecurityGroupCollection Groups { get; } = new TSecurityGroupCollection();

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TSecurityGroup() {
      Id = "";
      Name = "";
      Description = "";
    }

    public TSecurityGroup(string name, string description = "") {
      Id = "";
      Name = name;
      Description = description;
    }

    public TSecurityGroup(TSecurityGroup group) {
      Id = group.Id;
      Name = group.Name;
      Description = group.Description;
      UserIds = new TSecurityUserIdCollection(group.UserIds);
      Groups = new TSecurityGroupCollection(group.Groups);
    }

    public TSecurityGroup(XElement group) {
      FromXml(group);
    } 
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    #region --- IToXml --------------------------------------------
    public XElement ToXml() {
      XElement RetVal = new XElement("group");
      RetVal.SetAttributeValue("id", Id);
      RetVal.SetAttributeValue("name", Name);
      RetVal.SetElementValue("description", Description);
      RetVal.Add(UserIds.ToXml());
      RetVal.Add(Groups.ToXml());
      return RetVal;
    }

    public void FromXml(XElement source) {
      if (source is null) {
        string Msg = "Unable to create a TSecurityGroup from a null XElement";
        Trace.WriteLine(Msg, Severity.Error);
        throw new ArgumentNullException(Msg, "group");
      }
      Id = source.SafeReadAttribute<string>("id", "");
      Name = source.SafeReadAttribute<string>("name", "");
      Description = source.SafeReadElementValue<string>("description", "");
      UserIds.FromXml(source.SafeReadElement("userids"));
      Groups.FromXml(source.SafeReadElement("groups"));
    } 
    #endregion --- IToXml --------------------------------------------
  }
}
