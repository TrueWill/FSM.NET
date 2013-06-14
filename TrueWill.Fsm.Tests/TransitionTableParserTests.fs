module TransitionTableParserTests

open TrueWill.Fsm
open Xunit
open FsUnit.Xunit

[<Fact>]
let parse_WhenBasic_ReturnsCollection () =
    let tableText =
        "Locked|coin|Unlocked\r\n\
         Unlocked|coin|Unlocked\r\n\
         Unlocked|pass|Locked"

    let table = TransitionTableParser.parse(tableText)

    Seq.toList table |> should equal
        [ { CurrentState = "Locked";   TriggeringEvent = "coin"; NewState = "Unlocked" };
          { CurrentState = "Unlocked"; TriggeringEvent = "coin"; NewState = "Unlocked" };
          { CurrentState = "Unlocked"; TriggeringEvent = "pass"; NewState = "Locked" } ]
