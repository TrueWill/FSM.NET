module TransitionTableParserTests

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

    let table = TransitionTableParser.parse(tableText)

    Seq.toList table |> should equal
        [ { CurrentState = "Locked";   TriggeringEvent = "coin"; NewState = "Unlocked" };
          { CurrentState = "Unlocked"; TriggeringEvent = "coin"; NewState = "Unlocked" };
          { CurrentState = "Unlocked"; TriggeringEvent = "pass"; NewState = "Locked" } ]

[<Fact>]
let parse_WhenBlankLines_Ignores () =
    let tableText =
        "\r\n\
         Locked|coin|Unlocked\r\n\
         \t  \r\n\
         Unlocked|pass|Locked\r\n\
         \r\n"

    let table = TransitionTableParser.parse(tableText)

    Seq.toList table |> should equal
        [ { CurrentState = "Locked";   TriggeringEvent = "coin"; NewState = "Unlocked" };
          { CurrentState = "Unlocked"; TriggeringEvent = "pass"; NewState = "Locked" } ]

[<Fact>]
let parse_WhenEmpty_ReturnsEmptyCollection () =
    // Validating this case is not the parser's responsibility.
    let table = TransitionTableParser.parse(String.Empty)
    table |> Seq.isEmpty |> should be True

[<Fact>]
let parse_WhenEventsDifferOnlyByCase_Throws () =
    let tableText =
        "Locked|coin|Unlocked\r\n\
         Unlocked|Coin|Unlocked\r\n\
         Unlocked|pass|Locked"

    (fun () -> TransitionTableParser.parse(tableText) |> ignore) |> should throw typeof<ArgumentException>

[<Fact>]
let parse_WhenExtraTransitionPart_Throws () =
    let tableText =
        "Locked|coin|Unlocked\r\n\
         Unlocked|coin|Unlocked\r\n\
         Unlocked|pass|Locked|fish"

    let ex = Assert.Throws<ArgumentException>(fun () -> TransitionTableParser.parse(tableText) |> ignore)

    ex.Message |> should equal "Invalid number of elements on line 3.\r\nParameter name: tableText"

// TODO: test for insufficent parts

[<Fact>]
let parse_WhenErrorAndBlankLine_ThrowsWithCorrectLineNumber () =
    let tableText =
        "Locked|coin|Unlocked\r\n\
         \r\n\
         Unlocked|coin|Unlocked\r\n\
         Unlocked|pass|Locked|fish"

    let ex = Assert.Throws<ArgumentException>(fun () -> TransitionTableParser.parse(tableText) |> ignore)

    ex.Message |> should equal "Invalid number of elements on line 4.\r\nParameter name: tableText"

[<Fact>]
let differOnlyByCase_WhenEmpty_ReturnsFalse () =
    let result = TransitionTableParser.differOnlyByCase []
    result |> should be False

[<Fact>]
let differOnlyByCase_WhenOne_ReturnsFalse () =
    let result = TransitionTableParser.differOnlyByCase [ "foo" ]
    result |> should be False

[<Fact>]
let differOnlyByCase_WhenDuplicates_ReturnsFalse () =
    let result = TransitionTableParser.differOnlyByCase [ "foo"; "foo" ]
    result |> should be False

[<Fact>]
let differOnlyByCase_WhenDistinct_ReturnsFalse () =
    let result = TransitionTableParser.differOnlyByCase [ "foo"; "bar" ]
    result |> should be False

[<Fact>]
let differOnlyByCase_WhenDiffer_ReturnsTrue () =
    let result = TransitionTableParser.differOnlyByCase [ "foo"; "FoO" ]
    result |> should be True

[<Fact>]
let differOnlyByCase_WhenDifferAndDuplicates_ReturnsTrue () =
    let result = TransitionTableParser.differOnlyByCase [ "foo"; "bar"; "Foo"; "Bar" ]
    result |> should be True
