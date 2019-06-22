// <copyright file="SetTokenFromValueActionFixture.cs">
//    Copyright © 2013 Dan Piessens  All rights reserved.
// </copyright>

namespace SpecBind.Tests.Actions
{
    using System;

    using Moq;
    using NUnit.Framework;
    using SpecBind.ActionPipeline;
    using SpecBind.Actions;
    using SpecBind.Helpers;
    using SpecBind.Pages;

    /// <summary>
    /// A test fixture for a button click action
    /// </summary>
    [TestFixture]
    public class SetTokenFromValueActionFixture
    {
        /// <summary>
        /// Tests getting the name of the action.
        /// </summary>
        [Test]
        public void TestGetActionName()
        {
            var getItemAction = new SetTokenFromValueAction(null);

            Assert.AreEqual("SetTokenFromValueAction", getItemAction.Name);
        }

        /// <summary>
        /// Tests the get item action with a field on the page that doesn't exist.
        /// </summary>
        [Test]
        public void TestExecuteWhenFieldDoesNotExistThenExceptionIsThrown()
        {
            var tokenManager = new Mock<ITokenManager>(MockBehavior.Strict);

            var locator = new Mock<IElementLocator>(MockBehavior.Strict);
            locator.Setup(p => p.GetElement("doesnotexist")).Throws(new ElementExecuteException("Cannot find item"));

            var getItemAction = new SetTokenFromValueAction(tokenManager.Object)
                                        {
                                            ElementLocator = locator.Object
                                        };

            var context = new SetTokenFromValueAction.TokenFieldContext("doesnotexist", "mytoken");
            Assert.Throws<ElementExecuteException>(() => 
            ExceptionHelper.SetupForException<ElementExecuteException>(
                () => getItemAction.Execute(context),
                e =>
                    {
                        locator.VerifyAll();
                        tokenManager.VerifyAll();
                    }));
        }

        /// <summary>
        /// Tests the get item action with a field on the page that doesn't exist.
        /// </summary>
        [Test]
        public void TestExecuteWhenContextTypeIsInvalidThenAnExceptionIsThrown()
        {
            var tokenManager = new Mock<ITokenManager>(MockBehavior.Strict);
            var locator = new Mock<IElementLocator>(MockBehavior.Strict);

            var getItemAction = new SetTokenFromValueAction(tokenManager.Object)
            {
                ElementLocator = locator.Object
            };

            var context = new ActionContext("doesnotexist");
            Assert.Throws<InvalidOperationException>(() => 
            ExceptionHelper.SetupForException<InvalidOperationException>(
                () => getItemAction.Execute(context),
                e =>
                {
                    StringAssert.Contains(e.Message, "TokenFieldContext");

                    locator.VerifyAll();
                    tokenManager.VerifyAll();
                }));
        }

        /// <summary>
        /// Tests the get item action with a property that exists.
        /// </summary>
        [Test]
        public void TestGetItemAndSetTokenSuccess()
        {
            var tokenManager = new Mock<ITokenManager>(MockBehavior.Strict);
            tokenManager.Setup(t => t.SetToken("mytoken", "Hello!"));

            var propData = new Mock<IPropertyData>(MockBehavior.Strict);
            propData.Setup(p => p.GetCurrentValue()).Returns("Hello!");

            var locator = new Mock<IElementLocator>(MockBehavior.Strict);
            locator.Setup(p => p.GetElement("doesnotexist")).Returns(propData.Object);

            var getItemAction = new SetTokenFromValueAction(tokenManager.Object)
                                        {
                                            ElementLocator = locator.Object
                                        };

            var context = new SetTokenFromValueAction.TokenFieldContext("doesnotexist", "mytoken");
            var result = getItemAction.Execute(context);

            Assert.AreEqual(true, result.Success);
            Assert.AreEqual("Hello!", result.Result);

            locator.VerifyAll();
            propData.VerifyAll();
            tokenManager.VerifyAll();
        }
    }
}