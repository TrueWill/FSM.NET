module TransitionTests

open TrueWill.Fsm
open Xunit
open FsUnit.Xunit

[<Fact>]
let toString_WhenCalled_ReturnsText () =
    let transition = { CurrentState = "Current"; TriggeringEvent = "event"; NewState = "New" }
    let s = transition.ToString()
    s |> should equal "Current|event|New"
