# StatusBar
A desktop status bar.

[Here's an example video](https://dl.dropboxusercontent.com/u/35032740/ShareX/2015/04/2015-04-28_19-31-02.webm)

The plugin system is very managable. You just write a .NET assembly in anything you want and compile it to a DLL. The system looks for static methods that look like `void AddTickHandler(Action<string, int>)`. The plugin should implement this method so that it calls the given action with the new message and the priority level when some event happens.
Here's what an example plugin looks like. It just fires a "Test" notification every 7 seconds. (written in F#, but could be anything that compiles to .NET assemblies)
```
module Spammer

open System.Timers

let timer = new Timer( Interval  = 7000.0
                     , AutoReset = true )
timer.Start()

let AddTickHandler (fn : System.Action<string, int>) = timer.Elapsed.Add( fun _ -> fn.Invoke("Test", 0) )
```

Every time a new tick is fired, the status bar becomes visible for 5 seconds and then hides again.
This is the ticker part of it.

The second part is the status. Your plugins can register a status that's always visible in the status bar (when the status bar itself is visible) and a method to update that status. This method will be called periodically when the status bar is visible.
It can be used to track the progress of a download for example.
