module TrueWill.Fsm.TransitionTableParser

open System.Text.RegularExpressions

// TODO Not using constant for delimiter - need to escape
// TODO Error checking
// TODO Allow whitespace (so can line up delimiters if desired)
// TODO Allow comments and blank lines - maybe just filter those lines?
// TODO Cleanup
let parse tableText =
    let lines = Regex.Split(tableText, @"\r?\n")
    lines |> Seq.map
        (fun line ->
           let mtch = Regex.Match(line, "^(?<currentState>\w+)\|(?<triggeringEvent>\w+)\|(?<newState>\w+)$")
           { CurrentState = mtch.Groups.["currentState"].Value; TriggeringEvent = mtch.Groups.["triggeringEvent"].Value; NewState = mtch.Groups.["newState"].Value } )
