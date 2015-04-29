module Plugin

open Data
open System
open System.IO
open System.Reflection
open System.Collections.Generic
open System.Threading.Tasks
open Common

let private _onTick = Event<_>()
let onTick = _onTick.Publish
let statuses = List<_>()

let private methodMatchType (ret : Type) (paramTypes : Type []) (met : MethodInfo)  =
    let ps = met.GetParameters() |> Array.map (fun p -> p.ParameterType)
    met.ReturnType = ret && (paramTypes |> Array.zip ps |> Array.forall (fun (x, y) -> x = y))

let private makeDelegate (met : MethodInfo) : 'actionOrFunc option =
    let typ     = typeof<'actionOrFunc>
    let genType = typ.GetGenericTypeDefinition()
    let mutable ret = null
    let mutable ps  = null
    if genType = typedefof<Action<_>> then
        ret <- typeof<Void>
        ps  <- typ.GetGenericArguments()
    else if genType = typedefof<Func<_>> then
        let genericArgs = typ.GetGenericArguments()
        ret <- Array.last genericArgs
        ps  <- Array.sub genericArgs 0 (genericArgs.Length - 1)
    if List.contains genType [typedefof<Action<_>>; typedefof<Func<_>>] then
        if methodMatchType ret ps met then
            Some (met.CreateDelegate typ :?> 'actionOrFunc)
        else
            printfn "The method %A doesn't match the type %A" met typ 
            None
    else
        printfn "The provided type parameter isn't an Action<> or a Func<>"
        None

let private makeValue (fld : PropertyInfo) : 'a option =
    if fld.PropertyType = typeof<'a> then Some (fld.GetValue(null) :?> 'a)
    else None

let private loadTicker (types : Type []) =
    types 
    |> Array.choose( fun typ -> 
        typ.GetMethod("AddTickHandler", BindingFlags.Public ||| BindingFlags.Static) 
        |> asOption
        |> Option.bind makeDelegate )
    |> Array.iter( fun (addHandler : Action<Action<string, int>>) ->
        addHandler.Invoke( fun msg level -> 
            _onTick.Trigger( Tick(msg, enum level) ) ) )

let private loadStatuses (types : Type []) =
    types 
    |> Array.choose( fun typ -> 
        typ.GetProperty("Status", BindingFlags.Public ||| BindingFlags.Static) 
        |> asOption
        |> Option.bind makeValue
        |> Option.bind (fun init -> 
            typ.GetMethod("UpdateStatus", BindingFlags.Public ||| BindingFlags.Static) 
            |> asOption
            |> Option.bind makeDelegate
            |> Option.map (fun (fn : Func<Task<string>>) -> { initial = init; update = fun () -> fn.Invoke() }) ) )
    |> statuses.AddRange

let private loadAssembly path =
    try let asm = Assembly.LoadFile <| Path.GetFullPath(path)
        let types = asm.GetTypes() 
        loadTicker types
        loadStatuses types
    with ex -> printfn "%A" ex

let loadPlugins path =
    AppDomain.CurrentDomain.add_AssemblyResolve(ResolveEventHandler( fun obj args -> 
        Path.Combine(path, "deps", AssemblyName(args.Name).Name + ".dll") 
        |> Path.GetFullPath
        |> Assembly.LoadFile ))
    if Directory.Exists path then
        let temp = Directory.GetFiles path |> Array.filter (fun path -> Path.GetExtension(path).ToLower() = ".dll")
        temp  |> Array.iter loadAssembly
