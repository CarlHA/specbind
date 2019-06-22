// <copyright file="TableFormatterFixture.cs">
//    Copyright © 2013 Dan Piessens  All rights reserved.
// </copyright>

namespace SpecBind.Tests.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using NUnit.Framework;
	using SpecBind.Helpers;

	/// <summary>
	/// A test class for validating the able formatter.
	/// </summary>
	[TestFixture]
	public class TableFormatterFixture
	{
		/// <summary>
		/// Tests the add column with null cell selector throws an exception.
		/// </summary>
		[Test]
		public void TestAddColumnWithNullCellSelectorThrowsException()
		{
			var tableFormatter = new TableFormater<MyFormatItem>();

			Assert.Throws<ArgumentNullException>(() =>
			                                     {
				                                     try
				                                     {
					                                     tableFormatter.AddColumn<string>("BadColumn", null, null);
				                                     }
				                                     catch (ArgumentNullException ex)
				                                     {
					                                     Assert.AreEqual("cellSelector", ex.ParamName);
					                                     throw;
				                                     }
			                                     });
		}

		/// <summary>
		/// Tests the add column with null value selector throws an exception.
		/// </summary>
		[Test]
		public void TestAddColumnWithNullValueSelectorThrowsException()
		{
			var tableFormatter = new TableFormater<MyFormatItem>();

			Assert.Throws<ArgumentNullException>(() =>
			                                     {
				                                     try
				                                     {
					                                     tableFormatter.AddColumn("BadColumn", i => "Hello", null);
				                                     }
				                                     catch (ArgumentNullException ex)
				                                     {
					                                     Assert.AreEqual("valueSelector", ex.ParamName);
					                                     throw;
				                                     }
			                                     });
		}

		/// <summary>
		/// Tests the add column with correct values.
		/// </summary>
		[Test]
		public void TestAddColumnWithValidSelectors()
		{
			var tableFormatter = new TableFormater<MyFormatItem>();

			tableFormatter.AddColumn("BadColumn", i => "Hello", i => i);

			Assert.AreEqual(1, tableFormatter.ColumnCount);
		}

		/// <summary>
		/// Tests the rendering of a simple table that has all valid values.
		/// </summary>
		[Test]
		public void TestPrintValidTable()
		{
			var items = new[]
			            {
				            new MyFormatItem { Name = "Bob", Age = 21 },
							new MyFormatItem { Name = "Alice", Age = 5 }
			            };

			var tableFormatter = new TableFormater<MyFormatItem>()
										.AddColumn("Name", i => i.Name, c => c)
										.AddColumn("Age", i => i.Age, c => c.ToString("D"));

			var result = tableFormatter.CreateTable(items);

			var expectedTable = new StringBuilder()
										.AppendLine("| Name  | Age |")
										.AppendLine("| Bob   | 21  |")
										    .Append("| Alice | 5   |");

			Assert.AreEqual(expectedTable.ToString(), result);
		}

		/// <summary>
		/// Tests the rendering returns null if no rows exist and the exclude option is enabled.
		/// </summary>
		[Test]
		public void TestPrintTableWithRowExclusionEnableReturnsNullIfNoRowsExist()
		{
			var tableFormatter = new TableFormater<MyFormatItem>()
										.ExcludePrintingIfNoRows()
										.AddColumn("Name", i => i.Name, c => c)
										.AddColumn("Age", i => i.Age, c => c.ToString("D"));

			var result = tableFormatter.CreateTable(new List<MyFormatItem>(0));

			Assert.IsNull(result);
		}

		/// <summary>
		/// Tests the rendering returns just the headers if no rows exist.
		/// </summary>
		[Test]
		public void TestPrintTableWithNoRowsReturnsJustHeaders()
		{
			var tableFormatter = new TableFormater<MyFormatItem>()
										.AddColumn("Name", i => i.Name, c => c)
										.AddColumn("Age", i => i.Age, c => c.ToString("D"));

			var result = tableFormatter.CreateTable(new List<MyFormatItem>(0));

			Assert.AreEqual("| Name | Age |", result);
		}

		/// <summary>
		/// Tests the rendering of a simple table that has all valid values and a validation selector that returns null.
		/// </summary>
		[Test]
		public void TestPrintValidTableWithNullValidationFunctionTreatsValuesAsTrue()
		{
			var items = new[]
			            {
				            new MyFormatItem { Name = "Bob", Age = 21 },
							new MyFormatItem { Name = "Alice", Age = 5 }
			            };

			var tableFormatter = new TableFormater<MyFormatItem>()
										.AddColumn("Name", i => i.Name, c => c)
										.AddColumn("Age", i => i.Age, c => c.ToString("D"), i => null);

			var result = tableFormatter.CreateTable(items);

			var expectedTable = new StringBuilder()
										.AppendLine("| Name  | Age |")
										.AppendLine("| Bob   | 21  |")
											.Append("| Alice | 5   |");

			Assert.AreEqual(expectedTable.ToString(), result);
		}

		/// <summary>
		/// Tests the rendering of a simple table that has all valid values, but some are null.
		/// </summary>
		[Test]
		public void TestPrintValidTableWithNullValues()
		{
			var items = new[]
			            {
				            new MyFormatItem { Name = "Bob", Age = 21 },
							new MyFormatItem { Name = null, Age = 5 }
			            };

			var tableFormatter = new TableFormater<MyFormatItem>()
										.AddColumn("Name", i => i.Name, c => c)
										.AddColumn("Age", i => i.Age, c => c.ToString("D"));

			var result = tableFormatter.CreateTable(items);

			var expectedTable = new StringBuilder()
										.AppendLine("| Name    | Age |")
										.AppendLine("| Bob     | 21  |")
										    .Append("| <EMPTY> | 5   |");

			Assert.AreEqual(expectedTable.ToString(), result);
		}

		/// <summary>
		/// Tests the rendering of a simple table that has valid and invalid columns.
		/// </summary>
		[Test]
		public void TestPrintValidTableWithInvalidValues()
		{
			var items = new[]
			            {
				            new MyFormatItem { Name = "Bob", Age = 21 },
							new MyFormatItem { Name = "Alice", Age = 5 }
			            };

			var tableFormatter = new TableFormater<MyFormatItem>()
										.AddColumn("Name", i => i.Name, c => c)
										.AddColumn("Age", i => i.Age, c => c.ToString("D"), i => new Tuple<bool, string>(i >= 21, "21"));

			var result = tableFormatter.CreateTable(items);

			var expectedTable = new StringBuilder()
										.AppendLine("| Name  | Age    |")
										.AppendLine("| Bob   | 21     |")
											.Append("| Alice | 5 [21] |");

			Assert.AreEqual(expectedTable.ToString(), result);
		}

		/// <summary>
		/// Tests the rendering of a simple table that has valid and invalid columns.
		/// </summary>
		[Test]
		public void TestPrintValidTableWithInvalidNullValues()
		{
			var items = new[]
			            {
				            new MyFormatItem { Name = "Bob", Age = 21 },
							new MyFormatItem { Name = "Alice", Age = 5 }
			            };

			var tableFormatter = new TableFormater<MyFormatItem>()
										.AddColumn("Name", i => i.Name, c => c)
										.AddColumn("Age", i => i.Age, c => c.ToString("D"), i => new Tuple<bool, string>(i >= 21, null));

			var result = tableFormatter.CreateTable(items);

			var expectedTable = new StringBuilder()
										.AppendLine("| Name  | Age         |")
										.AppendLine("| Bob   | 21          |")
											.Append("| Alice | 5 [<EMPTY>] |");

			Assert.AreEqual(expectedTable.ToString(), result);
		}

		/// <summary>
		/// A test class for formatting the item.
		/// </summary>
		private class MyFormatItem
		{
			/// <summary>
			/// Gets or sets the name.
			/// </summary>
			/// <value>The name.</value>
			public string Name { get; set; }

			/// <summary>
			/// Gets or sets the age.
			/// </summary>
			/// <value>The age.</value>
			public int Age { get; set; }
		}
	}
}