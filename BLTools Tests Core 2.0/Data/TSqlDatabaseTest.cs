using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using System.Globalization;
using BLTools.SQL;

namespace BLTools.UnitTest.Core20.Data.Sql {

  [TestClass()]
  public class TSqlDatabaseTest {

    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext {
      get {
        return testContextInstance;
      }
      set {
        testContextInstance = value;
      }
    }

    #region Additional test attributes
    // 
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    //[ClassInitialize()]
    //public static void MyClassInitialize(TestContext testContext)
    //{
    //}
    //
    //Use ClassCleanup to run code after all tests in a class have run
    //[ClassCleanup()]
    //public static void MyClassCleanup()
    //{
    //}
    //
    //Use TestInitialize to run code before running each test
    [TestInitialize()]
    public void MyTestInitialize() {
      
    }
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion

    [TestMethod(), TestCategory("SQL")]
    public void TSqlDatabaseConstructor_NoParams_ResultDefault() {
      TSqlDatabase actual = new TSqlDatabase();
      Assert.AreEqual(TSqlDatabase.DEFAULT_DATABASENAME, actual.DatabaseName);
      Assert.AreEqual(TSqlDatabase.DEFAULT_SERVERNAME, actual.ServerName);
      Assert.AreEqual(TSqlDatabase.DEFAULT_USERNAME, actual.UserName);
      Assert.AreEqual(TSqlDatabase.DEFAULT_PASSWORD, actual.Password);
      Assert.AreEqual(TSqlDatabase.DEFAULT_USE_INTEGRATED_SECURITY, actual.UseIntegratedSecurity);
      Assert.AreEqual(TSqlDatabase.DEFAULT_USE_MARS, actual.UseMars);
      Assert.AreEqual(TSqlDatabase.DEFAULT_USE_POOLED_CONNECTIONS, actual.UsePooledConnections);
      Assert.IsNull(actual.Transaction);
      Assert.IsFalse(actual.IsOpened);
    }

    [TestMethod(), TestCategory("SQL"), ExpectedException(typeof(ArgumentNullException))]
    public void TSqlDatabaseConstructor_EmptyConnectionString_Exception() {
      TSqlDatabase actual = new TSqlDatabase("");
    }

    [TestMethod(), TestCategory("SQL")]
    public void TSqlDatabaseConstructor_ServerAndDbOnly_DefaultUserAndPassword() {
      const string TEST_SERVER = "TestServer";
      const string TEST_DB = "TestDb";
      TSqlDatabase actual = new TSqlDatabase(TEST_SERVER, TEST_DB);
      Assert.AreEqual(TEST_DB, actual.DatabaseName);
      Assert.AreEqual(TEST_SERVER, actual.ServerName);
      Assert.AreEqual(TSqlDatabase.DEFAULT_USERNAME, actual.UserName);
      Assert.AreEqual(TSqlDatabase.DEFAULT_PASSWORD, actual.Password);
    }

    [TestMethod(), TestCategory("SQL")]
    public void TSqlDatabaseConstructor_InvalidConnectionStringSyntax_ResultWithDefaultsExceptCorrectValues() {
      TSqlDatabase actual = new TSqlDatabase("Savername=(locl);Database=test");
      Assert.AreEqual("test", actual.DatabaseName);
      Assert.AreEqual(TSqlDatabase.DEFAULT_SERVERNAME, actual.ServerName);
      Assert.AreEqual(TSqlDatabase.DEFAULT_USERNAME, actual.UserName);
      Assert.AreEqual(TSqlDatabase.DEFAULT_PASSWORD, actual.Password);
      Assert.IsTrue(actual.UseIntegratedSecurity);
      Assert.IsTrue(actual.UseMars);
      Assert.IsTrue(actual.UsePooledConnections);
      Assert.IsNull(actual.Transaction);
      Assert.IsFalse(actual.IsOpened);
    }

    [TestMethod(), TestCategory("SQL")]
    public void TSqlDatabaseConstructor_InvalidConnectionStringDuplicate_ResultWithDefaults() {
      TSqlDatabase actual = new TSqlDatabase("Servername=(local);Database=test;Servername=(local);");
      Assert.AreEqual("test", actual.DatabaseName);
      Assert.AreEqual(TSqlDatabase.DEFAULT_SERVERNAME, actual.ServerName);
      Assert.AreEqual(TSqlDatabase.DEFAULT_USERNAME, actual.UserName);
      Assert.AreEqual(TSqlDatabase.DEFAULT_PASSWORD, actual.Password);
      Assert.IsTrue(actual.UseIntegratedSecurity);
      Assert.IsTrue(actual.UseMars);
      Assert.IsTrue(actual.UsePooledConnections);
      Assert.IsNull(actual.Transaction);
      Assert.IsFalse(actual.IsOpened);
    }

    [TestMethod(), TestCategory("SQL")]
    public void TSqlDatabaseConstructor_CopyConstructor_ResultSameAsSource() {
      const string TEST_SERVER = "TestServer";
      const string TEST_DB = "TestDb";
      const string TEST_USER = "TestUser";
      const string TEST_PASSWORD = "TestPassword";
      TSqlDatabase source = new TSqlDatabase(TEST_SERVER, TEST_DB, TEST_USER, TEST_PASSWORD);
      source.UseMars = false;
      source.UsePooledConnections = false;
      TSqlDatabase actual = new TSqlDatabase(source);
      Assert.AreEqual(TEST_DB, actual.DatabaseName);
      Assert.AreEqual(TEST_SERVER, actual.ServerName);
      Assert.AreEqual(TEST_USER, actual.UserName);
      Assert.AreEqual(TEST_PASSWORD, actual.Password);
      Assert.AreEqual(actual.UseIntegratedSecurity, source.UseIntegratedSecurity);
      Assert.AreEqual(actual.UseMars, source.UseMars);
      Assert.AreEqual(actual.UsePooledConnections, source.UsePooledConnections);
      Assert.AreEqual(actual.Transaction, source.Transaction);
      Assert.AreEqual(actual.IsOpened, source.IsOpened);
    }
  }
}
