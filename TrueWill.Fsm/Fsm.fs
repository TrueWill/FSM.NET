namespace TrueWill.Fsm

module internal Constants =
    [<Literal>]
    let Delimiter = '|'

type Transition =
    { CurrentState : string; TriggeringEvent : string; NewState : string }
    override x.ToString() =
        System.String.Format("{1}{0}{2}{0}{3}",
            Constants.Delimiter, x.CurrentState, x.TriggeringEvent, x.NewState)

module Fsm =
    let private validateState transitions state =
        let found = transitions |> Seq.exists (fun t -> state = t.CurrentState || state = t.NewState)
        if not found then
            raise <| System.InvalidOperationException(sprintf "Invalid state: '%s'." state)

    let internal getStatesIncludingDuplicates transitions =
        transitions
        |> Seq.collect (fun t -> seq [t.CurrentState; t.NewState])

    let getInitialState transitions =
        (transitions |> Seq.head).CurrentState

    let getNewState transitions currentState triggeringEvent =
        let isMatch t = t.CurrentState = currentState && t.TriggeringEvent = triggeringEvent
        match transitions |> Seq.tryFind isMatch with
        | Some transition -> transition.NewState
        | None -> raise <| System.InvalidOperationException(sprintf "Invalid state transition: state '%s', event '%s'." currentState triggeringEvent)

    let getAvailableEvents transitions currentState =
        validateState transitions currentState
        transitions
        |> Seq.filter (fun t -> t.CurrentState = currentState)
        |> Seq.map (fun t -> t.TriggeringEvent)

    let getStates transitions =
        transitions
        |> getStatesIncludingDuplicates
        |> Seq.distinct
