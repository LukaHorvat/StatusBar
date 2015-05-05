module WinAPI

open System
open System.Runtime.InteropServices
open System.Windows.Forms
open System.Diagnostics

type VK =
    ///<summary>
    ///Left mouse button
    ///</summary>
    | LBUTTON = 0x01
    ///<summary>
    ///Right mouse button
    ///</summary>
    | RBUTTON = 0x02
    ///<summary>
    ///Control-break processing
    ///</summary>
    | CANCEL = 0x03
    ///<summary>
    ///Middle mouse button (three-button mouse)
    ///</summary>
    | MBUTTON = 0x04
    ///<summary>
    ///Windows 2000/XP: X1 mouse button
    ///</summary>
    | XBUTTON1 = 0x05
    ///<summary>
    ///Windows 2000/XP: X2 mouse button
    ///</summary>
    | XBUTTON2 = 0x06
    ///<summary>
    ///BACKSPACE key
    ///</summary>
    | BACK = 0x08
    ///<summary>
    ///TAB key
    ///</summary>
    | TAB = 0x09
    ///<summary>
    ///CLEAR key
    ///</summary>
    | CLEAR = 0x0C
    ///<summary>
    ///ENTER key
    ///</summary>
    | RETURN = 0x0D
    ///<summary>
    ///SHIFT key
    ///</summary>
    | SHIFT = 0x10
    ///<summary>
    ///CTRL key
    ///</summary>
    | CONTROL = 0x11
    ///<summary>
    ///ALT key
    ///</summary>
    | MENU = 0x12
    ///<summary>
    ///PAUSE key
    ///</summary>
    | PAUSE = 0x13
    ///<summary>
    ///CAPS LOCK key
    ///</summary>
    | CAPITAL = 0x14
    ///<summary>
    ///Input Method Editor (IME) Kana mode
    ///</summary>
    | KANA = 0x15
    ///<summary>
    ///IME Hangul mode
    ///</summary>
    | HANGUL = 0x15
    ///<summary>
    ///IME Junja mode
    ///</summary>
    | JUNJA = 0x17
    ///<summary>
    ///IME final mode
    ///</summary>
    | FINAL = 0x18
    ///<summary>
    ///IME Hanja mode
    ///</summary>
    | HANJA = 0x19
    ///<summary>
    ///IME Kanji mode
    ///</summary>
    | KANJI = 0x19
    ///<summary>
    ///ESC key
    ///</summary>
    | ESCAPE = 0x1B
    ///<summary>
    ///IME convert
    ///</summary>
    | CONVERT = 0x1C
    ///<summary>
    ///IME nonconvert
    ///</summary>
    | NONCONVERT = 0x1D
    ///<summary>
    ///IME accept
    ///</summary>
    | ACCEPT = 0x1E
    ///<summary>
    ///IME mode change request
    ///</summary>
    | MODECHANGE = 0x1F
    ///<summary>
    ///SPACEBAR
    ///</summary>
    | SPACE = 0x20
    ///<summary>
    ///PAGE UP key
    ///</summary>
    | PRIOR = 0x21
    ///<summary>
    ///PAGE DOWN key
    ///</summary>
    | NEXT = 0x22
    ///<summary>
    ///END key
    ///</summary>
    | END = 0x23
    ///<summary>
    ///HOME key
    ///</summary>
    | HOME = 0x24
    ///<summary>
    ///LEFT ARROW key
    ///</summary>
    | LEFT = 0x25
    ///<summary>
    ///UP ARROW key
    ///</summary>
    | UP = 0x26
    ///<summary>
    ///RIGHT ARROW key
    ///</summary>
    | RIGHT = 0x27
    ///<summary>
    ///DOWN ARROW key
    ///</summary>
    | DOWN = 0x28
    ///<summary>
    ///SELECT key
    ///</summary>
    | SELECT = 0x29
    ///<summary>
    ///PRINT key
    ///</summary>
    | PRINT = 0x2A
    ///<summary>
    ///EXECUTE key
    ///</summary>
    | EXECUTE = 0x2B
    ///<summary>
    ///PRINT SCREEN key
    ///</summary>
    | SNAPSHOT = 0x2C
    ///<summary>
    ///INS key
    ///</summary>
    | INSERT = 0x2D
    ///<summary>
    ///DEL key
    ///</summary>
    | DELETE = 0x2E
    ///<summary>
    ///HELP key
    ///</summary>
    | HELP = 0x2F
    ///<summary>
    ///0 key
    ///</summary>
    | KEY_0 = 0x30
    ///<summary>
    ///1 key
    ///</summary>
    | KEY_1 = 0x31
    ///<summary>
    ///2 key
    ///</summary>
    | KEY_2 = 0x32
    ///<summary>
    ///3 key
    ///</summary>
    | KEY_3 = 0x33
    ///<summary>
    ///4 key
    ///</summary>
    | KEY_4 = 0x34
    ///<summary>
    ///5 key
    ///</summary>
    | KEY_5 = 0x35
    ///<summary>
    ///6 key
    ///</summary>
    | KEY_6 = 0x36
    ///<summary>
    ///7 key
    ///</summary>
    | KEY_7 = 0x37
    ///<summary>
    ///8 key
    ///</summary>
    | KEY_8 = 0x38
    ///<summary>
    ///9 key
    ///</summary>
    | KEY_9 = 0x39
    ///<summary>
    ///A key
    ///</summary>
    | KEY_A = 0x41
    ///<summary>
    ///B key
    ///</summary>
    | KEY_B = 0x42
    ///<summary>
    ///C key
    ///</summary>
    | KEY_C = 0x43
    ///<summary>
    ///D key
    ///</summary>
    | KEY_D = 0x44
    ///<summary>
    ///E key
    ///</summary>
    | KEY_E = 0x45
    ///<summary>
    ///F key
    ///</summary>
    | KEY_F = 0x46
    ///<summary>
    ///G key
    ///</summary>
    | KEY_G = 0x47
    ///<summary>
    ///H key
    ///</summary>
    | KEY_H = 0x48
    ///<summary>
    ///I key
    ///</summary>
    | KEY_I = 0x49
    ///<summary>
    ///J key
    ///</summary>
    | KEY_J = 0x4A
    ///<summary>
    ///K key
    ///</summary>
    | KEY_K = 0x4B
    ///<summary>
    ///L key
    ///</summary>
    | KEY_L = 0x4C
    ///<summary>
    ///M key
    ///</summary>
    | KEY_M = 0x4D
    ///<summary>
    ///N key
    ///</summary>
    | KEY_N = 0x4E
    ///<summary>
    ///O key
    ///</summary>
    | KEY_O = 0x4F
    ///<summary>
    ///P key
    ///</summary>
    | KEY_P = 0x50
    ///<summary>
    ///Q key
    ///</summary>
    | KEY_Q = 0x51
    ///<summary>
    ///R key
    ///</summary>
    | KEY_R = 0x52
    ///<summary>
    ///S key
    ///</summary>
    | KEY_S = 0x53
    ///<summary>
    ///T key
    ///</summary>
    | KEY_T = 0x54
    ///<summary>
    ///U key
    ///</summary>
    | KEY_U = 0x55
    ///<summary>
    ///V key
    ///</summary>
    | KEY_V = 0x56
    ///<summary>
    ///W key
    ///</summary>
    | KEY_W = 0x57
    ///<summary>
    ///X key
    ///</summary>
    | KEY_X = 0x58
    ///<summary>
    ///Y key
    ///</summary>
    | KEY_Y = 0x59
    ///<summary>
    ///Z key
    ///</summary>
    | KEY_Z = 0x5A
    ///<summary>
    ///Left Windows key (Microsoft Natural keyboard) 
    ///</summary>
    | LWIN = 0x5B
    ///<summary>
    ///Right Windows key (Natural keyboard)
    ///</summary>
    | RWIN = 0x5C
    ///<summary>
    ///Applications key (Natural keyboard)
    ///</summary>
    | APPS = 0x5D
    ///<summary>
    ///Computer Sleep key
    ///</summary>
    | SLEEP = 0x5F
    ///<summary>
    ///Numeric keypad 0 key
    ///</summary>
    | NUMPAD0 = 0x60
    ///<summary>
    ///Numeric keypad 1 key
    ///</summary>
    | NUMPAD1 = 0x61
    ///<summary>
    ///Numeric keypad 2 key
    ///</summary>
    | NUMPAD2 = 0x62
    ///<summary>
    ///Numeric keypad 3 key
    ///</summary>
    | NUMPAD3 = 0x63
    ///<summary>
    ///Numeric keypad 4 key
    ///</summary>
    | NUMPAD4 = 0x64
    ///<summary>
    ///Numeric keypad 5 key
    ///</summary>
    | NUMPAD5 = 0x65
    ///<summary>
    ///Numeric keypad 6 key
    ///</summary>
    | NUMPAD6 = 0x66
    ///<summary>
    ///Numeric keypad 7 key
    ///</summary>
    | NUMPAD7 = 0x67
    ///<summary>
    ///Numeric keypad 8 key
    ///</summary>
    | NUMPAD8 = 0x68
    ///<summary>
    ///Numeric keypad 9 key
    ///</summary>
    | NUMPAD9 = 0x69
    ///<summary>
    ///Multiply key
    ///</summary>
    | MULTIPLY = 0x6A
    ///<summary>
    ///Add key
    ///</summary>
    | ADD = 0x6B
    ///<summary>
    ///Separator key
    ///</summary>
    | SEPARATOR = 0x6C
    ///<summary>
    ///Subtract key
    ///</summary>
    | SUBTRACT = 0x6D
    ///<summary>
    ///Decimal key
    ///</summary>
    | DECIMAL = 0x6E
    ///<summary>
    ///Divide key
    ///</summary>
    | DIVIDE = 0x6F
    ///<summary>
    ///F1 key
    ///</summary>
    | F1 = 0x70
    ///<summary>
    ///F2 key
    ///</summary>
    | F2 = 0x71
    ///<summary>
    ///F3 key
    ///</summary>
    | F3 = 0x72
    ///<summary>
    ///F4 key
    ///</summary>
    | F4 = 0x73
    ///<summary>
    ///F5 key
    ///</summary>
    | F5 = 0x74
    ///<summary>
    ///F6 key
    ///</summary>
    | F6 = 0x75
    ///<summary>
    ///F7 key
    ///</summary>
    | F7 = 0x76
    ///<summary>
    ///F8 key
    ///</summary>
    | F8 = 0x77
    ///<summary>
    ///F9 key
    ///</summary>
    | F9 = 0x78
    ///<summary>
    ///F10 key
    ///</summary>
    | F10 = 0x79
    ///<summary>
    ///F11 key
    ///</summary>
    | F11 = 0x7A
    ///<summary>
    ///F12 key
    ///</summary>
    | F12 = 0x7B
    ///<summary>
    ///F13 key
    ///</summary>
    | F13 = 0x7C
    ///<summary>
    ///F14 key
    ///</summary>
    | F14 = 0x7D
    ///<summary>
    ///F15 key
    ///</summary>
    | F15 = 0x7E
    ///<summary>
    ///F16 key
    ///</summary>
    | F16 = 0x7F
    ///<summary>
    ///F17 key  
    ///</summary>
    | F17 = 0x80
    ///<summary>
    ///F18 key  
    ///</summary>
    | F18 = 0x81
    ///<summary>
    ///F19 key  
    ///</summary>
    | F19 = 0x82
    ///<summary>
    ///F20 key  
    ///</summary>
    | F20 = 0x83
    ///<summary>
    ///F21 key  
    ///</summary>
    | F21 = 0x84
    ///<summary>
    ///F22 key, (PPC only) Key used to lock device.
    ///</summary>
    | F22 = 0x85
    ///<summary>
    ///F23 key  
    ///</summary>
    | F23 = 0x86
    ///<summary>
    ///F24 key  
    ///</summary>
    | F24 = 0x87
    ///<summary>
    ///NUM LOCK key
    ///</summary>
    | NUMLOCK = 0x90
    ///<summary>
    ///SCROLL LOCK key
    ///</summary>
    | SCROLL = 0x91
    ///<summary>
    ///Left SHIFT key
    ///</summary>
    | LSHIFT = 0xA0
    ///<summary>
    ///Right SHIFT key
    ///</summary>
    | RSHIFT = 0xA1
    ///<summary>
    ///Left CONTROL key
    ///</summary>
    | LCONTROL = 0xA2
    ///<summary>
    ///Right CONTROL key
    ///</summary>
    | RCONTROL = 0xA3
    ///<summary>
    ///Left MENU key
    ///</summary>
    | LMENU = 0xA4
    ///<summary>
    ///Right MENU key
    ///</summary>
    | RMENU = 0xA5
    ///<summary>
    ///Windows 2000/XP: Browser Back key
    ///</summary>
    | BROWSER_BACK = 0xA6
    ///<summary>
    ///Windows 2000/XP: Browser Forward key
    ///</summary>
    | BROWSER_FORWARD = 0xA7
    ///<summary>
    ///Windows 2000/XP: Browser Refresh key
    ///</summary>
    | BROWSER_REFRESH = 0xA8
    ///<summary>
    ///Windows 2000/XP: Browser Stop key
    ///</summary>
    | BROWSER_STOP = 0xA9
    ///<summary>
    ///Windows 2000/XP: Browser Search key 
    ///</summary>
    | BROWSER_SEARCH = 0xAA
    ///<summary>
    ///Windows 2000/XP: Browser Favorites key
    ///</summary>
    | BROWSER_FAVORITES = 0xAB
    ///<summary>
    ///Windows 2000/XP: Browser Start and Home key
    ///</summary>
    | BROWSER_HOME = 0xAC
    ///<summary>
    ///Windows 2000/XP: Volume Mute key
    ///</summary>
    | VOLUME_MUTE = 0xAD
    ///<summary>
    ///Windows 2000/XP: Volume Down key
    ///</summary>
    | VOLUME_DOWN = 0xAE
    ///<summary>
    ///Windows 2000/XP: Volume Up key
    ///</summary>
    | VOLUME_UP = 0xAF
    ///<summary>
    ///Windows 2000/XP: Next Track key
    ///</summary>
    | MEDIA_NEXT_TRACK = 0xB0
    ///<summary>
    ///Windows 2000/XP: Previous Track key
    ///</summary>
    | MEDIA_PREV_TRACK = 0xB1
    ///<summary>
    ///Windows 2000/XP: Stop Media key
    ///</summary>
    | MEDIA_STOP = 0xB2
    ///<summary>
    ///Windows 2000/XP: Play/Pause Media key
    ///</summary>
    | MEDIA_PLAY_PAUSE = 0xB3
    ///<summary>
    ///Windows 2000/XP: Start Mail key
    ///</summary>
    | LAUNCH_MAIL = 0xB4
    ///<summary>
    ///Windows 2000/XP: Select Media key
    ///</summary>
    | LAUNCH_MEDIA_SELECT = 0xB5
    ///<summary>
    ///Windows 2000/XP: Start Application 1 key
    ///</summary>
    | LAUNCH_APP1 = 0xB6
    ///<summary>
    ///Windows 2000/XP: Start Application 2 key
    ///</summary>
    | LAUNCH_APP2 = 0xB7
    ///<summary>
    ///Used for miscellaneous characters; it can vary by keyboard.
    ///</summary>
    | OEM_1 = 0xBA
    ///<summary>
    ///Windows 2000/XP: For any country/region, the '+' key
    ///</summary>
    | OEM_PLUS = 0xBB
    ///<summary>
    ///Windows 2000/XP: For any country/region, the ',' key
    ///</summary>
    | OEM_COMMA = 0xBC
    ///<summary>
    ///Windows 2000/XP: For any country/region, the '-' key
    ///</summary>
    | OEM_MINUS = 0xBD
    ///<summary>
    ///Windows 2000/XP: For any country/region, the '.' key
    ///</summary>
    | OEM_PERIOD = 0xBE
    ///<summary>
    ///Used for miscellaneous characters; it can vary by keyboard.
    ///</summary>
    | OEM_2 = 0xBF
    ///<summary>
    ///Used for miscellaneous characters; it can vary by keyboard. 
    ///</summary>
    | OEM_3 = 0xC0
    ///<summary>
    ///Used for miscellaneous characters; it can vary by keyboard. 
    ///</summary>
    | OEM_4 = 0xDB
    ///<summary>
    ///Used for miscellaneous characters; it can vary by keyboard. 
    ///</summary>
    | OEM_5 = 0xDC
    ///<summary>
    ///Used for miscellaneous characters; it can vary by keyboard. 
    ///</summary>
    | OEM_6 = 0xDD
    ///<summary>
    ///Used for miscellaneous characters; it can vary by keyboard. 
    ///</summary>
    | OEM_7 = 0xDE
    ///<summary>
    ///Used for miscellaneous characters; it can vary by keyboard.
    ///</summary>
    | OEM_8 = 0xDF
    ///<summary>
    ///Windows 2000/XP: Either the angle bracket key or the backslash key on the RT 102-key keyboard
    ///</summary>
    | OEM_102 = 0xE2
    ///<summary>
    ///Windows 95/98/Me, Windows NT 4.0, Windows 2000/XP: IME PROCESS key
    ///</summary>
    | PROCESSKEY = 0xE5
    ///<summary>
    ///Windows 2000/XP: Used to pass Unicode characters as if they were keystrokes. The VK_PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods. For more information, see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP
    ///</summary>
    | PACKET = 0xE7
    ///<summary>
    ///Attn key
    ///</summary>
    | ATTN = 0xF6
    ///<summary>
    ///CrSel key
    ///</summary>
    | CRSEL = 0xF7
    ///<summary>
    ///ExSel key
    ///</summary>
    | EXSEL = 0xF8
    ///<summary>
    ///Erase EOF key
    ///</summary>
    | EREOF = 0xF9
    ///<summary>
    ///Play key
    ///</summary>
    | PLAY = 0xFA
    ///<summary>
    ///Zoom key
    ///</summary>
    | ZOOM = 0xFB
    ///<summary>
    ///Reserved 
    ///</summary>
    | NONAME = 0xFC
    ///<summary>
    ///PA1 key
    ///</summary>
    | PA1 = 0xFD
    ///<summary>
    ///Clear key
    ///</summary>
    | OEM_CLEAR = 0xFE  

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

let private _keyUp   = Event<VK>()
let private _keyDown = Event<VK>()
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