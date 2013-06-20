module TrueWill.Fsm.TransitionTableParser

open System.Text.RegularExpressions

let private isBlank line =
    Regex.IsMatch(line, @"^\s*$")

/// Determines if a sequence (which may contain duplicates) contains
/// any elements that differ only by case.
/// Exposed primarily for unit tests.
let differOnlyByCase (items : seq<string>) =
    let distinctCount = items |> Seq.distinct |> Seq.length
    let distinctUpperCount = items |> Seq.distinctBy (fun x -> x.ToUpperInvariant()) |> Seq.length
    distinctCount <> distinctUpperCount

// TODO Not using constant for delimiter - need to escape
// TODO Error checking
// TODO Allow whitespace (so can line up delimiters if desired)
// TODO Disallow blank elements
// TODO Allow comments - what about end-of-line ones?
// TODO Line numbers will be off when blank lines/comments
// TODO Cleanup
let parse tableText =
    let lines = Regex.Split(tableText, @"\r?\n")

    let result =
        lines
        |> Seq.filter (not << isBlank)
        |> Seq.mapi
            (fun lineNumber line ->
            let mtch = Regex.Match(line, "^(?<currentState>\w+)\|(?<triggeringEvent>\w+)\|(?<newState>\w+)$")
            if not mtch.Success then
                raise <| System.ArgumentException(sprintf "Invalid number of elements on line %d." (lineNumber + 1), "tableText")
            { CurrentState = mtch.Groups.["currentState"].Value; TriggeringEvent = mtch.Groups.["triggeringEvent"].Value; NewState = mtch.Groups.["newState"].Value } )
        |> Seq.toList

    if result |> Seq.map (fun x -> x.TriggeringEvent) |> differOnlyByCase then
        raise <| System.ArgumentException("Some events differ only by case.", "tableText")

    result
