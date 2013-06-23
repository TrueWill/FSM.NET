module TransitionTests

open TrueWill.Fsm
open Xunit
open FsUnit.Xunit

// TODO: Consider overriding ToString
[<Fact>]
let toString_WhenCalled_ReturnsText () =
    let transition = { CurrentState = "Current"; TriggeringEvent = "event"; NewState = "New" }
    let s = transition.AsString()
    s |> should equal "Current|event|New"
