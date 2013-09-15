module FunctionalFsmTests

open System
open TrueWill.Fsm
open Xunit
open FsUnit.Xunit

let testTransitions =
    [ { CurrentState = "Locked";   TriggeringEvent = "coin"; NewState = "Unlocked" };
      { CurrentState = "Unlocked"; TriggeringEvent = "coin"; NewState = "Unlocked" };
      { CurrentState = "Unlocked"; TriggeringEvent = "pass"; NewState = "Locked" } ]

[<Fact>]
let getInitialState_WhenNoTransitions_ThrowsArgumentException () =
    (fun () -> Fsm.getInitialState Seq.empty |> ignore) |> should throw typeof<ArgumentException>

[<Fact>]
let getInitialState_WhenMultipleTransitions_ReturnsFirstCurrentState () =
    let initialState = Fsm.getInitialState testTransitions
    initialState |> should equal "Locked"

[<Fact>]
let getAvailableEvents_WhenValidState_ReturnsAvailableEvents () =
    let currentState = "Unlocked"
    let getAvailableEvents = Fsm.getAvailableEvents testTransitions
    let availableEvents = getAvailableEvents currentState

    Seq.toList availableEvents |> should equal [ "coin"; "pass" ]

[<Fact>]
let getNewState_WhenValidArguments_ReturnsNewState () =
    let currentState = "Locked"
    let selectedEvent = "coin"
    let getNewState = Fsm.getNewState testTransitions
    let newState = getNewState currentState selectedEvent

    newState |> should equal "Unlocked"

[<Fact>]
let getNewState_WhenTransitionToSameState_ReturnsSameState () =
    let currentState = "Unlocked"
    let selectedEvent = "coin"
    let getNewState = Fsm.getNewState testTransitions
    let newState = getNewState currentState selectedEvent

    newState |> should equal "Unlocked"

[<Fact>]
let getNewState_WhenMultipleEventsResultInSameTransition_ReturnsIdenticalResults () =
    let transitions =
        [ { CurrentState = "A"; TriggeringEvent = "1"; NewState = "B" };
          { CurrentState = "A"; TriggeringEvent = "2"; NewState = "B" } ]

    let currentState = "A"
    let getNewState = Fsm.getNewState transitions
    let newState1 = getNewState currentState "1"
    let newState2 = getNewState currentState "2"

    newState1 |> should equal newState2

[<Fact>]
let getStates_WhenCalled_ReturnsStates () =
    let states = Fsm.getStates testTransitions

    Seq.toList states |> should equal ["Locked"; "Unlocked" ]
