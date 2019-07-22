// <copyright file="LessThanComparerFixture.cs">
//    Copyright © 2013 Dan Piessens  All rights reserved.
// </copyright>

namespace SpecBind.Tests.Validation
{
    using System.Linq;
    using NUnit.Framework;
    using SpecBind.Validation;

    /// <summary>
    /// A test fixture for the less than comparison across supported data types.
    /// </summary>
    [TestFixture]
    public class LessThanComparerFixture : ComparisonTestBase<LessThanComparer>
    {
        /// <summary>
        /// Tests the rule key property for the correct tags.
        /// </summary>
        [Test]
        public void TestRuleValuesReturnsProperTags()
        {
            var item = new LessThanComparer();

            CollectionAssert.AreEquivalent(new[] { "lessthan" }, item.RuleKeys.ToList());
        }

        /// <summary>
        /// Tests the comparison with string members returns false because it's not supported.
        /// </summary>
        [Test]
        public void TestComparisonWithStringsReturnsFalse()
        {
            RunItemCompareTest("foo", "foo", false);
        }

        /// <summary>
        /// Tests the comparison with greater than integer members returns true.
        /// </summary>
        [Test]
        public void TestComparisonWithLessThanIntsReturnsTrue()
        {
            RunItemCompareTest("2", "1", true);
        }

        /// <summary>
        /// Tests the comparison with equal double members returns true.
        /// </summary>
        [Test]
        public void TestComparisonWithLessThanDoublesReturnsTrue()
        {
            RunItemCompareTest("2.5", "2.0", true);
        }

        /// <summary>
        /// Tests the comparison with boolean members returns false because it's not supported.
        /// </summary>
        [Test]
        public void TestComparisonWithBooleansThrowsNotSupportedException()
        {
            RunItemCompareTest("true", "false", false);
        }

        /// <summary>
        /// Tests the comparison with greater than date time members returns true.
        /// </summary>
        [Test]
        public void TestComparisonWithLessThanDateTimeReturnsTrue()
        {
            RunItemCompareTest("2/22/2013", "February 21, 2013", true);
        }

        /// <summary>
        /// Tests the ToString method for an expected response.
        /// </summary>
        [Test]
        public void TestToStringReturnsValidationInfo()
        {
            var comparer = new LessThanComparer();

            var result = comparer.ToString();

            Assert.AreEqual("Validation: lessthan", result);
        }
    }
}