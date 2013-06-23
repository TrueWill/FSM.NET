module TrueWill.Fsm.Parser

open System
open System.Text.RegularExpressions

let private regexComment = new Regex("#.*")
let private regexBlankLine = new Regex(@"^\s*$")
let private regexLineBreak = new Regex(@"\r?\n")
// TODO Not using constant for delimiter - need to escape
let private regexTransition = new Regex(@"^(?<currentState>\w+)\|(?<triggeringEvent>\w+)\|(?<newState>\w+)\s*$")

let private removeComment line =
    regexComment.Replace(line, String.Empty)

let private isBlank line =
    regexBlankLine.IsMatch(line)

/// Determines if a sequence (which may contain duplicates) contains
/// any elements that differ only by case.
/// Exposed primarily for unit tests.
let differOnlyByCase (items : seq<string>) =
    let distinctCount = items |> Seq.distinct |> Seq.length
    let distinctUpperCount = items |> Seq.distinctBy (fun x -> x.ToUpperInvariant()) |> Seq.length
    distinctCount <> distinctUpperCount

// TODO Allow whitespace (so can line up delimiters if desired)
// TODO Disallow blank elements
// TODO Cleanup
let parse tableText =
    let lines = regexLineBreak.Split(tableText)

    let result =
        lines
        |> Seq.map removeComment
        |> Seq.mapi (fun linenumber line -> (linenumber, line))
        |> Seq.filter (not << isBlank << snd)
        |> Seq.map
            (fun (lineNumber, line) ->
            let mtch = regexTransition.Match(line)
            if not mtch.Success then
                raise <| ArgumentException(sprintf "Invalid number of elements on line %d." (lineNumber + 1), "tableText")
            { CurrentState = mtch.Groups.["currentState"].Value; TriggeringEvent = mtch.Groups.["triggeringEvent"].Value; NewState = mtch.Groups.["newState"].Value } )
        |> Seq.toList

    if result |> Seq.map (fun x -> x.TriggeringEvent) |> differOnlyByCase then
        raise <| ArgumentException("Some events differ only by case.", "tableText")

    if result |> Seq.collect (fun t -> seq [t.CurrentState; t.NewState]) |> differOnlyByCase then
        raise <| ArgumentException("Some states differ only by case.", "tableText")

    result |> Seq.ofList
