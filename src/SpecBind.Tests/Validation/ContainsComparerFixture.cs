// <copyright file="ContainsComparerFixture.cs">
//    Copyright © 2013 Dan Piessens  All rights reserved.
// </copyright>


namespace SpecBind.Tests.Validation
{
    using System.Linq;
    using NUnit.Framework;
    using SpecBind.Validation;

    /// <summary>
    /// A test fixture for the contains string comparison.
    /// </summary>
    [TestFixture]
    public class ContainsComparerFixture : ComparisonTestBase<ContainsComparer>
    {
        /// <summary>
        /// Tests the rule key property for the correct tags.
        /// </summary>
        [Test]
        public void TestRuleValuesReturnsProperTags()
        {
            var item = new ContainsComparer();

            CollectionAssert.AreEquivalent(new[] { "contains" }, item.RuleKeys.ToList());
        }

        /// <summary>
        /// Tests the contains method when it matches the substring returns true.
        /// </summary>
        [Test]
        public void TestContainsWhenMatchesReturnsTrue()
        {
            RunItemCompareTest("Fie", "My Field", true);
        }

        /// <summary>
        /// Tests the contains method when it doesn't match the substring returns false.
        /// </summary>
        [Test]
        public void TestContainsWhenDoesNotMatchReturnsFalse()
        {
            RunItemCompareTest("foo", "My Field", false);
        }

        /// <summary>
        /// Tests the contains method when the actual value is null returns false.
        /// </summary>
        [Test]
        public void TestContainsWhenActualValueIsNullReturnsFalse()
        {
            RunItemCompareTest("foo", null, false);
        }
    }
}