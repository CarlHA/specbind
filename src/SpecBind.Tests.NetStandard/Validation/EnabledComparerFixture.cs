// <copyright file="EnabledComparerFixture.cs">
//    Copyright © 2013 Dan Piessens  All rights reserved.
// </copyright>

namespace SpecBind.Tests.Validation
{
    using System.Linq;

    using Moq;
    using NUnit.Framework;
    using SpecBind.Pages;
    using SpecBind.Validation;

    /// <summary>
    /// A test fixture for the enabled comparison.
    /// </summary>
    [TestFixture]
    public class EnabledComparerFixture : ComparisonTestBase<EnabledComparer>
    {
        /// <summary>
        /// Tests the rule key property for the correct tags.
        /// </summary>
        [Test]
        public void TestRuleValuesReturnsProperTags()
        {
            var item = new EnabledComparer();

            CollectionAssert.AreEquivalent(new[] { "enabled", "isenabled" }, item.RuleKeys.ToList());
        }

        /// <summary>
        /// Tests the compare method for simple comparisons.
        /// </summary>
        [Test]
        public void TestCompareWithNonBoolValueCase()
        {
            var propertyData = new Mock<IPropertyData>(MockBehavior.Strict);
            propertyData.Setup(p => p.CheckElementEnabled()).Returns(true);

            RunItemCompareTest("foo", null, true, propertyData);
        }

        /// <summary>
        /// Tests the compare method for simple comparisons.
        /// </summary>
        [Test]
        public void TestCompareEnabledCase()
        {
            var propertyData = new Mock<IPropertyData>(MockBehavior.Strict);
            propertyData.Setup(p => p.CheckElementEnabled()).Returns(true);

            RunItemCompareTest(null, null, true, propertyData);
        }

        /// <summary>
        /// Tests the compare method for simple comparisons.
        /// </summary>
        [Test]
        public void TestCompareNotEnabledCase()
        {
            var propertyData = new Mock<IPropertyData>(MockBehavior.Strict);
            propertyData.Setup(p => p.CheckElementEnabled()).Returns(false);

            RunItemCompareTest(null, null, false, propertyData);
        }
    }
}