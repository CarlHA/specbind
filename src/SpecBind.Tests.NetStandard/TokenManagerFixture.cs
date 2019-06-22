// <copyright file="TokenManagerFixture.cs">
//    Copyright © 2013 Dan Piessens  All rights reserved.
// </copyright>

namespace SpecBind.Tests
{
	using System;

	using Moq;
	using NUnit.Framework;
	using SpecBind.Helpers;

	/// <summary>
	/// A test fixture for the TokenManager class.
	/// </summary>
	[TestFixture]
	public class TokenManagerFixture
	{
		/// <summary>
		/// Tests the GetToken method with a normal piece of data.
		/// </summary>
		[Test]
		public void TestGetTokenNormalData()
		{
			var context = new Mock<IScenarioContextHelper>(MockBehavior.Strict);

			var manager = new TokenManager(context.Object);

			var result = manager.GetToken("Test Data");

			Assert.AreEqual("Test Data", result);

			context.VerifyAll();
		}

	    /// <summary>
	    /// Tests the GetToken method with a normal piece of data.
	    /// </summary>
	    [Test]
	    public void TestGetTokenWithSendKeysFormat()
	    {
	        var context = new Mock<IScenarioContextHelper>(MockBehavior.Strict);

	        var manager = new TokenManager(context.Object);

	        var result = manager.GetToken("{{TAB}}");

	        Assert.AreEqual("{TAB}", result);

	        context.VerifyAll();
	    }

        /// <summary>
        /// Tests the GetTokenByKey method.
        /// </summary>
        [Test]
		public void TestGetTokenByKey()
		{
			var context = new Mock<IScenarioContextHelper>(MockBehavior.Strict);
			context.Setup(c => c.GetValue<string>("TOKEN:MYTOKEN")).Returns("Test Data");

			var manager = new TokenManager(context.Object);

			var result = manager.GetTokenByKey("mytoken");

			Assert.AreEqual("Test Data", result);

			context.VerifyAll();
		}

		/// <summary>
		/// Tests the GetToken method with a valid token.
		/// </summary>
		[Test]
		public void TestGetTokenIsTokenData()
		{
			var context = new Mock<IScenarioContextHelper>(MockBehavior.Strict);
			context.Setup(c => c.GetValue<string>("TOKEN:MYTOKEN")).Returns("Token Data");

			var manager = new TokenManager(context.Object);

			var result = manager.GetToken("{MyToken}");

			Assert.AreEqual("Token Data", result);

			context.VerifyAll();
		}

		/// <summary>
		/// Tests the GetToken method with a valid token that has extra whitespace.
		/// </summary>
		[Test]
		public void TestGetTokenIsMalformedTokenData()
		{
			var context = new Mock<IScenarioContextHelper>(MockBehavior.Strict);
			context.Setup(c => c.GetValue<string>("TOKEN:MYTOKEN")).Returns("Token Data");

			var manager = new TokenManager(context.Object);

			var result = manager.GetToken("{ MyToken }");

			Assert.AreEqual("Token Data", result);

			context.VerifyAll();
		}

		/// <summary>
		/// Tests the SetToken method with a key and value. No parsing is performed.
		/// </summary>
		[Test]
		public void TestSetTokenKeyAndValueNormalData()
		{
			var context = new Mock<IScenarioContextHelper>(MockBehavior.Strict);
			context.Setup(c => c.SetValue("Test Data", "TOKEN:MYTOKEN"));

			var manager = new TokenManager(context.Object);

			manager.SetToken("MyToken", "Test Data");

			context.VerifyAll();
		}

		/// <summary>
		/// Tests the SetToken method with a key and value. No parsing is performed.
		/// </summary>
		[Test]
		public void TestSetTokenKeyAndValueNullKey()
		{
			var context = new Mock<IScenarioContextHelper>(MockBehavior.Strict);
			var manager = new TokenManager(context.Object);

			Assert.Throws<ArgumentNullException>(() => 
			ExceptionHelper.SetupForException<ArgumentNullException>(
				() => manager.SetToken(null, "Test Data"),
				e =>
					{
						Assert.AreEqual("tokenName", e.ParamName);
						context.VerifyAll();
					}));
		}

		/// <summary>
		/// Tests the SetToken method with a normal piece of data.
		/// </summary>
		[Test]
		public void TestSetTokenNormalData()
		{
			var context = new Mock<IScenarioContextHelper>(MockBehavior.Strict);

			var manager = new TokenManager(context.Object);

			var result = manager.SetToken("Test Data");

			Assert.AreEqual("Test Data", result);

			context.VerifyAll();
		}

		/// <summary>
		/// Tests the SetToken method with a null of data.
		/// </summary>
		[Test]
		public void TestSetTokenNullData()
		{
			var context = new Mock<IScenarioContextHelper>(MockBehavior.Strict);

			var manager = new TokenManager(context.Object);

			var result = manager.SetToken(null);

			Assert.IsNull(result);

			context.VerifyAll();
		}

		/// <summary>
		/// Tests the SetToken method with a valid token.
		/// </summary>
		[Test]
		public void TestSetTokenIsTokenData()
		{
			var context = new Mock<IScenarioContextHelper>(MockBehavior.Strict);
			context.Setup(c => c.SetValue("Some Data", "TOKEN:MYTOKEN"));

			var manager = new TokenManager(context.Object);

			var result = manager.SetToken("{MyToken:Some Data}");

			Assert.AreEqual("Some Data", result);

			context.VerifyAll();
		}

		/// <summary>
		/// Tests the SetToken method with a token and no data which means the value should be retrieved.
		/// </summary>
		[Test]
		public void TestSetTokenIsEmptyTokenData()
		{
			var context = new Mock<IScenarioContextHelper>(MockBehavior.Strict);
			context.Setup(c => c.GetValue<string>("TOKEN:MYTOKEN")).Returns("My Data");

			var manager = new TokenManager(context.Object);

			var result = manager.SetToken("{MyToken}");

			Assert.AreEqual("My Data", result);

			context.VerifyAll();
		}

		/// <summary>
		/// Tests the SetToken method requesting a random integer value.
		/// </summary>
		[Test]
		public void TestSetTokenIsIntTokenData()
		{
			var context = new Mock<IScenarioContextHelper>(MockBehavior.Strict);
			context.Setup(c => c.SetValue(It.IsAny<string>(), "TOKEN:MYTOKEN"));

			var manager = new TokenManager(context.Object);

			var result = manager.SetToken("{MyToken:randomint}");

			Assert.IsNotNull(result);

			int value;
			Assert.IsTrue(int.TryParse(result, out value));

			context.VerifyAll();
		}

		/// <summary>
		/// Tests the SetToken method requesting a random GUID value.
		/// </summary>
		[Test]
		public void TestSetTokenIsGuidTokenData()
		{
			var context = new Mock<IScenarioContextHelper>(MockBehavior.Strict);
			context.Setup(c => c.SetValue(It.IsAny<string>(), "TOKEN:MYTOKEN"));

			var manager = new TokenManager(context.Object);

			var result = manager.SetToken("{MyToken:randomguid}");

			Assert.IsNotNull(result);

			Guid value;
			Assert.IsTrue(Guid.TryParse(result, out value));

			context.VerifyAll();
		}

		/// <summary>
		/// Tests the SetToken method requesting a random string value with no length.
		/// </summary>
		[Test]
		public void TestSetTokenIsStringTokenData()
		{
			var context = new Mock<IScenarioContextHelper>(MockBehavior.Strict);
			context.Setup(c => c.SetValue(It.IsAny<string>(), "TOKEN:MYTOKEN"));

			var manager = new TokenManager(context.Object);

			var result = manager.SetToken("{MyToken:randomstring}");

			Assert.IsNotNull(result);
			Assert.AreEqual(30, result.Length);

			context.VerifyAll();
		}

		/// <summary>
		/// Tests the SetToken method requesting a random string value with a length specified.
		/// </summary>
		[Test]
		public void TestSetTokenIsStringTokenDataCustomLength()
		{
			var context = new Mock<IScenarioContextHelper>(MockBehavior.Strict);
			context.Setup(c => c.SetValue(It.IsAny<string>(), "TOKEN:MYTOKEN"));

			var manager = new TokenManager(context.Object);

			var result = manager.SetToken("{MyToken:randomstring:10}");

			Assert.IsNotNull(result);
			Assert.AreEqual(10, result.Length);

			context.VerifyAll();
		}
	}
}
