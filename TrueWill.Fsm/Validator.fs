module TrueWill.Fsm.Validator

open System
open Helpers

let private validateTransition transition =
    let isPadded (s: string) = not (s.Trim().Equals(s))

    if isNull transition then
        raise <| new ArgumentNullException("Transition is null.", "transition")

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

    let numberOfTransitions = Seq.length transitions

    let distinctCount = transitions |> Seq.distinct |> Seq.length

    if numberOfTransitions <> distinctCount then
        raise <| ArgumentException("Transition Table contains duplicates.", "transitions")

    transitions |> Seq.iter validateTransition

    if transitions |> Seq.map (fun x -> x.TriggeringEvent) |> differOnlyByCase then
        raise <| ArgumentException("Some events differ only by case.", "transitions")

    if transitions |> Fsm.getStatesIncludingDuplicates |> differOnlyByCase then
        raise <| ArgumentException("Some states differ only by case.", "transitions")

    let distinctCurrentStateEventCount =
        transitions |> Seq.distinctBy (fun x -> (x.CurrentState, x.TriggeringEvent)) |> Seq.length

    if numberOfTransitions <> distinctCurrentStateEventCount then
        raise <| ArgumentException("The same event on the same state has multiple resulting states.", "transitions")
