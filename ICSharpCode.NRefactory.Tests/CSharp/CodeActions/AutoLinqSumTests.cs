// AutoLinqTests.cs
//  
// Author:
//      Luís Reis <luiscubal@gmail.com>
// 
// Copyright (c) 2013 Luís Reis
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using NUnit.Framework;
using ICSharpCode.NRefactory.CSharp.Refactoring;

namespace ICSharpCode.NRefactory.CSharp.CodeActions
{
	[TestFixture]
	public class AutoLinqSumTests : ContextActionTestBase
	{
		[Test]
		public void TestSimpleIntegerLoop() {
			string source = @"
using System.Linq;

class TestClass
{
	void TestMethod() {
		int result = 0;
		var list = new int[] { 1, 2, 3 };
		$foreach (var x in list)
			result += x;
	}
}";

			string result = @"
using System.Linq;

class TestClass
{
	void TestMethod() {
		int result = 0;
		var list = new int[] { 1, 2, 3 };
		result += list.Sum ();
	}
}";

			Assert.AreEqual(result, RunContextAction(new AutoLinqSum(), source));
		}

		[Test]
		public void TestIntegerLoopInBlock() {
			string source = @"
using System.Linq;

class TestClass
{
	void TestMethod() {
		int result = 0;
		var list = new int[] { 1, 2, 3 };
		$foreach (var x in list) {
			result += x;
		}
	}
}";

			string result = @"
using System.Linq;

class TestClass
{
	void TestMethod() {
		int result = 0;
		var list = new int[] { 1, 2, 3 };
		result += list.Sum ();
	}
}";

			Assert.AreEqual(result, RunContextAction(new AutoLinqSum(), source));
		}

		[Test]
		public void TestDisabledForStrings() {
			string source = @"
using System.Linq;

class TestClass
{
	void TestMethod() {
		string result = string.Empty;
		var list = new string[] { ""a"", ""b"" };
		$foreach (var x in list) {
			result += x;
		}
	}
}";
			TestWrongContext<AutoLinqSum>(source);
		}

		[Test]
		public void TestShort() {
			string source = @"
using System.Linq;

class TestClass
{
	void TestMethod() {
		short result = 0;
		var list = new short[] { 1, 2, 3 };
		$foreach (var x in list)
			result += x;
	}
}";

			string result = @"
using System.Linq;

class TestClass
{
	void TestMethod() {
		short result = 0;
		var list = new short[] { 1, 2, 3 };
		result += list.Sum ();
	}
}";

			Assert.AreEqual(result, RunContextAction(new AutoLinqSum(), source));
		}

		[Test]
		public void TestLong() {
			string source = @"
using System.Linq;

class TestClass
{
	void TestMethod() {
		long result = 0;
		var list = new long[] { 1, 2, 3 };
		$foreach (var x in list)
			result += x;
	}
}";

			string result = @"
using System.Linq;

class TestClass
{
	void TestMethod() {
		long result = 0;
		var list = new long[] { 1, 2, 3 };
		result += list.Sum ();
	}
}";

			Assert.AreEqual(result, RunContextAction(new AutoLinqSum(), source));
		}
	}
}

