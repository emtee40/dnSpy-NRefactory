//
// ReplaceWithLastOrDefaultTests.cs
//
// Author:
//       Mike Krüger <mkrueger@xamarin.com>
//
// Copyright (c) 2013 Xamarin Inc. (http://xamarin.com)
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
using ICSharpCode.NRefactory6.CSharp.Refactoring;
using ICSharpCode.NRefactory6.CSharp.CodeRefactorings;

namespace ICSharpCode.NRefactory6.CSharp.Diagnostics
{
	[TestFixture]
	public class ReplaceWithLastOrDefaultTests : InspectionActionTestBase
	{
		[Test]
		public void TestBasicCase()
		{
			Analyze<ReplaceWithLastOrDefaultAnalyzer>(@"using System.Linq;
class Bar
{
	public void FooBar(string[] args)
	{
		var first = $args.Any() ? args.Last() : null$;
	}
}", @"using System.Linq;
class Bar
{
	public void FooBar(string[] args)
	{
		var first = args.LastOrDefault();
	}
}");
		}

		[Test]
		public void TestBasicCaseWithExpression()
		{
			Analyze<ReplaceWithLastOrDefaultAnalyzer>(@"using System.Linq;
class Bar
{
    public void FooBar(string[] args)
    {
        $args.Any(a => a != null) ? args.Last(a => a != null) : null$;
    }
}", @"using System.Linq;
class Bar
{
    public void FooBar(string[] args)
    {
        args.LastOrDefault(a => a != null);
    }
}");
		}

		[Test]
		public void TestBasicCaseWithDefault()
		{
			Analyze<ReplaceWithLastOrDefaultAnalyzer>(@"using System.Linq;
class Bar
{
	public void FooBar<T>(T[] args)
	{
		var first = $args.Any() ? args.Last() : default(T)$;
	}
}", @"using System.Linq;
class Bar
{
	public void FooBar<T>(T[] args)
	{
		var first = args.LastOrDefault();
	}
}");
		}

		[Test]
		public void TestDisable()
		{
			Analyze<ReplaceWithLastOrDefaultAnalyzer>(@"using System.Linq;
class Bar
{
	public void FooBar(string[] args)
	{
		// ReSharper disable once ReplaceWithLastOrDefault
		var first = args.Any() ? args.Last() : null;
	}
}");
		}
	}
}
