﻿// 
// ConstantConditionTests.cs
// 
// Author:
//      Mansheng Yang <lightyang0@gmail.com>
// 
// Copyright (c) 2012 Mansheng Yang <lightyang0@gmail.com>
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

using ICSharpCode.NRefactory6.CSharp.Refactoring;
using NUnit.Framework;

namespace ICSharpCode.NRefactory6.CSharp.Diagnostics
{
	[TestFixture]
	public class ConstantConditionTests : InspectionActionTestBase
	{
		[Test]
		public void TestConditionalExpression ()
		{
			Analyze<ConstantConditionAnalyzer> (@"
class TestClass
{
	void TestMethod ()
	{
		var a = $1 > 0$ ? 1 : 0;
		var b = $1 < 0$ ? 1 : 0;
	}
}", @"
class TestClass
{
	void TestMethod ()
	{
		var a = 1;
		var b = 0;
	}
}");
		}

		[Test]
		public void TestIf ()
		{
			Analyze<ConstantConditionAnalyzer> (@"
class TestClass
{
    void TestMethod ()
    {
        int i;
        if ($1 > 0$)
        	i = 1;
        if ($1 > 0$) {
        	i = 1;
        }
        if ($1 < 0$)
        	i = 1;
        if ($1 == 0$) {
        	i = 1;
        } else {
        	i = 0;
        }
        if ($1 == 0$) {
        	i = 1;
        } else
        	i = 0;
    }
}", @"
class TestClass
{
    void TestMethod ()
    {
        int i;
        i = 1;
        i = 1;
        i = 0;
        i = 0;
    }
}");
		}

		[Test]
		public void TestFor ()
		{
			Analyze<ConstantConditionAnalyzer> (@"
class TestClass
{
	void TestMethod ()
	{
		for (int i = 0; $1 > 0$; i++) ;
	}
}", @"
class TestClass
{
	void TestMethod ()
	{
		for (int i = 0; true; i++) ;
	}
}");
		}

		[Test]
		public void TestWhile ()
		{
			Analyze<ConstantConditionAnalyzer> (@"
class TestClass
{
	void TestMethod ()
	{
		while ($1 > 0$)
			;
	}
}", @"
class TestClass
{
	void TestMethod ()
	{
		while (true)
			;
	}
}");
		}

		[Test]
		public void TestDoWhile ()
		{
			Analyze<ConstantConditionAnalyzer> (@"
class TestClass
{
	void TestMethod ()
	{
		do {
		} while ($1 < 0$);
	}
}", @"
class TestClass
{
	void TestMethod ()
	{
		do {
		} while (false);
	}
}");
		}

		[Test]
		public void TestNoIssue ()
		{
			Analyze<ConstantConditionAnalyzer> (@"
class TestClass
{
	void TestMethod (int x = true)
	{
		while (true) ;
		if (false) ;
		if (x) ;
	}
}");
		}
	}
}