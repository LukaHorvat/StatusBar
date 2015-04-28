module Spammer

open System.Timers

let timer = new Timer( Interval  = 7000.0
                     , AutoReset = true )
timer.Start()

let AddTickHandler (fn : System.Action<string, int>) = timer.Elapsed.Add( fun _ -> fn.Invoke("Test", 0) )
    