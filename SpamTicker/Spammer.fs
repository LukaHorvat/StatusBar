module Spammer

open System.Timers

let timer = new Timer( Interval  = 7000.0
                     , AutoReset = true )
timer.Start()

let AddTickHandler (fn : System.Action<string, int>) = timer.Elapsed.Add( fun _ -> fn.Invoke("Test", 0) )
    
let count = ref 0
let Status = "Test"
let UpdateStatus () = async {
    count := count.Value + 1
    return count.Value.ToString() } |> Async.StartAsTask