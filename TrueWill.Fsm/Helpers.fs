/// Helper functions.
module TrueWill.Fsm.Helpers

open System

// http://stackoverflow.com/a/10746757/161457
let internal isNull x = obj.ReferenceEquals(x, Unchecked.defaultof<_>)

let internal checkNotNull paramValue paramName =
    if isNull paramValue then
        raise <| new ArgumentNullException(paramName)

/// Determines if a sequence (which may contain duplicates) contains
/// any elements that differ only by case.
/// Exposed primarily for unit tests.
let differOnlyByCase (items : seq<string>) =
    let distinctCount = items |> Seq.distinct |> Seq.length
    let distinctUpperCount = items |> Seq.distinctBy (fun x -> x.ToUpperInvariant()) |> Seq.length
    distinctCount <> distinctUpperCount
