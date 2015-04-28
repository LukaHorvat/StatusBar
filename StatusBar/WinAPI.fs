module WinAPI

open System
open System.Runtime.InteropServices
open System.Windows.Forms
open System.Diagnostics

/// <summary>
///     Special window handles
/// </summary>
type SpecialWindowHandles =
    // ReSharper disable InconsistentNaming
    /// <summary>
    ///     Places the window at the top of the Z order.
    /// </summary>
    | HWND_TOP = 0
    /// <summary>
    ///     Places the window at the bottom of the Z order. If the hWnd parameter identifies a topmost window, the window loses its topmost status and is placed at the bottom of all other windows.
    /// </summary>
    | HWND_BOTTOM = 1
    /// <summary>
    ///     Places the window above all non-topmost windows. The window maintains its topmost position even when it is deactivated.
    /// </summary>
    | HWND_TOPMOST = -1
    /// <summary>
    ///     Places the window above all non-topmost windows (that is, behind all topmost windows). This flag has no effect if the window is already a non-topmost window.
    /// </summary>
    | HWND_NOTOPMOST = -2
    // ReSharper restore InconsistentNaming

[<Flags>]
type SetWindowPosFlags =
    // ReSharper disable InconsistentNaming

    /// <summary>
    ///     If the calling thread and the thread that owns the window are attached to different input queues, the system posts the request to the thread that owns the window. This prevents the calling thread from blocking its execution while other threads process the request.
    /// </summary>
    | SWP_ASYNCWINDOWPOS = 0x4000u

    /// <summary>
    ///     Prevents generation of the WM_SYNCPAINT message.
    /// </summary>
    | SWP_DEFERERASE = 0x2000u

    /// <summary>
    ///     Draws a frame (defined in the window's class description) around the window.
    /// </summary>
    | SWP_DRAWFRAME = 0x0020u

    /// <summary>
    ///     Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.
    /// </summary>
    | SWP_FRAMECHANGED = 0x0020u

    /// <summary>
    ///     Hides the window.
    /// </summary>
    | SWP_HIDEWINDOW = 0x0080u

    /// <summary>
    ///     Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
    /// </summary>
    | SWP_NOACTIVATE = 0x0010u

    /// <summary>
    ///     Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client area are saved and copied back into the client area after the window is sized or repositioned.
    /// </summary>
    | SWP_NOCOPYBITS = 0x0100u

    /// <summary>
    ///     Retains the current position (ignores X and Y parameters).
    /// </summary>
    | SWP_NOMOVE = 0x0002u

    /// <summary>
    ///     Does not change the owner window's position in the Z order.
    /// </summary>
    | SWP_NOOWNERZORDER = 0x0200u

    /// <summary>
    ///     Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of the window being moved. When this flag is set, the application must explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
    /// </summary>
    | SWP_NOREDRAW = 0x0008u

    /// <summary>
    ///     Same as the SWP_NOOWNERZORDER flag.
    /// </summary>
    | SWP_NOREPOSITION = 0x0200u

    /// <summary>
    ///     Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
    /// </summary>
    | SWP_NOSENDCHANGING = 0x0400u

    /// <summary>
    ///     Retains the current size (ignores the cx and cy parameters).
    /// </summary>
    | SWP_NOSIZE = 0x0001u

    /// <summary>
    ///     Retains the current Z order (ignores the hWndInsertAfter parameter).
    /// </summary>
    | SWP_NOZORDER = 0x0004u

    /// <summary>
    ///     Displays the window.
    /// </summary>
    | SWP_SHOWWINDOW = 0x0040u

[<DllImport("user32.dll", SetLastError = true)>]
extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags)

type LowLevelKeyboardProc = delegate of int * nativeint * nativeint -> nativeint

[<DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)>]
extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, UInt32 dwThreadId)

[<DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)>]
extern bool UnhookWindowsHookEx(IntPtr hhk)

[<DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)>]
extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

[<DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)>]
extern IntPtr GetModuleHandle(string lpModuleName)

let WH_KEYBOARD_LL = 13
let WM_KEYDOWN = 0x0100
let WM_KEYUP   = 0x0101
let mutable private _hookID = IntPtr.Zero;

let private _keyUp   = Event<Keys>()
let private _keyDown = Event<Keys>()
let keyUp   = _keyUp.Publish
let keyDown = _keyDown.Publish

let HookCallback nCode wParam lParam =
    if nCode >= 0 then
        let vkCode = Marshal.ReadInt32(lParam)
        if wParam = nativeint WM_KEYDOWN    then _keyDown.Trigger (enum vkCode)    
        else if wParam = nativeint WM_KEYUP then _keyUp.Trigger   (enum vkCode)
    CallNextHookEx(_hookID, nCode, wParam, lParam);

let private _proc = LowLevelKeyboardProc(HookCallback)

let private SetHook (proc : LowLevelKeyboardProc) =
    use curProcess = Process.GetCurrentProcess()
    use curModule  = curProcess.MainModule
    SetWindowsHookEx( WH_KEYBOARD_LL, proc
                    , GetModuleHandle(curModule.ModuleName), 0u )

type Hook () =
    do _hookID <- SetHook(_proc)
    interface IDisposable with
        member x.Dispose () = UnhookWindowsHookEx(_hookID) |> ignore

let private SW_SHOWNOACTIVATE = 4;
let private HWND_TOPMOST = -1;

[<DllImport("user32.dll")>]
extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

let showInactiveTopmost (frm : Form) =
     ShowWindow(frm.Handle, SW_SHOWNOACTIVATE) |> ignore
     SetWindowPos( frm.Handle, nativeint HWND_TOPMOST
                 , frm.Left, frm.Top, frm.Width, frm.Height
                 , SetWindowPosFlags.SWP_NOACTIVATE ) |> ignore