module TrueWill.Fsm.Parser

open System
open System.Text.RegularExpressions
open Helpers

let private escapedDelimiter = @"\" + string Constants.Delimiter
let private regexComment = new Regex("#.*")
let private regexBlankLine = new Regex(@"^\s*$")
let private regexLineBreak = new Regex(@"\r?\n")

// A state or event may contain embedded spaces.
// It must start and end with a letter, digit, or underscore.
// It needs to match "A" and not match all of "A ".
// Using positive lookbehind to ensure this.
let private stateOrEventRegexText = @"\w[\w ]*(?<=\w)"

let private regexTransition =
    new Regex(
        @"^\s*(?<currentState>" + stateOrEventRegexText + ")\s*" + escapedDelimiter +
        "\s*(?<triggeringEvent>" + stateOrEventRegexText + ")\s*" + escapedDelimiter +
        "\s*(?<newState>" + stateOrEventRegexText + ")\s*$")

let private removeComment line =
    regexComment.Replace(line, String.Empty)

let private isBlank line =
    regexBlankLine.IsMatch(line)

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

    result |> Seq.ofList
