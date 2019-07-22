// <copyright file="EndsWithComparerFixture.cs">
//    Copyright © 2013 Dan Piessens  All rights reserved.
// </copyright>

namespace SpecBind.Tests.Validation
{
    using System.Linq;

    using NUnit.Framework;
    using SpecBind.Validation;

    /// <summary>
    /// A test fixture for the ends with string comparison.
    /// </summary>
    [TestFixture]
    public class EndsWithComparerFixture : ComparisonTestBase<EndsWithComparer>
    {
        /// <summary>
        /// Tests the rule key property for the correct tags.
        /// </summary>
        [Test]
        public void TestRuleValuesReturnsProperTags()
        {
            var item = new EndsWithComparer();

            CollectionAssert.AreEquivalent(new[] { "endswith" }, item.RuleKeys.ToList());
        }

        /// <summary>
        /// Tests the ends with method when it matches the substring returns true.
        /// </summary>
        [Test]
        public void TestEndsWithWhenMatchesReturnsTrue()
        {
            RunItemCompareTest("Field", "My Field", true);
        }

        /// <summary>
        /// Tests the ends with method when it doesn't match the substring returns false.
        /// </summary>
        [Test]
        public void TestEndsWithWhenDoesNotMatchReturnsFalse()
        {
            RunItemCompareTest("foo", "My Field", false);
        }

        /// <summary>
        /// Tests the ends with method when the actual value is null returns false.
        /// </summary>
        [Test]
        public void TestEndsWithWhenActualValueIsNullReturnsFalse()
        {
            RunItemCompareTest("foo", null, false);
        }
    }
}