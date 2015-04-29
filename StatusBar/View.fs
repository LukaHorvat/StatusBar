module View

open System.Windows.Forms
open System.Drawing
open System.Timers
open System.Runtime.InteropServices
open System
open WinAPI
open Config
open Data

type StatusBarForm () =
    inherit Form()
    member private x.tabHotkeyId = 1
    override x.CreateParams 
        with get () =
            let p = base.CreateParams
            p.ExStyle <- p.ExStyle ||| 0x80
            p

type View = { form   : Form
              ticker : FlowLayoutPanel
              status : FlowLayoutPanel
              style  : Style
              update : IEvent<unit> 
              uiCtx  : System.Threading.SynchronizationContext
              mutable shown : bool}              

let initialize (style : Style) =
    let width = Screen.PrimaryScreen.WorkingArea.Width
    let tickerWidth = float width * style.tickerRatio |> int
    let statusWidth = width - tickerWidth
    let form = new StatusBarForm( FormBorderStyle = FormBorderStyle.None
                                , BackColor       = style.color
                                , TopMost         = true
                                , ShowInTaskbar   = false )
    WinAPI.SetWindowPos( form.Handle
                       , nativeint SpecialWindowHandles.HWND_TOP
                       , 0, 0
                       , width, style.height
                       , SetWindowPosFlags.SWP_SHOWWINDOW ) |> ignore
    let ticker = new FlowLayoutPanel( Location     = Point(0, 0)
                                    , Size         = Size(tickerWidth, style.height)
                                    , WrapContents = false )
    let status = new FlowLayoutPanel( Location     = Point(width - statusWidth, 0)
                                    , Size         = Size(statusWidth, style.height)
                                    , Anchor       = AnchorStyles.Right
                                    , RightToLeft  = RightToLeft.Yes
                                    , WrapContents = false )
    let updateEvent = Event<_>()
    let timer = new Timer( Interval = 1000.0
                         , AutoReset = true )
    timer.Elapsed.AddHandler( fun _ _ -> 
        form.Invoke (Func<unit>( fun () -> 
            updateEvent.Trigger() ) ) 
            |> ignore )
    timer.Start()
    status.Text <- "STATUS"
    ticker.Text <- "TICKER"
    form.Controls.Add(ticker)
    form.Controls.Add(status)   
    { form   = form
      ticker = ticker
      status = status
      style  = style
      update = updateEvent.Publish
      shown  = true
      uiCtx  = System.Threading.SynchronizationContext.Current }

let inFormThread fn (view : View) = Action(fn) |> view.form.Invoke |> ignore

let show (view : View) = 
    view |> inFormThread( fun () -> WinAPI.showInactiveTopmost view.form )
    view.shown <- true

let hide (view : View) = 
    view |> inFormThread view.form.Hide
    view.shown <- false

let addTick (Tick (msg, priority)) (view : View) =
    view |> inFormThread( fun () ->
        let controls = view.ticker.Controls
        let tick = new Label( Text      = msg
                            , ForeColor = Color.White
                            , Font      = if priority = Priority.High then new Font(view.style.font, FontStyle.Bold) else view.style.font
                            , AutoSize  = true
                            , Margin    = Padding.Empty
                            , Name      = msg )
        controls.Add tick 
        controls.SetChildIndex(tick, 0)
        let sep = new Label( Text      = "|"
                           , ForeColor = Color.White
                           , Font      = view.style.font
                           , AutoSize  = true
                           , Margin    = Padding.Empty )
        controls.Add sep 
        controls.SetChildIndex(sep, 0) )

let addStatus str (view : View) =
    let controls = view.status.Controls
    let label = new Label( Text      = str 
                         , ForeColor = Color.White
                         , Font      = view.style.font
                         , AutoSize  = true
                         , Margin    = Padding.Empty
                         , Name      = str )
    controls.Add label
    let sep = new Label( Text      = "|" 
                       , ForeColor = Color.White
                       , Font      = view.style.font
                       , AutoSize  = true
                       , Margin    = Padding.Empty )
    controls.Add sep
    label