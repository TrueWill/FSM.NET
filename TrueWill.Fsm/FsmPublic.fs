namespace TrueWill.Fsm

open System
open Fsm

[<assembly:CLSCompliant(true)>]
do
    ()

type StateMachine(transitions) =
    let transitions = transitions |> Seq.toList

    do
        if transitions |> Seq.isEmpty then
            raise <| new ArgumentException("Transition Table is empty.", "transitions")

    /// Gets the initial state.
    member x.InitialState = getInitialState transitions

    /// Gets the collection of available states.
    member x.States = getStates transitions

    /// Gets the new state.
    /// Throws InvalidOperationException if not found.
    member x.GetNewState(currentState, triggeringEvent) =
        getNewState currentState triggeringEvent transitions

    /// Gets the collection of available events for the given state.
    /// Throws InvalidOperationException if currentState is not a valid state.
    member x.GetAvailableEvents(currentState) =
        getAvailableEvents currentState transitions
