// <copyright file="ButtonClickActionFixture.cs">
//    Copyright © 2013 Dan Piessens  All rights reserved.
// </copyright>

namespace SpecBind.Tests.Actions
{
	using Moq;
	using NUnit.Framework;
	using SpecBind.ActionPipeline;
	using SpecBind.Actions;
	using SpecBind.Pages;
	using ExceptionHelper = Tests.ExceptionHelper;

	/// <summary>
	/// A test fixture for a button click action
	/// </summary>
	[TestFixture()]
	public class ButtonClickActionFixture
	{
		/// <summary>
		/// Tests getting the name of the action.
		/// </summary>
		[Test]
		public void TestGetActionName()
		{
			var buttonClickAction = new ButtonClickAction();

            Assert.AreEqual("ButtonClickAction", buttonClickAction.Name);
		}

		/// <summary>
		///     Tests the fill field with a field on the page that doesn't exist.
		/// </summary>
		[Test]
		public void TestClickItemFieldDoesNotExist()
		{
			var locator = new Mock<IElementLocator>(MockBehavior.Strict);
			locator.Setup(p => p.GetElement("doesnotexist")).Throws(new ElementExecuteException("Cannot find item"));

			var buttonClickAction = new ButtonClickAction
			                        {
				                        ElementLocator = locator.Object
			                        };

		    var context = new ActionContext("doesnotexist");

		    Assert.Throws<ElementExecuteException>(() =>
			ExceptionHelper.SetupForException<ElementExecuteException>(
				() => buttonClickAction.Execute(context), e => locator.VerifyAll()));
		}

		/// <summary>
		///     Tests the fill field with an element that exists and can be clicked.
		/// </summary>
		[Test]
		public void TestClickItemSuccess()
		{
			var propData = new Mock<IPropertyData>(MockBehavior.Strict);
            propData.Setup(p => p.WaitForElementCondition(WaitConditions.NotMoving, null)).Returns(true);
            propData.Setup(p => p.WaitForElementCondition(WaitConditions.BecomesEnabled, null)).Returns(true);
			propData.Setup(p => p.ClickElement());

			var locator = new Mock<IElementLocator>(MockBehavior.Strict);
			locator.Setup(p => p.GetElement("myproperty")).Returns(propData.Object);

			var buttonClickAction = new ButtonClickAction
			{
				ElementLocator = locator.Object
			};

            var context = new ActionContext("myproperty");
            var result = buttonClickAction.Execute(context);

			Assert.AreEqual(true, result.Success);

			locator.VerifyAll();
			propData.VerifyAll();
		}

        /// <summary>
		///     Tests the fill field with an element that exists and can be clicked.
		/// </summary>
		[Test]
        public void TestClickItemWhenWaitIsEnabledReturnsSuccess()
        {
            var propData = new Mock<IPropertyData>(MockBehavior.Strict);
            propData.Setup(p => p.ClickElement());

            var locator = new Mock<IElementLocator>(MockBehavior.Strict);
            locator.Setup(p => p.GetElement("myproperty")).Returns(propData.Object);

            ButtonClickAction.WaitForStillElementBeforeClicking = false;
            
            var buttonClickAction = new ButtonClickAction
            {
                ElementLocator = locator.Object
                
            };

            var context = new ActionContext("myproperty");
            var result = buttonClickAction.Execute(context);

            Assert.AreEqual(true, result.Success);

            locator.VerifyAll();
            propData.VerifyAll();
        }
    }
}