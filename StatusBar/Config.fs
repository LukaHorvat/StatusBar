module Config

open System.Drawing

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