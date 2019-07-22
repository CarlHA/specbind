// <copyright file="StartsWithComparerFixture.cs">
//    Copyright © 2013 Dan Piessens  All rights reserved.
// </copyright>

namespace SpecBind.Tests.Validation
{
    using System.Linq;
    using NUnit.Framework;
    using SpecBind.Validation;
    
    /// <summary>
    /// A test fixture for the starts with string comparison.
    /// </summary>
    [TestFixture]
    public class StartsWithComparerFixture : ComparisonTestBase<StartsWithComparer>
    {
        /// <summary>
        /// Tests the rule key property for the correct tags.
        /// </summary>
        [Test]
        public void TestRuleValuesReturnsProperTags()
        {
            var item = new StartsWithComparer();

            CollectionAssert.AreEquivalent(new[] { "startswith" }, item.RuleKeys.ToList());
        }

        /// <summary>
        /// Tests the start with method when it matches the substring returns true.
        /// </summary>
        [Test]
        public void TestStartWithWhenMatchesReturnsTrue()
        {
            RunItemCompareTest("My", "My Field", true);
        }

        /// <summary>
        /// Tests the start with method when it doesn't match the substring returns false.
        /// </summary>
        [Test]
        public void TestStartWithWhenDoesNotMatchReturnsFalse()
        {
            RunItemCompareTest("foo", "My Field", false);
        }

        /// <summary>
        /// Tests the start with method when the actual value is null returns false.
        /// </summary>
        [Test]
        public void TestStartWithWhenActualValueIsNullReturnsFalse()
        {
            RunItemCompareTest("foo", null, false);
        }
    }
}