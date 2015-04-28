module Presenter

open System.Collections.Generic
open System.Windows.Forms
open View
open Data
open System.Timers

type Presenter = { view        : View
                   tickerQueue : Tick Queue
                   statusList  : (Status * Label) List
                   mutable hideDelay : int }

let mutable updateStatus = false

let fromView (view : View) =
    WinAPI.keyDown |> Event.add( fun key ->
        if key = Keys.Tab then 
            view.form.Show()
            updateStatus <- true )
    WinAPI.keyUp   |> Event.add( fun key ->
        if key = Keys.Tab then 
            view.form.Hide() 
            updateStatus <- false ) 
    let statusList = List() 
    view.update.Add( fun () ->
        statusList.ForEach( fun (stat : Status, lbl : Label) -> lbl.Text <- stat.update() ) )
    { view = view; tickerQueue = Queue(); statusList = statusList; hideDelay = 0 }

let mutable delayToHide = 0

let showFor ms (pres : Presenter) =
    show pres.view
    if pres.hideDelay = 0 then
        let timer = new Timer( Interval  = 100.0
                             , AutoReset = true )
        let handler _ =
            pres.hideDelay <- pres.hideDelay - 100
            //printfn "%A" pres.hideDelay
            if pres.hideDelay <= 0 then
                pres.hideDelay <- 0
                hide pres.view
                timer.Stop()
        timer.Elapsed.Add handler
        timer.Start() 
    pres.hideDelay <- ms
            
let tick tick (pres : Presenter) =
    pres.tickerQueue.Enqueue tick
    View.addTick tick pres.view
    showFor 5000 pres