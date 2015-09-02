using System;
using System.Net;
using IniParser;
using IniParser.Model;

namespace PwChatMsgSender
{
    class Config
    {
        public IPEndPoint ip;

        public Config()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(AppDomain.CurrentDomain.BaseDirectory + "/conf.ini");
            ip = new IPEndPoint(IPAddress.Parse(data["SETTINGS"]["ip"]), Convert.ToInt32(data["SETTINGS"]["port"]));
        }
    }
}
