//
// PossibleAssignmentToReadonlyFieldIssueTests.cs
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
using ICSharpCode.NRefactory.CSharp.Refactoring;
using NUnit.Framework;

namespace ICSharpCode.NRefactory.CSharp.CodeIssues
{
	[TestFixture]
	public class PossibleAssignmentToReadonlyFieldIssueTests : InspectionActionTestBase
	{
		[Test]
		public void TestTypeParameter()
		{
			TestIssue<PossibleAssignmentToReadonlyFieldIssue>(@"
interface IFoo
{
	int Property { get; set; }
}

class FooBar<T> where T : IFoo
{
	readonly T field;

	void Test ()
	{
		field.Property = 7;
	}
}
");
		}

		[Test]
		public void TestClassTypeParameter()
		{
			TestWrongContext<PossibleAssignmentToReadonlyFieldIssue>(@"
interface IFoo
{
	int Property { get; set; }
}

class FooBar<T> where T : class, IFoo
{
	readonly T field;

	void Test ()
	{
		field.Property = 7;
	}
}
");
		}

		[Test]
		public void TestValueType()
		{
			TestIssue<PossibleAssignmentToReadonlyFieldIssue>(@"
interface IFoo
{
	int Property { get; set; }
}

struct Bar : IFoo
{
	public int Property { get; set; }
}

class FooBar<T> where T : IFoo
{
	readonly Bar field;

	public static void Foo()
	{
		var a = new FooBar<Bar>();
		a.field.Property = 7;
	}
}
");
		}

		[Test]
		public void TestDisable()
		{
			TestWrongContext<PossibleAssignmentToReadonlyFieldIssue>(@"
interface IFoo
{
	int Property { get; set; }
}

struct Bar : IFoo
{
	public int Property { get; set; }
}

class FooBar<T> where T : IFoo
{
	readonly T field;

	public static void Foo()
	{
		var a = new FooBar<Bar>();
		// ReSharper disable once PossibleAssignmentToReadonlyField
		a.field.Property = 7;
	}
}
");
		}
	}

}

