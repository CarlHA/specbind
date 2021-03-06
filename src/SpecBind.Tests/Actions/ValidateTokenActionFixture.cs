﻿// <copyright file="ValidateTokenActionFixture.cs">
//    Copyright © 2014 Dan Piessens.  All rights reserved.
// </copyright>

namespace SpecBind.Tests.Actions
{
     using Moq;
     using NUnit.Framework;
     using SpecBind.Actions;
    using SpecBind.Helpers;
    using SpecBind.Tests.Validation;

    /// <summary>
    /// A test fixture for a validate token action
    /// </summary>
    [TestFixture]
    public class ValidateTokenActionFixture
    {
        /// <summary>
        /// Tests getting the name of the action.
        /// </summary>
        [Test]
        public void TestGetActionName()
        {
            var buttonClickAction = new ValidateTokenAction(null);

            Assert.AreEqual("ValidateTokenAction", buttonClickAction.Name);
        }

        /// <summary>
        /// Tests the execute method with a property that does not exist.
        /// </summary>
        [Test]
        public void TestExecuteWhenTokenReturnsNullReturnsFailure()
        {
            var tokenManager = new Mock<ITokenManager>(MockBehavior.Strict);
            tokenManager.Setup(p => p.GetTokenByKey("doesnotexist")).Returns((string)null);

            var validateItemAction = new ValidateTokenAction(tokenManager.Object);

            var context = new ValidateTokenAction.ValidateTokenActionContext("doesnotexist", "equals", "my value");
            context.ValidationTable.Process();

            var result = validateItemAction.Execute(context);

            Assert.AreEqual(false, result.Success);

            Assert.IsNotNull(result.Exception);
            StringAssert.Contains("doesnotexist", result.Exception.Message);

            tokenManager.VerifyAll();
        }

        /// <summary>
        /// Tests the execute method with a valid token and value returns success.
        /// </summary>
        [Test]
        public void TestExecuteWhenTokenReturnsValidValueReturnsSuccess()
        {
            var tokenManager = new Mock<ITokenManager>(MockBehavior.Strict);
            tokenManager.Setup(p => p.GetTokenByKey("token1")).Returns("my value");

            var validateItemAction = new ValidateTokenAction(tokenManager.Object);

            var context = new ValidateTokenAction.ValidateTokenActionContext("token1", "equals", "my value");
            context.ValidationTable.Process();

            var result = validateItemAction.Execute(context);

            Assert.AreEqual(true, result.Success);

            tokenManager.VerifyAll();
        }

    }
}