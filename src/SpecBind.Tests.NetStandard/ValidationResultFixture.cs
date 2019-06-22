// <copyright file="ValidationResultFixture.cs">
//    Copyright © 2013 Dan Piessens  All rights reserved.
// </copyright>

namespace SpecBind.Tests
{
	using System;
	using System.Text;
	using NUnit.Framework;
	using SpecBind.Pages;
	using SpecBind.Tests.Validation;

    /// <summary>
	/// A test fixture for the ValidationResult class.
	/// </summary>
	[TestFixture]
	public class ValidationResultFixture
	{
		/// <summary>
		/// Tests the get comparison table by rule when multiple results throws exception.
		/// </summary>
		[Test]
		public void TestGetComparisonTableByRuleWhenMultipleResultsThrowsException()
		{
			var validations = new[] { ItemValidationHelper.Create("MyField", "Something") };

			var validationResult = new ValidationResult(validations);
			validationResult.CheckedItems.Add(new ValidationItemResult());
			validationResult.CheckedItems.Add(new ValidationItemResult());

			Assert.Throws<InvalidOperationException>(() => validationResult.GetComparisonTableByRule());
		}

		/// <summary>
		/// Tests the get comparison table by rule with valid fields.
		/// </summary>
		[Test]
		public void TestGetComparisonTableByRuleWithValidFields()
		{
            var validation = ItemValidationHelper.Create("MyField", "Something");
			var validationResult = new ValidationResult(new[] { validation });

			var itemResult = new ValidationItemResult();
			itemResult.NoteValidationResult(validation, true, null);

			validationResult.CheckedItems.Add(itemResult);

			var result = validationResult.GetComparisonTableByRule();

			var expectedTable = new StringBuilder()
										.AppendLine("| Field   | Rule   | Value     |")
											.Append("| MyField | equals | Something |");

			Assert.AreEqual(expectedTable.ToString(), result);
		}

		/// <summary>
		/// Tests the get comparison table by rule with invalid field value.
		/// </summary>
		[Test]
		public void TestGetComparisonTableByRuleWithInvalidFieldValue()
		{
            var validation = ItemValidationHelper.Create("MyField", "Something");
			var validationResult = new ValidationResult(new[] { validation });

			var itemResult = new ValidationItemResult();
			itemResult.NoteValidationResult(validation, false, "Nothing");

			validationResult.CheckedItems.Add(itemResult);

			var result = validationResult.GetComparisonTableByRule();

			var expectedTable = new StringBuilder()
										.AppendLine("| Field   | Rule   | Value               |")
											.Append("| MyField | equals | Something [Nothing] |");

			Assert.AreEqual(expectedTable.ToString(), result);
		}

		/// <summary>
		/// Tests the get comparison table by rule with missing field.
		/// </summary>
		[Test]
		public void TestGetComparisonTableByRuleWithMissingField()
		{
            var validation = ItemValidationHelper.Create("MyField", "Something");
			var validationResult = new ValidationResult(new[] { validation });

			var itemResult = new ValidationItemResult();
			itemResult.NoteMissingProperty(validation);

			validationResult.CheckedItems.Add(itemResult);

			var result = validationResult.GetComparisonTableByRule();

			var expectedTable = new StringBuilder()
										.AppendLine("| Field               | Rule   | Value     |")
											.Append("| MyField [Not Found] | equals | Something |");

			Assert.AreEqual(expectedTable.ToString(), result);
		}

		/// <summary>
		/// Tests the get comparison table with valid fields.
		/// </summary>
		[Test]
		public void TestGetComparisonTableWithValidFields()
		{
            var validation = ItemValidationHelper.Create("MyField", "Something");
			var validationResult = new ValidationResult(new[] { validation });

			var itemResult = new ValidationItemResult();
			itemResult.NoteValidationResult(validation, true, "Something");

			validationResult.CheckedItems.Add(itemResult);

			var result = validationResult.GetComparisonTable();

			var expectedTable = new StringBuilder()
										.AppendLine("| MyField equals Something |")
											.Append("| Something                |");

			Assert.AreEqual(expectedTable.ToString(), result);
		}

		/// <summary>
		/// Tests the get comparison table with invalid fields.
		/// </summary>
		[Test]
		public void TestGetComparisonTableWithInvalidFields()
		{
            var validation = ItemValidationHelper.Create("MyField", "Something");
			var validationResult = new ValidationResult(new[] { validation });

			var itemResult = new ValidationItemResult();
			itemResult.NoteValidationResult(validation, false, "Else");

			validationResult.CheckedItems.Add(itemResult);

			var result = validationResult.GetComparisonTable();

			var expectedTable = new StringBuilder()
										.AppendLine("| MyField equals Something |")
											.Append("| Else                     |");

			Assert.AreEqual(expectedTable.ToString(), result);
		}

		/// <summary>
		/// Tests the get comparison table with invalid null fields.
		/// </summary>
		[Test]
		public void TestGetComparisonTableWithInvalidNullFields()
		{
            var validation = ItemValidationHelper.Create("MyField", "Something");
			var validationResult = new ValidationResult(new[] { validation });

			var itemResult = new ValidationItemResult();
			itemResult.NoteValidationResult(validation, false, null);

			validationResult.CheckedItems.Add(itemResult);

			var result = validationResult.GetComparisonTable();

			var expectedTable = new StringBuilder()
										.AppendLine("| MyField equals Something |")
											.Append("| <EMPTY>                  |");

			Assert.AreEqual(expectedTable.ToString(), result);
		}

		/// <summary>
		/// Tests the get comparison table with missing fields.
		/// </summary>
		[Test]
		public void TestGetComparisonTableWithMissingFields()
		{
            var validation = ItemValidationHelper.Create("MyField", "Something");
			var validationResult = new ValidationResult(new[] { validation });

			var itemResult = new ValidationItemResult();
			itemResult.NoteMissingProperty(validation);

			validationResult.CheckedItems.Add(itemResult);

			var result = validationResult.GetComparisonTable();

			var expectedTable = new StringBuilder()
										.AppendLine("| MyField equals Something |")
											.Append("| <NOT FOUND>              |");

			Assert.AreEqual(expectedTable.ToString(), result);
		}
	}
}
