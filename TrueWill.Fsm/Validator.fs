module TrueWill.Fsm.Validator

open System

// TODO: Probably need internal - used elsewhere
let private isNull x = obj.ReferenceEquals(x, Unchecked.defaultof<_>)  // http://stackoverflow.com/a/10746757/161457

let private validateTransition transition =
    let isPadded (s: string) = not (s.Trim().Equals(s))

    if String.IsNullOrWhiteSpace transition.CurrentState
        || String.IsNullOrWhiteSpace transition.NewState then
        raise <| ArgumentException("Transition contains a null or blank state.", "transition")

    if String.IsNullOrWhiteSpace transition.TriggeringEvent then
        raise <| ArgumentException("Transition contains a null or blank event.", "transition")

    if isPadded transition.CurrentState
        || isPadded transition.NewState then
        raise <| ArgumentException("States may not contain leading or trailing whitespace.", "transition")

    if isPadded transition.TriggeringEvent then
        raise <| ArgumentException("Events may not contain leading or trailing whitespace.", "transition")

let validate transitions =

    if transitions |> Seq.isEmpty then
        raise <| new ArgumentException("Transition Table is empty.", "transitions")

    if transitions |> Seq.exists isNull then
        raise <| new ArgumentException("A transition is null.", "transitions")

    transitions |> Seq.iter validateTransition

    if transitions |> Seq.map (fun x -> x.TriggeringEvent) |> Parser.differOnlyByCase then
        raise <| ArgumentException("Some events differ only by case.", "transitions")

    // TODO: Move differ function and its tests around - also see if can reuse collect function
    if transitions |> Seq.collect (fun t -> seq [t.CurrentState; t.NewState]) |> Parser.differOnlyByCase then
        raise <| ArgumentException("Some states differ only by case.", "transitions")
