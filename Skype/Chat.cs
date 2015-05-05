using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skype
{
    public class Chat
    {
        public static Dictionary<string, string> ConfigurationSchema
        { get; }
        = new Dictionary<string, string>()
        {
            { "path", @"C:\Users\Luka\AppData\Roaming\Skype\<Skype name>\main.db" }
        };

        private static event Action<string, int> evt;

        public static void Configure(Dictionary<string, string> dict)
        {
            var skype = new Skype(dict["path"]);
            skype.OnMessage += msg =>
            {
                if (evt != null)
                {
                    evt("[skype " + msg.Sender + "] " + msg.Text, 0);
                }
            };
        }

        public static void AddTickHandler(Action<string, int> act)
        {
            evt += act;
        }
    }
}
