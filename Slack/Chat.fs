module Chat

open System
open System.Collections.Generic
open System.Timers
open IrcFS

let _evt = Event<_>()
let evt  = _evt.Publish

let mutable lastClient = None : Net.IrcClient option

let mutable user = "<user name>"
let mutable fullName = "<full name>"
let mutable server = "<server>"
let mutable port = 6697
let mutable password = "<password>"
let mutable configured = false

let ConfigurationSchema =
    let dict = Dictionary()
    ["user", user; "full-name", fullName; "server", server; "port", port.ToString(); "password", password]
    |> Seq.iter dict.Add
    dict

let reconnect () =
    if configured then
        if lastClient.IsSome && not (lastClient.Value.Connected) then lastClient.Value.StopEvent()
        if lastClient.IsNone || not (lastClient.Value.Connected) then 
            let client = new Net.IrcClient (server, port, true)
            lastClient <- Some client
            client.WriteMessage (IrcMessage.nick user)
            client.WriteMessage (IrcMessage.user user "0" fullName)
            client.WriteMessage (IrcMessage.pass password)

            client.MessageReceived.Add (fun m -> printfn "%A" m)
            client.MessageReceived
            |> Event.filter (fun e -> e.Command = "PRIVMSG")
            |> Event.add (fun m ->
                match m.Prefix with
                | Some (User (name, _, _)) -> 
                    match m.Arguments with
                    | [ ch; msg] -> if msg.StartsWith("[" + ch + "]") |> not then _evt.Trigger(sprintf "[%s %s] %s" ch name msg, 0)
                    | _          -> printfn "Bad message"
                | _  -> printfn "Bad message" )
            client.StartEvent()


let AddTickHandler (act : Action<string, int>) = evt.Add (fun (s, i) -> act.Invoke(s, i))

let timer = new Timer( Interval  = 10.0 * 1000.0 
                     , AutoReset = true )
timer.Elapsed.Add (fun _ -> reconnect ())

let Configure (dict : Dictionary<string, string>) =
    user <- dict.["user"]
    fullName <- dict.["full-name"]
    server <- dict.["server"]
    port <- dict.["port"] |> Int32.Parse
    password <- dict.["password"]
    configured <- true
    reconnect ()
    timer.Start()

[<EntryPoint>]
let main  args =
    reconnect ()
    Console.ReadKey()
    0