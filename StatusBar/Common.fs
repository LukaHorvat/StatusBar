[<AutoOpen>]
module Common

let asOption x = if x = null then None else Some x

module Async =
    let single x = async { return x }
    let sequence asyncs = 
        let rec sequence' todo res =
            if Seq.isEmpty todo then List.rev res |> single
            else async {
                let! r = Seq.head todo
                return! sequence' (Seq.skip 1 todo) (r :: res) }
        sequence' asyncs []