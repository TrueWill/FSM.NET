module ObjectOrientedFsmTests

open System.Collections.Generic
open System.Linq
open TrueWill.Fsm
open Xunit
open FsUnit.Xunit

[<Fact>]
let states_WhenGet_ReturnsStates () =
    let transitionTable = new List<Transition>()
    transitionTable.Add({ CurrentState = "A"; TriggeringEvent = "e1"; NewState = "B" })
    transitionTable.Add({ CurrentState = "B"; TriggeringEvent = "e2"; NewState = "C" })
    transitionTable.Add({ CurrentState = "A"; TriggeringEvent = "e3"; NewState = "C" })
    transitionTable.Add({ CurrentState = "B"; TriggeringEvent = "e4"; NewState = "D" })

    let sut = new StateMachine(transitionTable)

    let states = sut.States

    states.ToArray() |> should equal [| "A"; "B"; "C"; "D" |]
