// <copyright file="SetCookieAttributeFixture.cs">
//    Copyright © 2013 Dan Piessens.  All rights reserved.
// </copyright>
namespace SpecBind.Tests
{
    using NUnit.Framework;
    using SpecBind.Pages;

    /// <summary>
    /// A test fixture for the SetCookieAttribute class.
    /// </summary>
    [TestFixture]
    public class SetCookieAttributeFixture
    {
        /// <summary>
        /// Tests that the default constructor sets name and value.
        /// </summary>
        [Test]
        public void TestDefaultConstructorSetsNameAndValue()
        {
            var attribute = new SetCookieAttribute("TestName", "TestValue");

            Assert.AreEqual("TestName", attribute.Name);
            Assert.AreEqual("TestValue", attribute.Value);
            Assert.AreEqual("/", attribute.Path);
        }

        /// <summary>
        /// Tests that the set of the path value overrides the default value.
        /// </summary>
        [Test]
        public void TestSetPathOverridesDefaultValue()
        {
            var attribute = new SetCookieAttribute("TestName", "TestValue") { Path = "/MyDomain" };

            Assert.AreEqual("TestName", attribute.Name);
            Assert.AreEqual("TestValue", attribute.Value);
            Assert.AreEqual("/MyDomain", attribute.Path);
        }

        /// <summary>
        /// Tests that the set of the path value overrides the default value.
        /// </summary>
        [Test]
        public void TestToStringReturnsValues()
        {
            var attribute = new SetCookieAttribute("TestName", "TestValue") { Path = "/MyDomain" };

            var result = attribute.ToString();

            StringAssert.Contains("Name: TestName", result);
            StringAssert.Contains("Value: TestValue",result);
            StringAssert.Contains("Path: /MyDomain", result);
        }
    }
}
