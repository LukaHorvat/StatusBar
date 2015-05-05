module Config

open System.Drawing
open System.IO
open System.Text.RegularExpressions
open System.Collections.Generic
open System.Windows.Forms
open System
open System.Diagnostics
open FSharp.Data


type Style = { color       : Color
               font        : Font
               height      : int
               tickerRatio : double
               textColor   : Color }

let defaultStyle = { color       = Color.Gray
                     font        = new Font("Segoe UI", 12.0f)
                     height      = 20
                     tickerRatio = 0.7 
                     textColor   = Color.White }

let loadConfig name =
    let path = Path.Combine("config", name + ".json")
    if Directory.Exists "config" && File.Exists path then
        let dict = Dictionary()
        match JsonValue.Load(path) with
        | JsonValue.Record x -> x |> Seq.iter (fun (k, v) -> dict.Add(k, v.AsString()))
        | _                  -> printfn "Bad config format in %s" name
        Some dict
    else None

let askCfg (dict : Dictionary<string, string>) name =
    let path = Path.Combine("config", name + ".json")
    if Directory.Exists "config" |> not then Directory.CreateDirectory "config" |> ignore
    let json = dict
               |> Seq.map (fun kvp -> kvp.Key, JsonValue.String kvp.Value)
               |> Seq.toArray
               |> JsonValue.Record
    let writer = new StreamWriter(File.OpenWrite(path))
    json.WriteTo(writer, JsonSaveOptions.None)
    writer.Dispose()
    Process.Start(path) |> ignore
    MessageBox.Show(sprintf "Click OK when you're done editing the %s configuration file" name) |> ignore
    loadConfig name |> Option.get