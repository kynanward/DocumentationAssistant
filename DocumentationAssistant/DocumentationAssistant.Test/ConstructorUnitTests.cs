using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace DocumentationAssistant.Test
{
	/// <summary>
	/// The constructor unit test.
	/// </summary>
	[TestClass]
	public class ConstrcutorUnitTest : CodeFixVerifier
	{
		/// <summary>
		/// The inherit doc test code.
		/// </summary>
		private const string InheritDocTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	class ConstructorTester
	{
		/// <inheritdoc/>
		public ConstructorTester()
		{
		}
	}
}";

		/// <summary>
		/// The public constructor test code.
		/// </summary>
		private const string PublicConstructorTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	class ConstructorTester
	{
		public ConstructorTester()
		{
		}
	}
}";

		/// <summary>
		/// The public contructor test fix code.
		/// </summary>
		private const string PublicContructorTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	class ConstructorTester
	{
        /// <summary>
        /// Initializes a new instance of the <see cref=""ConstructorTester""/> class.
        /// </summary>
        public ConstructorTester()
		{
		}
	}
}";

		/// <summary>
		/// The private constructor test code.
		/// </summary>
		private const string PrivateConstructorTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	class ConstructorTester
	{
		private ConstructorTester()
		{
		}
	}
}";

		/// <summary>
		/// The private contructor test fix code.
		/// </summary>
		private const string PrivateContructorTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	class ConstructorTester
	{
        /// <summary>
        /// Prevents a default instance of the <see cref=""ConstructorTester""/> class from being created.
        /// </summary>
        private ConstructorTester()
		{
		}
	}
}";

		/// <summary>
		/// The public constructor test code.
		/// </summary>
		private const string PublicConstructorWithBooleanParameterTestCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	class ConstructorTester
	{
		public ConstructorTester(bool isRed, bool? isAssociatedWithAllProduct)
		{
		}
	}
}";

		/// <summary>
		/// The public contructor test fix code.
		/// </summary>
		private const string PublicContructorWithBooleanParameterTestFixCode = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp4
{
	class ConstructorTester
	{
        /// <summary>
        /// Initializes a new instance of the <see cref=""ConstructorTester""/> class.
        /// </summary>
        /// <param name=""isRed"">If true, is red.</param>
        /// <param name=""isAssociatedWithAllProduct"">If true, is associated with all product.</param>
        public ConstructorTester(bool isRed, bool? isAssociatedWithAllProduct)
		{
		}
	}
}";
		/// <summary>
		/// Nos diagnostics show.
		/// </summary>
		/// <param name="testCode">The test code.</param>
		[DataTestMethod]
		[DataRow("")]
		[DataRow(InheritDocTestCode)]
		public void NoDiagnosticsShow(string testCode)
		{
			this.VerifyCSharpDiagnostic(testCode);
		}

		/// <summary>
		/// Shows diagnostic and fix.
		/// </summary>
		/// <param name="testCode">The test code.</param>
		/// <param name="fixCode">The fix code.</param>
		/// <param name="line">The line.</param>
		/// <param name="column">The column.</param>
		[DataTestMethod]
		[DataRow(PublicConstructorTestCode, PublicContructorTestFixCode, 10, 10)]
		[DataRow(PrivateConstructorTestCode, PrivateContructorTestFixCode, 10, 11)]
		[DataRow(PublicConstructorWithBooleanParameterTestCode, PublicContructorWithBooleanParameterTestFixCode, 10, 10)]
		public void ShowDiagnosticAndFix(string testCode, string fixCode, int line, int column)
		{
			var expected = new DiagnosticResult
			{
				Id = ConstructorAnalyzer.DiagnosticId,
				Message = ConstructorAnalyzer.MessageFormat,
				Severity = DiagnosticSeverity.Info,
				Locations =
					new[] {
							new DiagnosticResultLocation("Test0.cs", line, column)
						}
			};

			this.VerifyCSharpDiagnostic(testCode, expected);

			this.VerifyCSharpFix(testCode, fixCode);
		}

		/// <summary>
		/// Gets c sharp code fix provider.
		/// </summary>
		/// <returns>A CodeFixProvider.</returns>
		protected override CodeFixProvider GetCSharpCodeFixProvider()
		{
			return new ConstructorCodeFixProvider();
		}

		/// <summary>
		/// Gets c sharp diagnostic analyzer.
		/// </summary>
		/// <returns>A DiagnosticAnalyzer.</returns>
		protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
		{
			return new ConstructorAnalyzer();
		}
	}
}
