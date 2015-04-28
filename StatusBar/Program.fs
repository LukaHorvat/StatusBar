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
    Ticker.loadPlugins "plugins"
    Ticker.onTick.Add( fun tick -> Presenter.tick tick presenter )
    let count = ref 0
    presenter.statusList.Add({ initial = "hey"; update = (fun () -> count := count.Value + 1; count.Value.ToString()) }, View.addStatus "hey" view)
    Application.Run(view.form)
    0
