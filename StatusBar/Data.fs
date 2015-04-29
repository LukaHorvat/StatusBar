module Data

open System.Threading.Tasks

type Priority = High = 10 | Low = 0
type Tick = Tick of string * Priority
type Status = { initial : string
                update  : unit -> Task<string> }