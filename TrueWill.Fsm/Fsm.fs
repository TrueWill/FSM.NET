namespace TrueWill.Fsm

module internal Constants =
    [<Literal>]
    let Delimiter = '|'

type Transition = { CurrentState : string; TriggeringEvent : string; NewState : string }

type Transition with
    member x.AsString() =
        System.String.Format("{1}{0}{2}{0}{3}",
            Constants.Delimiter, x.CurrentState, x.TriggeringEvent, x.NewState)

module Fsm =
    let getInitialState transitions =
        (transitions |> Seq.head).CurrentState

    let getNewState currentState triggeringEvent transitions =
        let isMatch t = t.CurrentState = currentState && t.TriggeringEvent = triggeringEvent
        match transitions |> Seq.tryFind isMatch with
        | Some transition -> transition.NewState
        | None -> raise <| System.InvalidOperationException("No match found.")

    let getAvailableEvents currentState transitions =
        transitions
        |> Seq.filter (fun t -> t.CurrentState = currentState)
        |> Seq.map (fun t -> t.TriggeringEvent)

    let getStates transitions =
        transitions
        |> Seq.collect (fun t -> seq [t.CurrentState; t.NewState])
        |> Seq.distinct
