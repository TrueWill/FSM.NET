module ValidationTests

open System
open TrueWill.Fsm
open TrueWill.Fsm.Validator
open Xunit
open Xunit.Extensions
open FsUnit.Xunit

[<Fact>]
let validate_WhenNoTransitions_Throws () =
    (fun () -> validate Seq.empty |> ignore) |> should throw typeof<ArgumentException>

[<Fact>]
let validate_WhenTransitionsNull_Throws () =
    (fun () -> validate null |> ignore) |> should throw typeof<ArgumentNullException>

[<Fact>]
let validate_WhenTransitionsContainsNull_Throws () =
    let nullTransition = Unchecked.defaultof<Transition>

    let transitions =
        [ { CurrentState = "Locked"; TriggeringEvent = "coin"; NewState = "Unlocked" };
          nullTransition ]

    (fun () -> validate transitions |> ignore) |> should throw typeof<ArgumentNullException>

[<Theory>]
[<InlineData(null,      "coin",  "Unlocked")>]
[<InlineData("Locked",  null,    "Unlocked")>]
[<InlineData("Locked",  "coin",  null)>]
[<InlineData(" Locked", "coin",  "Unlocked")>]
[<InlineData("Locked",  "coin ", "Unlocked")>]
[<InlineData("Locked",  "coin",  " Unlocked ")>]
[<InlineData("",        "coin",  "Unlocked")>]
[<InlineData("Locked",  "",      "Unlocked")>]
[<InlineData("Locked",  "coin",  "")>]
let validate_WhenInvalidTransition_Throws currentState triggeringEvent newState =
    let transitions =
        [ { CurrentState = currentState; TriggeringEvent = triggeringEvent; NewState = newState } ]

    (fun () -> validate transitions |> ignore) |> should throw typeof<ArgumentException>    

[<Fact>]
let validate_WhenStatesDifferOnlyByCase_Throws () =
    let transitions =
        [ { CurrentState = "Locked";   TriggeringEvent = "coin"; NewState = "Unlocked" };
          { CurrentState = "Unlocked"; TriggeringEvent = "coin"; NewState = "Unlocked" };
          { CurrentState = "Unlocked"; TriggeringEvent = "pass"; NewState = "locked" } ]

    let ex = Assert.Throws<ArgumentException>(fun () -> validate transitions |> ignore)

    ex.Message |> should startWith "Some states differ only by case."

[<Fact>]
let validate_WhenEventsDifferOnlyByCase_Throws () =
    let transitions =
        [ { CurrentState = "Locked";   TriggeringEvent = "coin"; NewState = "Unlocked" };
          { CurrentState = "Unlocked"; TriggeringEvent = "Coin"; NewState = "Unlocked" };
          { CurrentState = "Unlocked"; TriggeringEvent = "pass"; NewState = "Locked" } ]

    let ex = Assert.Throws<ArgumentException>(fun () -> validate transitions |> ignore)

    ex.Message |> should startWith "Some events differ only by case."

[<Fact>]
let validate_WhenConflictingTransitions_Throws () =
    let transitions =
        [ { CurrentState = "Unlocked"; TriggeringEvent = "coin"; NewState = "Locked" };
          { CurrentState = "Unlocked"; TriggeringEvent = "coin"; NewState = "Unlocked" } ]

    let ex = Assert.Throws<ArgumentException>(fun () -> validate transitions |> ignore)

    ex.Message |> should startWith "The same event on the same state has multiple resulting states."

[<Fact>]
let validate_WhenDuplicates_Throws () =
    let transitions =
        [ { CurrentState = "Unlocked"; TriggeringEvent = "coin"; NewState = "Locked" };
          { CurrentState = "Unlocked"; TriggeringEvent = "coin"; NewState = "Locked" } ]

    let ex = Assert.Throws<ArgumentException>(fun () -> validate transitions |> ignore)

    ex.Message |> should startWith "Transition Table contains duplicates."
