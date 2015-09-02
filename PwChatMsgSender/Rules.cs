using System;
using System.Collections.Generic;
using System.IO;

namespace PwChatMsgSender
{
    public struct interval_rules
    {
        public byte channel;
        public byte emotion;
        public uint id;
        public string msg;
        public int interval;
    }

    public struct ontime_rules
    {
        public byte channel;
        public byte emotion;
        public uint id;
        public string msg;
        public string time;
    }

    class Rules
    {
        public List<interval_rules> ir = new List<interval_rules>();
        public List<ontime_rules> onr = new List<ontime_rules>();

        public Rules()
        {
            StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/rules/interval");
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if(!line.StartsWith("#"))
                {
                    string[] s = line.Split(new char[] { ';' });
                    interval_rules i = new interval_rules();
                    i.channel = Convert.ToByte(s[0]);
                    i.emotion = Convert.ToByte(s[1]);
                    i.id = Convert.ToUInt32(s[2]);
                    i.msg = s[3];
                    i.interval = Convert.ToInt32(s[4]);
                    ir.Add(i);
                }
            }
            sr.Close();
            Console.WriteLine("Загружено правил с интервалом: " + ir.Count);

            StreamReader sr1 = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/rules/on_time");
            while ((line = sr1.ReadLine()) != null)
            {
                if (!line.StartsWith("#"))
                {
                    string[] s = line.Split(new char[] { ';' });
                    ontime_rules ot = new ontime_rules();
                    ot.channel = Convert.ToByte(s[0]);
                    ot.emotion = Convert.ToByte(s[1]);
                    ot.id = Convert.ToUInt32(s[2]);
                    ot.msg = s[3];
                    ot.time = s[4];
                    onr.Add(ot);
                }
            }
            sr1.Close();
            Console.WriteLine("Загружено правил по времени: " + onr.Count);
        }
    }
}
