module HelpersTests

open System
open TrueWill.Fsm.Helpers
open Xunit
open FsUnit.Xunit

[<Fact>]
let differOnlyByCase_WhenEmpty_ReturnsFalse () =
    let result = differOnlyByCase []
    result |> should be False

[<Fact>]
let differOnlyByCase_WhenOne_ReturnsFalse () =
    let result = differOnlyByCase [ "foo" ]
    result |> should be False

[<Fact>]
let differOnlyByCase_WhenDuplicates_ReturnsFalse () =
    let result = differOnlyByCase [ "foo"; "foo" ]
    result |> should be False

[<Fact>]
let differOnlyByCase_WhenDistinct_ReturnsFalse () =
    let result = differOnlyByCase [ "foo"; "bar" ]
    result |> should be False

[<Fact>]
let differOnlyByCase_WhenDiffer_ReturnsTrue () =
    let result = differOnlyByCase [ "foo"; "FoO" ]
    result |> should be True

[<Fact>]
let differOnlyByCase_WhenDifferAndDuplicates_ReturnsTrue () =
    let result = differOnlyByCase [ "foo"; "bar"; "Foo"; "Bar" ]
    result |> should be True
