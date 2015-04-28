module Ticker

open Data
open System
open System.IO
open System.Reflection
open Common

let private _onTick = Event<_>()
let onTick = _onTick.Publish 

let private loadAssembly path =
    try let asm = Assembly.LoadFrom path
        asm.GetTypes() 
            |> Array.choose( fun typ -> 
                let mtOpt = typ.GetMethod("AddTickHandler", BindingFlags.Public ||| BindingFlags.Static) |> asOption
                mtOpt |> Option.bind( fun mt ->
                    let ps = mt.GetParameters()
                    if mt.ReturnType = typeof<System.Void> && ps.Length = 1 && ps.[0].ParameterType = typeof<Action<string, int>> then
                        Some (mt.CreateDelegate(typeof<Action<Action<string, int>>>) :?> Action<Action<string, int>>)
                    else None ) )
            |> Array.iter( fun addHandler ->
                addHandler.Invoke( fun msg level -> 
                    _onTick.Trigger( Tick(msg, enum level) ) ) )
    with ex -> printfn "%A" ex

let loadPlugins path =
    if Directory.Exists path then
        Directory.GetFiles path |> Array.iter loadAssembly