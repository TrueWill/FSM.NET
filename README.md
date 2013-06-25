FSM.NET
=======

A simple &quot;stateless&quot; finite-state machine library for .NET, written in F#.

*Now available through [NuGet](https://nuget.org/packages/FSM.NET/)!*

## Origin/Credits

I was reading
[Agile Principles, Patterns, and Practices in C#](http://www.amazon.com/Agile-Principles-Patterns-Practices-C/dp/0131857258)
by Robert C. Martin and Micah Martin, and fixated on Uncle Bob's
State Machine Compiler (SMC). That reads a state transition table and
generates C++ code.

I wanted to write a functional library that would do the same - read
a textual DSL (domain-specific language) that defined a state transition table,
and create a Semantic Model ([Fowler](http://martinfowler.com/books/dsl.html)).

I borrowed parsing techniques from Fowler's book (see above link).

This project also is helping me to learn F#. :) 

[Keith Dahlby](https://twitter.com/dahlbyk) provided help and encouragement.

## Build

Visual Studio 2012, targeting .NET 4.0 under Windows. It doesn't need 4.5
features, and I don't want to restrict its use to Windows 7+.

The assembly should be CLS-compliant, and it should be natural to call from
C# (to give it the widest possible audience). There are unit tests in both
F# and C# to insure this.

I would prefer to avoid adding dependencies on other libraries to the core
library. (The tests depend on xUnit.net, FsUnit, etc.)

## State Transition Tables

### Format

    CurrentState|triggeringEvent|NewState

One per line.  
The first state of the first line is the initial state.

Anything following a '#' is ignored (a comment).
Blank lines are ignored.

Whitespace surrounding states and events is ignored, so you can line up
delimiters if you prefer.

### Example

    Locked|coin|Unlocked
    Unlocked|coin|Unlocked
    Unlocked|pass|Locked

## Philosophy

This is a solution in search of a problem. That's not a good thing.
Dogfooding will almost certainly improve the design.

The idea is that a web service would reference FSM. The state transition
tables could be provided by callers, loaded from a data store, stored in
configuration - it doesn't matter. The callers would be responsible for
storing their current state, and would pass it to the service.

Say I have a simple workflow, defined in a state machine.

1. Ask the service for the initial state
2. Ask the service for the available events
3. Display the available events to the user
4. User makes selection
5. Ask the service for the new state, given the old state and the event
6. Repeat from #2

You'd want to combine this with a data store and security.

I do **not** want to make FSM complicated. If you want to fork the project
and add the ability to run custom code on transitions, feel free, but I
probably won't accept that pull request. In particular, I want the DSL to
be self-contained, without requiring binary references. (I'm willing to
listen to ideas, though!)

## To Do

+ Much better documentation
+ Psake build script?
+ Example of one intended use - web service + desktop app
+ Refactor to TransitionTableValidator - trimmed, not blank, interop null checks (on transition elements, too), differ only by case, etc.

Finally, I'm very new to Git and GitHub, so please be patient with me.

Thanks,  
Bill
