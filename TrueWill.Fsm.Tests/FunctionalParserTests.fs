module FunctionalParserTests

open System
open TrueWill.Fsm
open Xunit
open FsUnit.Xunit

[<Fact>]
let parse_WhenBasic_ReturnsCollection () =
    let tableText =
        "Locked|coin|Unlocked\r\n\
         Unlocked|coin|Unlocked\r\n\
         Unlocked|pass|Locked"

    let table = Parser.parse(tableText)

    Seq.toList table |> should equal
        [ { CurrentState = "Locked";   TriggeringEvent = "coin"; NewState = "Unlocked" };
          { CurrentState = "Unlocked"; TriggeringEvent = "coin"; NewState = "Unlocked" };
          { CurrentState = "Unlocked"; TriggeringEvent = "pass"; NewState = "Locked" } ]

[<Fact>]
let parse_WhenBlankEvent_Throws () =
    let tableText =
        "Locked| |Unlocked\r\n\
         Unlocked|coin|Unlocked\r\n\
         Unlocked|pass|Locked"

    (fun () -> Parser.parse(tableText) |> ignore) |> should throw typeof<ArgumentException>

[<Fact>]
let parse_WhenBlankState_Throws () =
    let tableText =
        "Locked|coin|Unlocked\r\n\
         |coin|Unlocked\r\n\
         Unlocked|pass|Locked"

    (fun () -> Parser.parse(tableText) |> ignore) |> should throw typeof<ArgumentException>

[<Fact>]
let parse_WhenBlankLines_Ignores () =
    let tableText =
        "\r\n\
         Locked|coin|Unlocked\r\n\
         \t  \r\n\
         Unlocked|pass|Locked\r\n\
         \r\n"

    let table = Parser.parse(tableText)

    Seq.toList table |> should equal
        [ { CurrentState = "Locked";   TriggeringEvent = "coin"; NewState = "Unlocked" };
          { CurrentState = "Unlocked"; TriggeringEvent = "pass"; NewState = "Locked" } ]

[<Fact>]
let parse_WhenCommentAtEndOfLine_Ignores () =
    let tableText =
        "Locked|coin|Unlocked # This is an end-of-line comment\r\n\
         Unlocked|pass|Locked"

    let table = Parser.parse(tableText)

    Seq.toList table |> should equal
        [ { CurrentState = "Locked";   TriggeringEvent = "coin"; NewState = "Unlocked" };
          { CurrentState = "Unlocked"; TriggeringEvent = "pass"; NewState = "Locked" } ]

[<Fact>]
let parse_WhenCommentLines_Ignores () =
    let tableText =
        "# This is a comment\r\n\
         Locked|coin|Unlocked\r\n\
         # Another comment\r\n\
         Unlocked|pass|Locked"

    let table = Parser.parse(tableText)

    Seq.toList table |> should equal
        [ { CurrentState = "Locked";   TriggeringEvent = "coin"; NewState = "Unlocked" };
          { CurrentState = "Unlocked"; TriggeringEvent = "pass"; NewState = "Locked" } ]

[<Fact>]
let parse_WhenElementsPadded_Trims () =
    let tableText =
        " Locked   | coin | Unlocked \r\n" +
        " Unlocked | coin | Unlocked \r\n" +
        " Unlocked | pass | Locked   \r\n"

    let table = Parser.parse(tableText)

    Seq.toList table |> should equal
        [ { CurrentState = "Locked";   TriggeringEvent = "coin"; NewState = "Unlocked" };
          { CurrentState = "Unlocked"; TriggeringEvent = "coin"; NewState = "Unlocked" };
          { CurrentState = "Unlocked"; TriggeringEvent = "pass"; NewState = "Locked" } ]

[<Fact>]
let parse_WhenEmpty_ReturnsEmptyCollection () =
    // Validating this case is not the parser's responsibility.
    let table = Parser.parse(String.Empty)
    table |> Seq.isEmpty |> should be True

[<Fact>]
let parse_WhenExtraTransitionPart_Throws () =
    let tableText =
        "Locked|coin|Unlocked\r\n\
         Unlocked|coin|Unlocked\r\n\
         Unlocked|pass|Locked|fish"

    let ex = Assert.Throws<ArgumentException>(fun () -> Parser.parse(tableText) |> ignore)

    ex.Message |> should equal "Invalid number of elements on line 3.\r\nParameter name: tableText"

[<Fact>]
let parse_WhenErrorAndBlankLine_ThrowsWithCorrectLineNumber () =
    let tableText =
        "Locked|coin|Unlocked\r\n\
         \r\n\
         Unlocked|coin|Unlocked\r\n\
         Unlocked|pass|Locked|fish"

    let ex = Assert.Throws<ArgumentException>(fun () -> Parser.parse(tableText) |> ignore)

    ex.Message |> should equal "Invalid number of elements on line 4.\r\nParameter name: tableText"

[<Fact>]
let parse_WhenLinuxNewLines_ReturnsCollection () =
    let tableText =
        "Locked|coin|Unlocked\n\
         Unlocked|coin|Unlocked\n\
         Unlocked|pass|Locked\n"

    let table = Parser.parse(tableText)

    Seq.toList table |> should equal
        [ { CurrentState = "Locked";   TriggeringEvent = "coin"; NewState = "Unlocked" };
          { CurrentState = "Unlocked"; TriggeringEvent = "coin"; NewState = "Unlocked" };
          { CurrentState = "Unlocked"; TriggeringEvent = "pass"; NewState = "Locked" } ]

[<Fact>]
let parse_WhenMissingTransitionPart_Throws () =
    let tableText =
        "Locked|coin|Unlocked\r\n\
         Unlocked|Unlocked\r\n\
         Unlocked|pass|Locked"

    let ex = Assert.Throws<ArgumentException>(fun () -> Parser.parse(tableText) |> ignore)

    ex.Message |> should equal "Invalid number of elements on line 2.\r\nParameter name: tableText"

[<Fact>]
let parse_WhenOneTransition_ReturnsCollection () =
    let tableText = "Locked|coin|Unlocked"

    let table = Parser.parse(tableText)

    Seq.toList table |> should equal
        [ { CurrentState = "Locked";   TriggeringEvent = "coin"; NewState = "Unlocked" } ]

[<Fact>]
let parse_WhenEmbeddedSpaces_Preserves () =
    let tableText = " Order Entry | order is entered | Order Verification "

    let table = Parser.parse(tableText)

    Seq.toList table |> should equal
        [ { CurrentState = "Order Entry"; TriggeringEvent = "order is entered"; NewState = "Order Verification" } ]

[<Fact>]
let parse_WhenStatesAndEventsOfLengthOne_ReturnsCollection () =
    let tableText = "A|b|C"

    let table = Parser.parse(tableText)

    Seq.toList table |> should equal
        [ { CurrentState = "A"; TriggeringEvent = "b"; NewState = "C" } ]
