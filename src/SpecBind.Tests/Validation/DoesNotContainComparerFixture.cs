// <copyright file="DoesNotContainComparerFixture.cs">
//    Copyright © 2013 Dan Piessens  All rights reserved.
// </copyright>

namespace SpecBind.Tests.Validation
{
    using System.Linq;

    using NUnit.Framework;
    using SpecBind.Validation;

    /// <summary>
    /// A test fixture for the does not contain comparison.
    /// </summary>
    [TestFixture]
    public class DoesNotContainComparerFixture : ComparisonTestBase<DoesNotContainComparer>
    {
        /// <summary>
        /// Tests the rule key property for the correct tags.
        /// </summary>
        [Test]
        public void TestRuleValuesReturnsProperTags()
        {
            var item = new DoesNotContainComparer();

            CollectionAssert.AreEquivalent(new[] { "doesnotcontain" }, item.RuleKeys.ToList());
        }

        /// <summary>
        /// Tests the does not contain method when it matches the substring returns false.
        /// </summary>
        [Test]
        public void TestDoesNotContainWhenMatchesReturnsFalse()
        {
            RunItemCompareTest("Fie", "My Field", false);
        }

        /// <summary>
        /// Tests the does not contain method when it doesn't match the substring returns true.
        /// </summary>
        [Test]
        public void TestDoesNotContainWhenDoesNotMatchReturnsFalse()
        {
            RunItemCompareTest("foo", "My Field", true);
        }

        /// <summary>
        /// Tests the does not contain method when the actual value is null returns false.
        /// </summary>
        [Test]
        public void TestDoesNotContainWhenActualValueIsNullReturnsTrue()
        {
            RunItemCompareTest("foo", null, true);
        }
    }
}