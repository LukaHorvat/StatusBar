using SQLite;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using System.Linq;
using System.Collections.Generic;

namespace Skype
{
    public class Skype
    {
        string dbPath;
        SQLiteAsyncConnection conn;
        long lastReadTs = 0;

        public event Action<Message> OnMessage;

        public Skype(string dbPath)
        {
            this.dbPath = dbPath;

            conn = new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.ReadOnly | SQLiteOpenFlags.SharedCache | SQLiteOpenFlags.NoMutex);
            lastReadTs = conn.ExecuteScalarAsync<long>("SELECT timestamp FROM Messages ORDER BY id DESC LIMIT 1").Result;

            var watcher = new FileSystemWatcher(Path.GetDirectoryName(dbPath), Path.GetFileName(dbPath));
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.EnableRaisingEvents = true;
            watcher.Changed += delegate (object t, FileSystemEventArgs args)
            {
                CheckMessages();
            };
        }

        private async void CheckMessages()
        {
            try
            {
                await Task.Delay(100); //File is locked when this event fires so we wait for a bit first
                var res = await conn.QueryAsync<Message>("SELECT timestamp as SkypeTime, author as Sender, body_xml as Text FROM Messages WHERE timestamp > ? ORDER BY id ASC", lastReadTs);
                res = res.Where(m => m.Text != null).ToList();
                foreach (var msg in res)
                {
                    msg.Sender = WebUtility.HtmlDecode(msg.Sender);
                    msg.Text = WebUtility.HtmlDecode(msg.Text);
                }

                try
                {
                    if (OnMessage != null)
                    {
                        foreach (var msg in res) OnMessage(msg);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    while (e != null)
                    {
                        Console.WriteLine(e.Message);
                        e = e.InnerException;
                    }
                }
                if (res.Count > 0) lastReadTs = res[res.Count - 1].SkypeTime;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
