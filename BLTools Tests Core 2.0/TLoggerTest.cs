using System;
using System.Collections.Generic;
using System.Text;
using BLTools.Diagnostic.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace BLTools.UnitTest
{
    [TestClass()]
    public class TLoggerTest
    {
        [TestMethod]
        public void TFileLogger_ConstructorWithValidFilename_AllPropertiesInitialized()
        {
            using ( TFileLogger Target = new TFileLogger("c:\\test.log") )
            {
                Assert.IsNotNull(Target.Filename);
                Assert.AreEqual(ESeverity.Info, Target.SeverityLimit);
            }
        }

        [TestMethod]
        public void TConsoleLogger_ConstructorEmpty_AllPropertiesInitialized()
        {
            using ( TConsoleLogger Target = new TConsoleLogger() )
            {
                Assert.AreEqual(ESeverity.Info, Target.SeverityLimit);
            }
        }

        [TestMethod]
        public void TTraceLogger_ConstructorEmpty_AllPropertiesInitialized()
        {
            using ( TTraceLogger Target = new TTraceLogger() )
            {
                Assert.AreEqual(ESeverity.Info, Target.SeverityLimit);
            }
        }
    }
}
