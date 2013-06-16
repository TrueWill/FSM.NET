using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace TrueWill.Fsm.CSharp.Tests
{
    public class ObjectOrientedFsmTests
    {
        // TODO: Null checks :(
        // TODO: Way to verify CLS-compliance?
        // TODO: More tests of terminal state.

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
        public void Constructor_WhenTransitionTableIsEmpty_Throws()
        {
            var transitionTable = Enumerable.Empty<Transition>();

            Assert.Throws<ArgumentException>(
                () => new StateMachine(transitionTable));
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