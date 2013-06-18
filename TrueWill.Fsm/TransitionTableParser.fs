module TrueWill.Fsm.TransitionTableParser

open System.Text.RegularExpressions

let private isBlank line =
    Regex.IsMatch(line, @"^\s*$")

// TODO Not using constant for delimiter - need to escape
// TODO Error checking
// TODO Allow whitespace (so can line up delimiters if desired)
// TODO Disallow blank elements
// TODO Allow comments - what about end-of-line ones?
// TODO Cleanup
let parse tableText =
    let lines = Regex.Split(tableText, @"\r?\n")
    lines
        |> Seq.filter (not << isBlank)
        |> Seq.map
            (fun line ->
            let mtch = Regex.Match(line, "^(?<currentState>\w+)\|(?<triggeringEvent>\w+)\|(?<newState>\w+)$")
            { CurrentState = mtch.Groups.["currentState"].Value; TriggeringEvent = mtch.Groups.["triggeringEvent"].Value; NewState = mtch.Groups.["newState"].Value } )
