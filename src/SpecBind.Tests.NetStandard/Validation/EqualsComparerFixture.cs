// <copyright file="EqualsComparerFixture.cs">
//    Copyright © 2013 Dan Piessens  All rights reserved.
// </copyright>

namespace SpecBind.Tests.Validation
{
    using System.Linq;
    using NUnit.Framework;
    using SpecBind.Validation;

    /// <summary>
    /// A test fixture for the equal comparison across supported data types.
    /// </summary>
    [TestFixture]
    public class EqualsComparerFixture : ComparisonTestBase<EqualsComparer>
    {
        /// <summary>
        /// Tests the rule key property for the correct tags.
        /// </summary>
        [Test]
        public void TestRuleValuesReturnsProperTags()
        {
            var item = new EqualsComparer();

            CollectionAssert.AreEquivalent(new[] { "equals" }, item.RuleKeys.ToList());
        }

        /// <summary>
        /// Tests the comparison with equal string members returns true.
        /// </summary>
        [Test]
        public void TestComparisonWithEqualStringsReturnsTrue()
        {
            RunItemCompareTest("foo", "foo", true);
        }

        /// <summary>
        /// Tests the comparison with equal different case string members returns true.
        /// </summary>
        [Test]
        public void TestComparisonWithEqualDifferentCaseStringsReturnsTrue()
        {
            RunItemCompareTest("foo", "Foo", true);
        }

        /// <summary>
        /// Tests the comparison with equal integer members returns true.
        /// </summary>
        [Test]
        public void TestComparisonWithEqualIntsReturnsTrue()
        {
            RunItemCompareTest("1", "1", true);
        }

        /// <summary>
        /// Tests the comparison with equal double members returns true.
        /// </summary>
        [Test]
        public void TestComparisonWithEqualDoublesReturnsTrue()
        {
            RunItemCompareTest("2.0", "2", true);
        }

        /// <summary>
        /// Tests the comparison with equal boolean members returns true.
        /// </summary>
        [Test]
        public void TestComparisonWithEqualBooleansReturnsTrue()
        {
            RunItemCompareTest("true", "True", true);
        }

        /// <summary>
        /// Tests the comparison with equal date time members returns true.
        /// </summary>
        [Test]
        public void TestComparisonWithEqualDateTimeReturnsTrue()
        {
            RunItemCompareTest("2/22/2013", "February 22, 2013", true);
        }
    }
}