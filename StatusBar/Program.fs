open System.Windows.Forms
open System.Runtime.InteropServices
open Data
open System.Threading

[<DllImport("kernel32.dll", SetLastError=true, ExactSpelling=true)>]
extern bool FreeConsole();

[<EntryPoint>]
let main argv = 
    //FreeConsole() |> ignore
    use hook = new WinAPI.Hook()
    let view = View.initialize Config.defaultStyle
    let presenter = Presenter.fromView view
    Plugin.loadPlugins "plugins"
    Plugin.onTick.Add( fun tick -> Presenter.tick tick presenter )
    Plugin.statuses.ForEach(fun st -> Presenter.addStatus st presenter)
    Application.Run(view.form)
    0
