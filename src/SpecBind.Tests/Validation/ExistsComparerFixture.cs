// <copyright file="ExistsComparerFixture.cs">
//    Copyright © 2013 Dan Piessens.  All rights reserved.
// </copyright>
namespace SpecBind.Tests.Validation
{
    using System.Linq;

    using Moq;
    using NUnit.Framework;
    using SpecBind.Pages;
    using SpecBind.Validation;

    /// <summary>
    /// A test fixture for the exists comparison.
    /// </summary>
    [TestFixture]
    public class ExistsComparerFixture : ComparisonTestBase<ExistsComparer>
    {
        /// <summary>
        /// Tests the rule key property for the correct tags.
        /// </summary>
        [Test]
        public void TestRuleValuesReturnsProperTags()
        {
            var item = new ExistsComparer();

            CollectionAssert.AreEquivalent(new[] { "exists" }, item.RuleKeys.ToList());
        }

        /// <summary>
        /// Tests the properties to ensure no other checks are needed.
        /// </summary>
        [Test]
        public void TestRuleDoesNotCheckForFieldExistenceOrFieldValue()
        {
            var item = new ExistsComparer();

           Assert.IsFalse(item.RequiresFieldValue);
           Assert.IsFalse(item.ShouldCheckElementExistence);
        }

        /// <summary>
        /// Tests the compare method for simple comparisons.
        /// </summary>
        [Test]
        public void TestCompareWithNonBoolValueCase()
        {
            var propertyData = new Mock<IPropertyData>(MockBehavior.Strict);
            propertyData.Setup(p => p.CheckElementExists()).Returns(true);

            RunItemCompareTest("foo", null, true, propertyData);
        }

        /// <summary>
        /// Tests the compare method for simple comparisons.
        /// </summary>
        [Test]
        public void TestCompareExistsCase()
        {
            var propertyData = new Mock<IPropertyData>(MockBehavior.Strict);
            propertyData.Setup(p => p.CheckElementExists()).Returns(true);

            RunItemCompareTest("True", null, true, propertyData);
        }

        /// <summary>
        /// Tests the compare method for simple comparisons.
        /// </summary>
        [Test]
        public void TestCompareNotExistsCase()
        {
            var propertyData = new Mock<IPropertyData>(MockBehavior.Strict);
            propertyData.Setup(p => p.CheckElementExists()).Returns(false);

            RunItemCompareTest("False", null, true, propertyData);
        }
    }
}