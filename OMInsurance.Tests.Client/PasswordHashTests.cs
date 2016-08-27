using Microsoft.VisualStudio.TestTools.UnitTesting;
using OMInsurance.Utils;

namespace OMInsurance.Tests
{
    [TestClass]
    public class DBUtilsTest
    {
        [TestMethod]
        public void PasswordHash_Test()
        {
            string s1 = PasswordHash.CreateHash("123");
            string s2 = PasswordHash.CreateHash("123");
            Assert.IsTrue(PasswordHash.ValidatePassword("123", s2));
        }
    }
}
