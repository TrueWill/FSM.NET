using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace TrueWill.Fsm.CSharp.Tests
{
    public class ObjectOrientedFsmTests
    {
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

            var fsm = new StateMachine(transitionTable);

            IEnumerable<string> states = fsm.States;

            Assert.Equal(new[] {"A", "B", "C", "D"}, states.ToArray());
        }
    }
}