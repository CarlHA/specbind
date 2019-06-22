// <copyright file="PageBaseFixture.cs">
//    Copyright © 2013 Dan Piessens  All rights reserved.
// </copyright>

namespace SpecBind.Tests
{
	using NUnit.Framework;
	
	using System.Linq;

	using SpecBind.Pages;
	using SpecBind.Tests.Support;

	/// <summary>
	///     A test fixture for PageBase method.
	/// </summary>
	[TestFixture]
	public class PageBaseFixture
	{
		/// <summary>
		/// The that the metadata is discovered on inherited types.
		/// </summary>
		[Test]
		public void TestInheritedPageTypeDiscovery()
		{
			var target = new TestBase();

			IPropertyData data;
			var hasTopElement = target.TryGetElement("Button", out data);
			var hasProperty = target.TryGetProperty("Name", out data);

			var hasBaseClassProperty = target.TryGetProperty("HiddenProperty", out data);

			Assert.AreEqual(true, hasTopElement);
			Assert.AreEqual(true, hasProperty);
			Assert.AreEqual(false, hasBaseClassProperty);
		}

        /// <summary>
        /// The that the highlight method does nothing by default.
        /// </summary>
        [Test]
        public void TestHighlightDoesNothing()
        {
            var target = new TestBase();
            target.Highlight();
            target.Highlight(null);
        }

		/// <summary>
		/// The that the a non-existent element cannot be found.
		/// </summary>
		[Test]
		public void TestTryGetElementNotFound()
		{
			var target = new TestBase();

			IPropertyData data;
			var result = target.TryGetElement("NotFound", out data);

			Assert.AreEqual(false, result);
			Assert.IsNull(data);
		}

		/// <summary>
		/// The that the a property cannot be returned as an element.
		/// </summary>
		[Test]
		public void TestTryGetPropertyAsElementNotFound()
		{
			var target = new TestBase();

			IPropertyData data;
			var result = target.TryGetElement("Name", out data);

			Assert.AreEqual(false, result);
			Assert.IsNull(data);
		}

		/// <summary>
		/// The that a non-existent property cannot be found.
		/// </summary>
		[Test]
		public void TestTryGetPropertyNotFound()
		{
			var target = new TestBase();

			IPropertyData data;
			var result = target.TryGetProperty("NotFound", out data);

			Assert.AreEqual(false, result);
			Assert.IsNull(data);
		}

		/// <summary>
		/// The that a non-existent property cannot be found.
		/// </summary>
		[Test]
		public void TestGetPropertyNames()
		{
			var target = new TestBase();

			var result = target.GetPropertyNames(f => true).ToList();

			Assert.IsNotNull(result);
			CollectionAssert.Contains(result, "Name");
			CollectionAssert.Contains(result, "Button");
		}

		/// <summary>
		/// Ensures the property invoker for the class works properly.
		/// </summary>
		[Test]
		public void TestCallElementInvoker()
		{
			var page = new InheritedClass { Button = new BaseElement() };

			var target = new TestBase(page);

			IPropertyData property;
			var result = target.TryGetProperty("Button", out property);

			property.ClickElement();

			Assert.IsTrue(result);
		}

        /// <summary>
        /// Tests that a property value can be set with the property.
        /// </summary>
        [Test]
        public void TestSetPropertySetsPropertyValue()
        {
            var page = new InheritedClass();
            var target = new TestBase(page);

            IPropertyData data;
            var result = target.TryGetProperty("Name", out data);
            Assert.AreEqual(true, result);

            // Set the property value via the action
            data.FillData("Dan");

            Assert.AreEqual("Dan", page.Name);
        }

        /// <summary>
        /// The that wait for page checks for and invokes the interface.
        /// </summary>
        [Test]
        public void TestWaitForPageToBeActiveCallsNativeInterfaceNoActionIfMissing()
        {
            var targetClass = new InheritedClass();
            var target = new TestBase(targetClass);

            target.WaitForPageToBeActive();
        }

        /// <summary>
        /// The that wait for page checks for and invokes the interface.
        /// </summary>
        [Test]
        public void TestWaitForPageToBeActiveCallsNativeInterface()
        {
            var targetClass = new InheritedActivateClass();
            var target = new TestBase(targetClass);

            target.WaitForPageToBeActive();

            Assert.IsTrue(targetClass.ActiveCheck);
        }

        /// <summary>
        /// A test class.
        /// </summary>
	    private class InheritedActivateClass : InheritedClass, IActiveCheck
	    {
            /// <summary>
            /// Gets a value indicating whether the active check was called.
            /// </summary>
            /// <value><c>true</c> if active check; otherwise, <c>false</c>.</value>
            public bool ActiveCheck { get; private set; }

            /// <summary>
            /// Waits for the page to become active based on a property.
            /// </summary>
            public void WaitForActive()
            {
                this.ActiveCheck = true;
            }
	    }
	}
}