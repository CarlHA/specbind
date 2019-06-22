// <copyright file="ListPropertyDataFixture.cs">
//    Copyright © 2013 Dan Piessens.  All rights reserved.
// </copyright>

namespace SpecBind.Tests.PropertyHandlers
{
    using System;
    using System.Collections.Generic;

    using Moq;
    using NUnit.Framework;
    using SpecBind.Actions;
    using SpecBind.Pages;
    using SpecBind.PropertyHandlers;
    using SpecBind.Tests.Support;
    using SpecBind.Tests.Validation;
    using SpecBind.Validation;

    /// <summary>
    /// A test fixture for the list property data.
    /// </summary>
    [TestFixture]
    public class ListPropertyDataFixture
    {
        /// <summary>
        /// Tests that other methods in the class are not supported.
        /// </summary>
        [Test]
        public void TestsAreNotSupported()
        {
            var element = new BaseElement();
            var pageBase = new Mock<IPageElementHandler<BaseElement>>(MockBehavior.Strict);

            var propertyData = CreatePropertyData(pageBase, element);

            propertyData.TestForNotSupportedException(p => p.ClickElement(), "Clicking an element");
            propertyData.TestForNotSupportedException(p => p.CheckElementEnabled(), "Checking for an element being enabled");
            propertyData.TestForNotSupportedException(p => p.CheckElementExists(), "Checking for an element existing");
            propertyData.TestForNotSupportedException(p => p.FillData(null), "Filling in data");
            propertyData.TestForNotSupportedException(p => p.GetCurrentValue(), "Getting the current value");
            propertyData.TestForNotSupportedException(p => p.GetItemAsPage(), "Getting a property as a page item");
            propertyData.TestForNotSupportedException(p => p.Highlight(), "Highlighting an item");
            propertyData.TestForNotSupportedException(p => p.WaitForElementCondition(WaitConditions.Exists, TimeSpan.Zero), "Waiting for an element");

            pageBase.VerifyAll();
        }

        /// <summary>
        /// Tests the ValidateItem method for a list parent.
        /// </summary>
        [Test]
        public void TestValidateItemAsList()
        {
            var element = new BaseElement();
            var parentElement = new BaseElement();

            var listMock = new Mock<IElementList<BaseElement, BaseElement>>(MockBehavior.Strict);
            listMock.SetupGet(l => l.Parent).Returns(parentElement);

            var pageBase = new Mock<IPageElementHandler<BaseElement>>(MockBehavior.Strict);
            pageBase.Setup(p => p.GetElementText(parentElement)).Returns("My Data");

            var propertyData = CreatePropertyData(pageBase, element, (p, f) => f(listMock.Object));

            string actualValue;
            var result = propertyData.ValidateItem(ItemValidationHelper.Create("MyProperty", "My Data"), out actualValue);

            Assert.IsTrue(result);

            pageBase.VerifyAll();
            listMock.VerifyAll();
        }

        /// <summary>
        /// Tests that GetCurrentValue throws an exception if getting a value from the property.
        /// </summary>
        [Test]
         public void TestGetCurrentValueFromListProperty()
        {
            var element = new BaseElement();
            var propData = new Mock<IPropertyData>();
            var page = new Mock<IPage>(MockBehavior.Strict);

            var pageBase = new Mock<IPageElementHandler<BaseElement>>(MockBehavior.Strict);

            var propertyData = CreatePropertyData(pageBase, element);

            Assert.Throws<NotSupportedException>(() => 
            ExceptionHelper.SetupForException<NotSupportedException>(
                () => propertyData.GetCurrentValue(),
                v =>
                {
                    pageBase.VerifyAll();
                    page.VerifyAll();
                    propData.VerifyAll();
                }));
        }

        /// <summary>
        /// Tests the ValidateList method.
        /// </summary>
        [Test]
        public void TestValidateListContains()
        {
            var element = new BaseElement();
            var listElement = new BaseElement();
            var validation = ItemValidationHelper.Create("MyProperty", "My Data");
            var validations = new List<ItemValidation> { validation };

            var propData = new Mock<IPropertyData>();
            string actualValue;
            propData.Setup(p => p.ValidateItem(validation, out actualValue)).Returns(true);

            var page = new Mock<IPage>(MockBehavior.Strict);

            // ReSharper disable once RedundantAssignment
            var property = propData.Object;
            page.Setup(p => p.TryGetProperty("MyProperty", out property)).Returns(true);

            var pageBase = new Mock<IPageElementHandler<BaseElement>>(MockBehavior.Strict);
            pageBase.Setup(p => p.GetPageFromElement(listElement)).Returns(page.Object);

            var propertyData = CreatePropertyData(pageBase, element, (p, f) => f(new List<BaseElement> { listElement }));

            var result = propertyData.ValidateList(ComparisonType.Contains, validations);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(1, result.ItemCount);

            pageBase.VerifyAll();
            page.VerifyAll();
            propData.VerifyAll();
        }

        /// <summary>
        /// Tests the ValidateList method.
        /// </summary>
        [Test]
        public void TestValidateListStartsWith()
        {
            var element = new BaseElement();
            var listElement = new BaseElement();
            var validation = ItemValidationHelper.Create("MyProperty", "My Data");
            var validations = new List<ItemValidation> { validation };

            var propData = new Mock<IPropertyData>();
            string actualValue;
            propData.Setup(p => p.ValidateItem(validation, out actualValue)).Returns(true);

            var page = new Mock<IPage>(MockBehavior.Strict);

            // ReSharper disable once RedundantAssignment
            var property = propData.Object;
            page.Setup(p => p.TryGetProperty("MyProperty", out property)).Returns(true);

            var pageBase = new Mock<IPageElementHandler<BaseElement>>(MockBehavior.Strict);
            pageBase.Setup(p => p.GetPageFromElement(listElement)).Returns(page.Object);

            var propertyData = CreatePropertyData(pageBase, element, (p, f) => f(new List<BaseElement> { listElement }));

            var result = propertyData.ValidateList(ComparisonType.StartsWith, validations);

            Assert.IsTrue(result.IsValid);

            pageBase.VerifyAll();
            page.VerifyAll();
            propData.VerifyAll();
        }

        /// <summary>
        /// Tests the ValidateList method using the equals operator.
        /// </summary>
        [Test]
        public void TestValidateListEquals()
        {
            var element = new BaseElement();
            var listElement = new BaseElement();
            var validation = ItemValidationHelper.Create("MyProperty", "My Data");
            var validations = new List<ItemValidation> { validation };

            var propData = new Mock<IPropertyData>();
            string actualValue;
            propData.Setup(p => p.ValidateItem(validation, out actualValue)).Returns(true);

            var page = new Mock<IPage>(MockBehavior.Strict);

            // ReSharper disable once RedundantAssignment
            var property = propData.Object;
            page.Setup(p => p.TryGetProperty("MyProperty", out property)).Returns(true);

            var pageBase = new Mock<IPageElementHandler<BaseElement>>(MockBehavior.Strict);
            pageBase.Setup(p => p.GetPageFromElement(listElement)).Returns(page.Object);

            var propertyData = CreatePropertyData(pageBase, element, (p, f) => f(new List<BaseElement> { listElement }));

            var result = propertyData.ValidateList(ComparisonType.Equals, validations);

            Assert.IsTrue(result.IsValid);

            pageBase.VerifyAll();
            page.VerifyAll();
            propData.VerifyAll();
        }

        /// <summary>
        /// Tests the ValidateList method.
        /// </summary>
        [Test]
        public void TestValidateListEndsWith()
        {
            var element = new BaseElement();
            var listElement = new BaseElement();
            var validation = ItemValidationHelper.Create("MyProperty", "My Data");
            var validations = new List<ItemValidation> { validation };

            var propData = new Mock<IPropertyData>();
            string actualValue;
            propData.Setup(p => p.ValidateItem(validation, out actualValue)).Returns(true);

            var page = new Mock<IPage>(MockBehavior.Strict);

            // ReSharper disable once RedundantAssignment
            var property = propData.Object;
            page.Setup(p => p.TryGetProperty("MyProperty", out property)).Returns(true);

            var pageBase = new Mock<IPageElementHandler<BaseElement>>(MockBehavior.Strict);
            pageBase.Setup(p => p.GetPageFromElement(listElement)).Returns(page.Object);

            var propertyData = CreatePropertyData(pageBase, element, (p, f) => f(new List<BaseElement> { listElement }));

            var result = propertyData.ValidateList(ComparisonType.EndsWith, validations);

            Assert.IsTrue(result.IsValid);

            pageBase.VerifyAll();
            page.VerifyAll();
            propData.VerifyAll();
        }

        /// <summary>
        /// Tests the ValidateList method.
        /// </summary>
        [Test]
        public void TestValidateListNotContains()
        {
            var element = new BaseElement();
            var listElement = new BaseElement();
            var validation = ItemValidationHelper.Create("MyProperty", "My Data");
            var validations = new List<ItemValidation> { validation };

            var propData = new Mock<IPropertyData>();
            string actualValue;
            propData.Setup(p => p.ValidateItem(validation, out actualValue)).Returns(false);

            var page = new Mock<IPage>(MockBehavior.Strict);

            // ReSharper disable once RedundantAssignment
            var property = propData.Object;
            page.Setup(p => p.TryGetProperty("MyProperty", out property)).Returns(true);

            var pageBase = new Mock<IPageElementHandler<BaseElement>>(MockBehavior.Strict);
            pageBase.Setup(p => p.GetPageFromElement(listElement)).Returns(page.Object);

            var propertyData = CreatePropertyData(pageBase, element, (p, f) => f(new List<BaseElement> { listElement }));

            var result = propertyData.ValidateList(ComparisonType.DoesNotContain, validations);

            Assert.IsTrue(result.IsValid);

            pageBase.VerifyAll();
            page.VerifyAll();
            propData.VerifyAll();
        }

        /// <summary>
        /// Tests the ValidateList method with NotEquals comparison.
        /// </summary>
        [Test]
        public void TestValidateListNotEquals()
        {
            var element = new BaseElement();
            var listElement = new BaseElement();
            var validation = ItemValidationHelper.Create("MyProperty", "My Data");
            var validations = new List<ItemValidation> { validation };

            var propData = new Mock<IPropertyData>();
            string actualValue;
            propData.Setup(p => p.ValidateItem(validation, out actualValue)).Returns(false);

            var page = new Mock<IPage>(MockBehavior.Strict);

            // ReSharper disable once RedundantAssignment
            var property = propData.Object;
            page.Setup(p => p.TryGetProperty("MyProperty", out property)).Returns(true);

            var pageBase = new Mock<IPageElementHandler<BaseElement>>(MockBehavior.Strict);
            pageBase.Setup(p => p.GetPageFromElement(listElement)).Returns(page.Object);

            var propertyData = CreatePropertyData(pageBase, element, (p, f) => f(new List<BaseElement> { listElement }));

            var result = propertyData.ValidateList(ComparisonType.DoesNotEqual, validations);

            Assert.IsTrue(result.IsValid);

            pageBase.VerifyAll();
            page.VerifyAll();
            propData.VerifyAll();
        }

        /// <summary>
        /// Tests the ValidateList method.
        /// </summary>
        [Test]
        public void TestValidateListInvalidComparison()
        {
            var element = new BaseElement();
            var listElement = new BaseElement();
            var validation = ItemValidationHelper.Create("MyProperty", "My Data");
            var validations = new List<ItemValidation> { validation };

            var pageBase = new Mock<IPageElementHandler<BaseElement>>(MockBehavior.Strict);
            var propertyData = CreatePropertyData(pageBase, element, (p, f) => f(new List<BaseElement> { listElement }));

            var result = propertyData.ValidateList(ComparisonType.Enabled, validations);

            Assert.IsFalse(result.IsValid);

            pageBase.VerifyAll();
        }

        /// <summary>
        /// Tests the ValidateList method.
        /// </summary>
        [Test]
        public void TestValidateListContainsChildElementNotFound()
        {
            var element = new BaseElement();
            var listElement = new BaseElement();
            var validation = ItemValidationHelper.Create("MyProperty", "My Data");
            var validations = new List<ItemValidation> { validation };

            var propData = new Mock<IPropertyData>();
            var page = new Mock<IPage>(MockBehavior.Strict);

            // ReSharper disable once RedundantAssignment
            var property = propData.Object;
            page.Setup(p => p.TryGetProperty("MyProperty", out property)).Returns(false);

            var pageBase = new Mock<IPageElementHandler<BaseElement>>(MockBehavior.Strict);
            pageBase.Setup(p => p.GetPageFromElement(listElement)).Returns(page.Object);

            var propertyData = CreatePropertyData(pageBase, element, (p, f) => f(new List<BaseElement> { listElement }));

            var result = propertyData.ValidateList(ComparisonType.Contains, validations);

            Assert.IsFalse(result.IsValid);

            pageBase.VerifyAll();
            page.VerifyAll();
            propData.VerifyAll();
        }

        /// <summary>
        /// Tests the ValidateList method.
        /// </summary>
        [Test]
        public void TestValidateListContainsValidationFails()
        {
            var element = new BaseElement();
            var listElement = new BaseElement();
            var validation = ItemValidationHelper.Create("MyProperty", "My Data");
            var validations = new List<ItemValidation> { validation };

            var propData = new Mock<IPropertyData>();
            string actualValue;
            propData.Setup(p => p.ValidateItem(validation, out actualValue)).Returns(false);

            var page = new Mock<IPage>(MockBehavior.Strict);

            // ReSharper disable once RedundantAssignment
            var property = propData.Object;
            page.Setup(p => p.TryGetProperty("MyProperty", out property)).Returns(true);

            var pageBase = new Mock<IPageElementHandler<BaseElement>>(MockBehavior.Strict);
            pageBase.Setup(p => p.GetPageFromElement(listElement)).Returns(page.Object);

            var propertyData = CreatePropertyData(pageBase, element, (p, f) => f(new List<BaseElement> { listElement }));

            var result = propertyData.ValidateList(ComparisonType.Contains, validations);

            Assert.IsFalse(result.IsValid);

            pageBase.VerifyAll();
            page.VerifyAll();
            propData.VerifyAll();
        }

        /// <summary>
        /// Tests the GetItemAtIndex method.
        /// </summary>
        [Test]
        public void TestGetItemAtIndexChildElementNotFound()
        {
            var element = new BaseElement();
            var propData = new Mock<IPropertyData>();
            var page = new Mock<IPage>(MockBehavior.Strict);


            var pageBase = new Mock<IPageElementHandler<BaseElement>>(MockBehavior.Strict);

            var propertyData = CreatePropertyData(pageBase, element, (p, f) => f(new List<BaseElement>()));

            var result = propertyData.GetItemAtIndex(0);

            Assert.IsNull(result);

            pageBase.VerifyAll();
            page.VerifyAll();
            propData.VerifyAll();
        }

        /// <summary>
        /// Tests the GetItemAtIndex method.
        /// </summary>
        [Test]
        public void TestGetItemAtIndexSuccess()
        {
            var element = new BaseElement();
            var listElement = new BaseElement();
            var propData = new Mock<IPropertyData>();
            var page = new Mock<IPage>(MockBehavior.Strict);
            var listPage = new Mock<IPage>(MockBehavior.Strict);

            var pageBase = new Mock<IPageElementHandler<BaseElement>>(MockBehavior.Strict);
            pageBase.Setup(p => p.GetPageFromElement(listElement)).Returns(listPage.Object);

            var propertyData = CreatePropertyData(pageBase, element, (p, f) => f(new List<BaseElement> { listElement }));

            var result = propertyData.GetItemAtIndex(0);

            Assert.AreSame(listPage.Object, result);

            pageBase.VerifyAll();
            page.VerifyAll();
            propData.VerifyAll();
        }

        /// <summary>
        /// Tests the ValidateListRowCount with an equality check that is successful.
        /// </summary>
        [Test]
        public void TestValidateListRowCountEqualsSuccess()
        {
            var element = new BaseElement();
            var listElement = new BaseElement();
            var propData = new Mock<IPropertyData>();
            var page = new Mock<IPage>(MockBehavior.Strict);

            var pageBase = new Mock<IPageElementHandler<BaseElement>>(MockBehavior.Strict);

            var propertyData = CreatePropertyData(pageBase, element, (p, f) => f(new List<BaseElement> { listElement }));

            var result = propertyData.ValidateListRowCount(NumericComparisonType.Equals, 1);

            Assert.AreEqual(true, result.Item1);
            Assert.AreEqual(1, result.Item2);

            pageBase.VerifyAll();
            page.VerifyAll();
            propData.VerifyAll();
        }

        /// <summary>
        /// Tests the ValidateListRowCount with an equality check that is successful.
        /// </summary>
        [Test]
        public void TestValidateListRowCountGreaterThanEqualsSuccess()
        {
            var element = new BaseElement();
            var listElement = new BaseElement();
            var propData = new Mock<IPropertyData>();
            var page = new Mock<IPage>(MockBehavior.Strict);

            var pageBase = new Mock<IPageElementHandler<BaseElement>>(MockBehavior.Strict);

            var propertyData = CreatePropertyData(pageBase, element, (p, f) => f(new List<BaseElement> { listElement }));

            var result = propertyData.ValidateListRowCount(NumericComparisonType.GreaterThanEquals, 1);

            Assert.AreEqual(true, result.Item1);
            Assert.AreEqual(1, result.Item2);

            pageBase.VerifyAll();
            page.VerifyAll();
            propData.VerifyAll();
        }

        /// <summary>
        /// Tests the ValidateListRowCount with an equality check that is successful.
        /// </summary>
        [Test]
        public void TestValidateListRowCountLessThanEqualsSuccess()
        {
            var element = new BaseElement();
            var propData = new Mock<IPropertyData>();
            var page = new Mock<IPage>(MockBehavior.Strict);

            var pageBase = new Mock<IPageElementHandler<BaseElement>>(MockBehavior.Strict);

            var propertyData = CreatePropertyData(pageBase, element, (p, f) => f(new List<BaseElement>()));

            var result = propertyData.ValidateListRowCount(NumericComparisonType.LessThanEquals, 1);

            Assert.AreEqual(true, result.Item1);
            Assert.AreEqual(0, result.Item2);

            pageBase.VerifyAll();
            page.VerifyAll();
            propData.VerifyAll();
        }

        /// <summary>
        /// Creates the property data.
        /// </summary>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="mock">The mock.</param>
        /// <param name="element">The element.</param>
        /// <param name="action">The action.</param>
        /// <returns>The property data item.</returns>
        private static ListPropertyData<TElement> CreatePropertyData<TElement>(Mock<IPageElementHandler<TElement>> mock, TElement element, Func<IPage, Func<object, bool>, bool> action = null)
        {
            if (action == null)
            {
                action = (p, f) =>
                    {
                        Assert.AreSame(p, mock.Object);
                        return f(element);
                    };
            }

            return new ListPropertyData<TElement>(mock.Object, "MyProperty", typeof(TElement), action);
        }
    }
}
