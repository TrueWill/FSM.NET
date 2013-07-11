module FunctionalFsmTests

open TrueWill.Fsm
open Xunit
open FsUnit.Xunit

let testTransitions =
    [ { CurrentState = "Locked";   TriggeringEvent = "coin"; NewState = "Unlocked" };
      { CurrentState = "Unlocked"; TriggeringEvent = "coin"; NewState = "Unlocked" };
      { CurrentState = "Unlocked"; TriggeringEvent = "pass"; NewState = "Locked" } ]

[<Fact>]
let getInitialState_WhenNoTransitions_ThrowsArgumentException () =
    (fun () -> Fsm.getInitialState Seq.empty |> ignore) |> should throw typeof<System.ArgumentException>

[<Fact>]
let getInitialState_WhenMultipleTransitions_ReturnsFirstCurrentState () =
    let initialState = Fsm.getInitialState testTransitions
    initialState |> should equal "Locked"

[<Fact>]
let getAvailableEvents_WhenValidState_ReturnsAvailableEvents () =
    let currentState = "Unlocked"
    let availableEvents = Fsm.getAvailableEvents currentState testTransitions

    Seq.toList availableEvents |> should equal [ "coin"; "pass" ]

[<Fact>]
let getNewState_WhenValidArguments_ReturnsNewState () =
    let currentState = "Locked"
    let selectedEvent = "coin"
    let newState = Fsm.getNewState currentState selectedEvent testTransitions

    newState |> should equal "Unlocked"

[<Fact>]
let getStates_WhenCalled_ReturnsStates () =
    let states = Fsm.getStates testTransitions

    Seq.toList states |> should equal ["Locked"; "Unlocked" ]
