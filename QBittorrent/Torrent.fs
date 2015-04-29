module Torrent

open System.Net.Http
open System.Collections.Generic
open FSharp.Data
open System.Text
open System

type Torrent = { name    : string
                 speed   : string
                 eta     : string option
                 percent : float }

let dict = Dictionary<string, Torrent>()
let client = new HttpClient()

let port = 1234

let _evt = Event<_>()
let evt = _evt.Publish

let Status = "QBittorrent"

let UpdateStatus () = async {
    try let! torrents = client.GetStringAsync("http://localhost:" + port.ToString() + "/json/torrents") |> Async.AwaitTask
        let values = JsonValue.Parse(torrents).AsArray()
        return values 
        |> Array.choose( fun v -> 
            let name  = v.GetProperty("name").AsString()
            let speed = v.GetProperty("dlspeed").AsString()
            let eta   = v.TryGetProperty "eta" |> Option.map (fun v -> v.AsString())
            let perc  = v.GetProperty("progress").AsFloat()
            let state = v.GetProperty("state").AsString()
            let t     = { name = name; speed = speed; eta = eta; percent = perc }
            if dict.ContainsKey name then
                let t = dict.[name]
                if t.percent <> 1.0 && perc = 1.0 then _evt.Trigger(name + " finished downloading", 0)
            dict.[name] <- t
            if state = "downloading" then Some t
            else None )
        |> Array.sortBy (fun t -> t.name)
        |> Array.map (fun t -> t.name.Substring(0, 5) + ".. " + t.speed + " " + t.eta.Value + " " + (t.percent * 100.0).ToString() + "%" )
        |> String.concat " | "
    with _ -> return "" } |> Async.StartAsTask

let AddTickHandler (act : Action<string, int>) = evt.Add (fun (s, n) -> act.Invoke(s, n))
