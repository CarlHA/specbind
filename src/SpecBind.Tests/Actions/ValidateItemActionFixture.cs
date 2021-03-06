﻿// <copyright file="ValidateItemActionFixture.cs">
//    Copyright © 2013 Dan Piessens  All rights reserved.
// </copyright>

namespace SpecBind.Tests.Actions
{
	using System.Linq;

	using Moq;
	using NUnit.Framework;
	using SpecBind.ActionPipeline;
	using SpecBind.Actions;
	using SpecBind.Pages;
	using SpecBind.Tests.Validation;
	using SpecBind.Validation;

	/// <summary>
	/// A test fixture for a validate item action
	/// </summary>
	[TestFixture]
    public class ValidateItemActionFixture
    {
        /// <summary>
        /// Tests getting the name of the action.
        /// </summary>
        [Test]
        public void TestGetActionName()
        {
            var buttonClickAction = new ValidateItemAction();

            Assert.AreEqual("ValidateItemAction", buttonClickAction.Name);
        }

        /// <summary>
        /// Tests the execute method with a property that does not exist.
        /// </summary>
        [Test]
        public void TestExecuteWhenFieldDoesNotExistReturnsFailure()
        {
            IPropertyData propertyData;
            var locator = new Mock<IElementLocator>(MockBehavior.Strict);
            locator.Setup(p => p.TryGetProperty("doesnotexist", out propertyData)).Returns(false);

            var validateItemAction = new ValidateItemAction
                                        {
                                            ElementLocator = locator.Object
                                        };

            var table = new ValidationTable();
            table.AddValidation("doesnotexist", "My Data", "equals");
            table.Process();

            var context = new ValidateItemAction.ValidateItemContext(table);

            var result = validateItemAction.Execute(context);

            Assert.AreEqual(false, result.Success);

            Assert.IsNotNull(result.Exception);
            StringAssert.Contains("doesnotexist", result.Exception.Message);

            locator.VerifyAll();
        }

        /// <summary>
        /// Tests the execute method with a property that does not exist on the model fails.
        /// </summary>
        [Test]
        public void TestExecuteWhenFieldDoesNotExistAndIsDoesNotExistComparerReturnsFailure()
        {
            IPropertyData propertyData;
            var locator = new Mock<IElementLocator>(MockBehavior.Strict);
            locator.Setup(p => p.TryGetProperty("pdoesnotexist", out propertyData)).Returns(false);

            var validateItemAction = new ValidateItemAction
            {
                ElementLocator = locator.Object
            };

            var table = new ValidationTable();
            table.AddValidation("pdoesnotexist", "My Data", "doesnotexist");
            table.Process(new DoesNotExistComparer());

            var context = new ValidateItemAction.ValidateItemContext(table);

            var result = validateItemAction.Execute(context);

            Assert.AreEqual(false, result.Success);

            Assert.IsNotNull(result.Exception);
            StringAssert.Contains("[Not Found]", result.Exception.Message);

            locator.VerifyAll();
        }

        /// <summary>
        /// Tests the execute method with a property that exists and is valid returns a successful result.
        /// </summary>
        [Test]
        public void TestExecuteWhenFieldExistsAndIsValidReturnsSuccess()
        {
            var table = new ValidationTable();
            table.AddValidation("name", "My Data", "equals");
            table.Process();

            var propData = new Mock<IPropertyData>(MockBehavior.Strict);
            string actualValue;
            propData.Setup(p => p.ValidateItem(table.Validations.First(), out actualValue)).Returns(true);

            // ReSharper disable once RedundantAssignment
            var propertyData = propData.Object;
            var locator = new Mock<IElementLocator>(MockBehavior.Strict);
            locator.Setup(p => p.TryGetProperty("name", out propertyData)).Returns(true);

            var validateItemAction = new ValidateItemAction
            {
                ElementLocator = locator.Object
            };

            var context = new ValidateItemAction.ValidateItemContext(table);

            var result = validateItemAction.Execute(context);

            Assert.AreEqual(true, result.Success);

            locator.VerifyAll();
        }

        /// <summary>
        /// Tests the execute method with a property that exists and is valid returns a failure result.
        /// </summary>
        [Test]
        public void TestExecuteWhenFieldExistsButIsNotValidReturnsFailure()
        {
            var table = new ValidationTable();
            table.AddValidation("name", "equals", "wrong");
            table.Process();

            var propData = new Mock<IPropertyData>(MockBehavior.Strict);
            string actualValue;
            propData.Setup(p => p.ValidateItem(table.Validations.First(), out actualValue)).Returns(false);

            // ReSharper disable once RedundantAssignment
            var propertyData = propData.Object;
            var locator = new Mock<IElementLocator>(MockBehavior.Strict);
            locator.Setup(p => p.TryGetProperty("name", out propertyData)).Returns(true);

            var validateItemAction = new ValidateItemAction
            {
                ElementLocator = locator.Object
            };

            var context = new ValidateItemAction.ValidateItemContext(table);

            var result = validateItemAction.Execute(context);

            Assert.AreEqual(false, result.Success);
            Assert.IsNotNull(result.Exception);
            StringAssert.Contains("wrong", result.Exception.Message);

            locator.VerifyAll();
        }


		/// <summary>
		/// Tests the RetryValidationUntilTimeout configuration attribute,
		/// with initial failure but subsequent success returned before the timeout.
		/// </summary>
		[Test]
		public void TestRetryValidationUntilTimeoutWithEventualSuccessBeforeTimeout()
		{
			try
			{
				ValidateItemAction.RetryValidationUntilTimeout = true;
				ActionBase.DefaultTimeout = System.TimeSpan.FromSeconds(5); // NOTE: interval between checks is 200ms

				var table = new ValidationTable();
				table.AddValidation("name", "My Data", "equals");
				table.Process();

				var propData = new Mock<IPropertyData>(MockBehavior.Strict);
				string actualValue;
				propData.SetupSequence(p => p.ValidateItem(table.Validations.First(), out actualValue))
					.Returns(false)
					.Returns(true); // after 200ms -- just in time

				var propertyData = propData.Object;
				var locator = new Mock<IElementLocator>(MockBehavior.Strict);
				locator.Setup(p => p.TryGetProperty("name", out propertyData)).Returns(true);

				var validateItemAction = new ValidateItemAction
				{
					ElementLocator = locator.Object
				};

				var context = new ValidateItemAction.ValidateItemContext(table);

				var result = validateItemAction.Execute(context);

				Assert.IsTrue(result.Success);

				locator.VerifyAll();
			}
			finally
			{
				ValidateItemAction.RetryValidationUntilTimeout = false;
				ActionBase.DefaultTimeout = System.TimeSpan.FromSeconds(5);
			}
		}

		/// <summary>
		/// Tests the RetryValidationUntilTimeout configuration attribute,
		/// with initial failure but subsequent success returned before the timeout.
		/// </summary>
		[Test]
		public void TestRetryValidationUntilTimeoutWithNoSuccessBeforeTimeout()
		{
			try
			{
				ValidateItemAction.RetryValidationUntilTimeout = true;
				ActionBase.DefaultTimeout = System.TimeSpan.FromMilliseconds(300); // NOTE: interval between checks is 200ms

				var table = new ValidationTable();
				table.AddValidation("name", "My Data", "equals");
				table.Process();

				var propData = new Mock<IPropertyData>(MockBehavior.Strict);
				string actualValue;
				propData.SetupSequence(p => p.ValidateItem(table.Validations.First(), out actualValue))
					.Returns(false)
					.Returns(false) // after 200ms
					.Returns(true); // after 400ms -- too late

				var propertyData = propData.Object;
				var locator = new Mock<IElementLocator>(MockBehavior.Strict);
				locator.Setup(p => p.TryGetProperty("name", out propertyData)).Returns(true);

				var validateItemAction = new ValidateItemAction
				{
					ElementLocator = locator.Object
				};

				var context = new ValidateItemAction.ValidateItemContext(table);

				var result = validateItemAction.Execute(context);

				Assert.IsFalse(result.Success);

				locator.VerifyAll();
			}
			finally
			{
				ValidateItemAction.RetryValidationUntilTimeout = false;
				ActionBase.DefaultTimeout = System.TimeSpan.FromSeconds(5);
			}
		}
	}
}