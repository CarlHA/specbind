﻿// <copyright file="DismissDialogActionFixture.cs">
//    Copyright © 2013 Dan Piessens  All rights reserved.
// </copyright>

namespace SpecBind.Tests.Actions
{
   using Moq;
   using NUnit.Framework;
   using SpecBind.Actions;
    using SpecBind.BrowserSupport;

    /// <summary>
    /// A test fixture for dismissing a dialog action.
    /// </summary>
    [TestFixture]
    public class DismissDialogActionFixture
    {
        /// <summary>
        /// Tests getting the name of the action.
        /// </summary>
        [Test]
        public void TestGetActionName()
        {
            var buttonClickAction = new DismissDialogAction(null);

            Assert.AreEqual("DismissDialogAction", buttonClickAction.Name);
        }


        /// <summary>
        /// Tests the dismiss alert when an invalid option is selected returns a failure.
        /// </summary>
        [Test]
        public void TestDismissAlertWhenInvalidOptionIsSelectedReturnsAFailure()
        {
            var browser = new Mock<IBrowser>(MockBehavior.Strict);

            var buttonClickAction = new DismissDialogAction(browser.Object);

            var context = new DismissDialogAction.DismissDialogContext("foo");
            var result = buttonClickAction.Execute(context);

            Assert.AreEqual(false, result.Success);
            Assert.IsNotNull(result.Exception);
            StringAssert.Contains("Could not translate 'foo' into a known dialog action. Available Actions:", result.Exception.Message);

            browser.VerifyAll();
        }

        /// <summary>
        /// dismiss alert when the OK button is selected.
        /// </summary>
        [Test]
        public void TestDismissAlertWhenOkButtonIsChoosenCallsBrowserAction()
        {
            var browser = new Mock<IBrowser>(MockBehavior.Strict);
            browser.Setup(p => p.DismissAlert(AlertBoxAction.Ok, null));

            var buttonClickAction = new DismissDialogAction(browser.Object);

            var context = new DismissDialogAction.DismissDialogContext("ok");
            var result = buttonClickAction.Execute(context);

            Assert.AreEqual(true, result.Success);

            browser.VerifyAll();
        }

        /// <summary>
        /// dismiss alert when the OK button is selected.
        /// </summary>
        [Test]
        public void TestDismissAlertWhenOkButtonWithWhitespaceIsChoosenCallsBrowserAction()
        {
            var browser = new Mock<IBrowser>(MockBehavior.Strict);
            browser.Setup(p => p.DismissAlert(AlertBoxAction.Ok, null));

            var buttonClickAction = new DismissDialogAction(browser.Object);

            var context = new DismissDialogAction.DismissDialogContext("  ok  ");
            var result = buttonClickAction.Execute(context);

            Assert.AreEqual(true, result.Success);

            browser.VerifyAll();
        }

        /// <summary>
        /// dismiss alert when the text is null, to make sure it's translated into an empty string and the OK button is selected.
        /// </summary>
        [Test]
        public void TestDismissAlertWhenTextIsEnteredButNullAndOkButtonIsChoosenCallsBrowserAction()
        {
            var browser = new Mock<IBrowser>(MockBehavior.Strict);
            browser.Setup(p => p.DismissAlert(AlertBoxAction.Ok, string.Empty));

            var buttonClickAction = new DismissDialogAction(browser.Object);

            var context = new DismissDialogAction.DismissDialogContext("ok", null);
            var result = buttonClickAction.Execute(context);

            Assert.AreEqual(true, result.Success);

            browser.VerifyAll();
        }

        /// <summary>
        /// dismiss alert when the text is null, to make sure it's translated into an empty string and the OK button is selected.
        /// </summary>
        [Test]
        public void TestDismissAlertWhenTextIsEnteredAndOkButtonIsChoosenCallsBrowserAction()
        {
            var browser = new Mock<IBrowser>(MockBehavior.Strict);
            browser.Setup(p => p.DismissAlert(AlertBoxAction.Ok, "Some Text"));

            var buttonClickAction = new DismissDialogAction(browser.Object);

            var context = new DismissDialogAction.DismissDialogContext("ok", "Some Text");
            var result = buttonClickAction.Execute(context);

            Assert.AreEqual(true, result.Success);

            browser.VerifyAll();
        }
    }
}