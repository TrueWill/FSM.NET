module FunctionalFsmTests

open TrueWill.Fsm
open Xunit
open FsUnit.Xunit

[<Fact>]
let getInitialState_WhenNoTransitions_ThrowsArgumentException () =
    (fun () -> Fsm.getInitialState Seq.empty |> ignore) |> should throw typeof<System.ArgumentException>
