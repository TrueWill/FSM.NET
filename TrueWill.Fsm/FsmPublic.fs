namespace TrueWill.Fsm

open System
open System.Reflection
open Fsm
open Helpers

[<assembly:AssemblyVersion("2.0.1.0")>]
[<assembly:AssemblyFileVersion("2.0.1.0")>]
[<assembly:CLSCompliant(true)>]
[<assembly:AssemblyTitle("FSM.NET")>]
[<assembly:AssemblyDescription("A simple 'stateless' finite-state machine library for .NET.")>]
[<assembly:AssemblyCopyright("Copyright © 2013 William E. Sorensen and contributors")>]
do
    ()

type StateMachine(transitions) =
    let transitions = transitions |> Seq.toList

    do
        Validator.validate transitions

    /// Gets the initial state.
    member x.InitialState = getInitialState transitions

    /// Gets the collection of available states.
    member x.States = getStates transitions

    /// Gets the new state.
    /// Throws InvalidOperationException if not found.
    member x.GetNewState(currentState, triggeringEvent) =
        checkNotNull currentState "currentState"
        checkNotNull triggeringEvent "triggeringEvent"
        getNewState transitions currentState triggeringEvent

    /// Gets the collection of available events for the given state.
    /// Throws InvalidOperationException if currentState is not a valid state.
    member x.GetAvailableEvents(currentState) =
        checkNotNull currentState "currentState"
        getAvailableEvents transitions currentState

[<AbstractClass; Sealed>]  // class is static
type TransitionTableParser =
    static member Parse(tableText) = Parser.parse tableText
