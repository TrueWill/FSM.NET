using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Extensions;

namespace TrueWill.Fsm.CSharp.Tests
{
    public class ObjectOrientedFsmTests
    {
        [Fact]
        public void Assembly_WhenLoaded_IsClsCompliant()
        {
            // Thanks to http://mcbeanit.blogspot.com/2012/10/c-testing-for-cls-compliance.html

            var fsmAssembly = Assembly.GetAssembly(typeof (StateMachine));

            var attributes = fsmAssembly.GetCustomAttributes(typeof(CLSCompliantAttribute), false);

            Assert.Single(attributes);

            var csl = (CLSCompliantAttribute) attributes.First();

            Assert.True(csl.IsCompliant);
        }

        [Fact]
        public void Constructor_WhenCalled_CopiesTransitionTable()
        {
            var transitionTable =
                new List<Transition>
                    {
                        new Transition("Locked", "coin", "Unlocked")
                    };

            var sut = new StateMachine(transitionTable);

            transitionTable.Add(new Transition("Unlocked", "pass", "Locked"));

            Assert.Throws<InvalidOperationException>(
                () => sut.GetNewState("Unlocked", "pass"));
        }

        [Fact]
        public void Constructor_WhenTransitionTableContainsNull_Throws()
        {
            var transitionTable =
                new List<Transition>
                    {
                        new Transition("Locked", "coin", "Unlocked"),
                        null
                    };

            Assert.Throws<ArgumentException>(
                () => new StateMachine(transitionTable));
        }

        [Fact]
        public void Constructor_WhenTransitionTableIsEmpty_Throws()
        {
            var transitionTable = Enumerable.Empty<Transition>();

            Assert.Throws<ArgumentException>(
                () => new StateMachine(transitionTable));
        }

        [Fact]
        public void Constructor_WhenTransitionTableIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => new StateMachine(null));
        }

        [Fact]
        public void Constructor_WhenUnreachableState_CreatesWithAllStates()
        {
            // Insure that it supports migrations with transitional state machines.

            var transitionTable =
                new List<Transition>
                    {
                        new Transition("A", "e1", "B"),
                        new Transition("D", "e2", "E")
                    };

            var sut = new StateMachine(transitionTable);

            IEnumerable<string> states = sut.States;

            Assert.Equal(new[] { "A", "B", "D", "E" }, states.ToArray());
        }

        [Fact]
        public void GetAvailableEvents_WhenCurrentStateIsNull_Throws()
        {
            var sut = new StateMachine(GetTestTransitionTable());

            Assert.Throws<ArgumentNullException>(
                () => sut.GetAvailableEvents(null));
        }

        [Fact]
        public void GetAvailableEvents_WhenInvalidState_Throws()
        {
            var sut = new StateMachine(GetTestTransitionTable());

            var ex = Assert.Throws<InvalidOperationException>(
                () => sut.GetAvailableEvents("Bogus"));

            Assert.Equal("Invalid state: 'Bogus'.", ex.Message);
        }

        [Fact]
        public void GetAvailableEvents_WhenTerminalState_ReturnsEmpty()
        {
            const string TerminalState = "Terminal";

            var transitionTable =
                new List<Transition>
                    {
                        new Transition("Whatever", "something", TerminalState)
                    };

            var sut = new StateMachine(transitionTable);

            IEnumerable<string> result = sut.GetAvailableEvents(TerminalState);

            Assert.Empty(result);
        }

        [Theory]
        [InlineData("Locked", new[] { "coin" })]
        [InlineData("Unlocked", new[] { "coin", "pass" })]
        public void GetAvailableEvents_WhenValidState_ReturnsAvailableEvents(
            string currentState, string[] expectedEvents)
        {
            var sut = new StateMachine(GetTestTransitionTable());

            IEnumerable<string> result = sut.GetAvailableEvents(currentState);

            Assert.Equal(expectedEvents, result.ToArray());
        }

        [Fact]
        public void GetNewState_WhenCurrentStateIsNull_Throws()
        {
            var sut = new StateMachine(GetTestTransitionTable());

            Assert.Throws<ArgumentNullException>(
                () => sut.GetNewState(null, "coin"));
        }

        [Fact]
        public void GetNewState_WhenInvalidCombination_Throws()
        {
            var sut = new StateMachine(GetTestTransitionTable());

            Assert.Throws<InvalidOperationException>(
                () => sut.GetNewState("Locked", "pass"));
        }

        [Fact]
        public void GetNewState_WhenInvalidCurrentState_Throws()
        {
            var sut = new StateMachine(GetTestTransitionTable());

            var ex = Assert.Throws<InvalidOperationException>(
                () => sut.GetNewState("Bogus", "coin"));

            Assert.Equal("Invalid state transition: state 'Bogus', event 'coin'.", ex.Message);
        }

        [Fact]
        public void GetNewState_WhenInvalidEvent_Throws()
        {
            var sut = new StateMachine(GetTestTransitionTable());

            Assert.Throws<InvalidOperationException>(
                () => sut.GetNewState("Locked", "bogus"));
        }

        [Theory]
        [InlineData("Locked", "coin", "Unlocked")]
        [InlineData("Unlocked", "coin", "Unlocked")]
        [InlineData("Unlocked", "pass", "Locked")]
        public void GetNewState_WhenValidArguments_ReturnsNewState(
            string currentState, string triggeringEvent, string expectedNewState)
        {
            var sut = new StateMachine(GetTestTransitionTable());

            string result = sut.GetNewState(currentState, triggeringEvent);

            Assert.Equal(expectedNewState, result);
        }

        [Fact]
        public void GetNewState_WhenTerminalCurrentState_Throws()
        {
            const string TerminalState = "Terminal";
            const string TriggeringEvent = "ev";

            var transitionTable =
                new List<Transition>
                    {
                        new Transition("Whatever", TriggeringEvent, TerminalState)
                    };

            var sut = new StateMachine(transitionTable);

            Assert.Throws<InvalidOperationException>(
                () => sut.GetNewState(TerminalState, TriggeringEvent));
        }

        [Fact]
        public void GetNewState_WhenTriggeringEventIsNull_Throws()
        {
            var sut = new StateMachine(GetTestTransitionTable());

            Assert.Throws<ArgumentNullException>(
                () => sut.GetNewState("Locked", null));
        }

        [Fact]
        public void InitialState_WhenMultipleTransitions_ReturnsFirstCurrentState()
        {
            const string FirstCurrentState = "Locked";

            var transitionTable =
                new List<Transition>
                    {
                        new Transition(FirstCurrentState, "coin", "Unlocked"),
                        new Transition("Unlocked", "coin", "Unlocked"),
                        new Transition("Unlocked", "pass", "Zebra")
                    };

            var sut = new StateMachine(transitionTable);

            Assert.Equal(FirstCurrentState, sut.InitialState);
        }

        [Fact]
        public void InitialState_WhenOneTransition_ReturnsCurrentState()
        {
            const string CurrentState = "Locked";

            var transitionTable =
                new List<Transition>
                    {
                        new Transition(CurrentState, "coin", "Unlocked")
                    };

            var sut = new StateMachine(transitionTable);

            string result = sut.InitialState;

            Assert.Equal(CurrentState, result);
        }

        [Fact]
        public void Parse_WhenBasic_ReturnsCollection()
        {
            const string TableText =
                "Locked|coin|Unlocked\r\n" +
                "Unlocked|coin|Unlocked\r\n" +
                "Unlocked|pass|Locked";

            IEnumerable<Transition> result = TransitionTableParser.Parse(TableText);

            var expected =
                new []
                    {
                        new Transition("Locked", "coin", "Unlocked"),
                        new Transition("Unlocked", "coin", "Unlocked"),
                        new Transition("Unlocked", "pass", "Locked")
                    };

            Assert.Equal(expected, result.ToArray());
        }

        [Fact]
        public void Parse_WhenTableTextIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => TransitionTableParser.Parse(null));
        }

        [Fact]
        public void States_WhenGet_ReturnsStates()
        {
            var transitionTable =
                new List<Transition>
                    {
                        new Transition("A", "e1", "B"),
                        new Transition("B", "e2", "C"),
                        new Transition("A", "e3", "C"),
                        new Transition("B", "e4", "D")
                    };

            var sut = new StateMachine(transitionTable);

            IEnumerable<string> result = sut.States;

            Assert.Equal(new[] { "A", "B", "C", "D" }, result.ToArray());
        }

        private static IEnumerable<Transition> GetTestTransitionTable()
        {
            yield return new Transition("Locked", "coin", "Unlocked");
            yield return new Transition("Unlocked", "coin", "Unlocked");
            yield return new Transition("Unlocked", "pass", "Locked");
        }
    }
}