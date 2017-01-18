# Commit #1
The unit tests get 20% test coverage to start with, which seems bad. But most of the missing coverage turns out to be in the UI, because most of the "core" classes are covered at 80% or better. The places where it looks like additional coverage is most needed are RegexRef and RegexBuffer.

In spite of the fact that the existing unit tests (which are essentially integration tests for the core classes) get adequate coverage, I would prefer to see unit tests on the individual class level. By nature, that style of test is easier to debug and easier to make sure that each piece of functionality is actually tested.

So, for the RegexCharacter class, the "space" character in the constructor was not covered. I created a TestRegexCharacter unit test to try and attack this. Here is the first thing I did:

```c#
[Test]
public void TestSpace()
{
    var reChar = new RegexCharacter(new RegexBuffer(" "));
    Assert.IsFalse(reChar.Special);
    Assert.AreEqual("' ' (space)", reChar.ToString(0));
}
```

That is well and good. But this type of unit test runs the risk of duplicating the implementation too much, which tends to turn into a maintenance nightmare. One way to mitigate this which goes to a specific design improvement is to introduce constants for those strings, which can be shared by the implementation and the tests:

(in RegexCharacter.cs)
```c#
public const string SpaceDescription = "' ' (space)";
```

(in the unit test)
```c#
Assert.AreEqual(RegexCharacter.SpaceDescription, reChar.ToString(0));
```

With that, and a couple additional tests, I was able to bring coverage for the class in question up to 100% (from 87%) with about 20 minutes of effort, much of which was just to understand the class that I wasn't previously familiar with.

# Commit #2
What about the UI? There is an interesting question of how we should treat testing in the area of WinForms...

Looking at Form1, there are clearly some separable "business logic" components that can be split out. Probably the easiest thing to do is directly pull out helper methods with parameters and feed them from the UI. Take StringMaker as an example -- the methods MakeCSharpString and MakeVBString are easy to parameterize. These methods *could* be static, but static classes/methods typically aren't great for unit testing, and in this case it's not too hard to imagine StringMaker at some point having member variables.

# Commit #3
One approach that I've found to be effective in making legacy WinForms code testable on our project is to create an "actions" class to hold logic that operates on UI properties, then make that actions class work against an interface with all of the necessary properties and methods. Then have the form explicitly implement the interface, create an actions class as a field passing "this" to the ctor, and delegate to it in event handlers. It's basically an implementation of MVP, except in this case there's not necessarily any reason to separate out the model class. When all of that is complete, it's possible to unit-test the action class using mock objects.
