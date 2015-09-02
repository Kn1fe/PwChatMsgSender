using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace PwChatMsgSender
{
    class Program
    {
        static Rules rule = new Rules();
        static Config c = new Config();
        public static bool Work = true;

        static void Main(string[] args)
        {
            BackgroundWorker bw = new BackgroundWorker();
            Thread t = new Thread(new ParameterizedThreadStart(IntervalRulesManager));
            t.Start(rule.ir);
            Thread t1 = new Thread(new ParameterizedThreadStart(onTimeRulesManager));
            t1.Start(rule.onr);
        }

        public static void IntervalRulesManager(object i)
        {
            List<interval_rules> ir = (List<interval_rules>)i;
            TimerCallback tcb = SendIntervalMSG;
            for (int a = 0; a < ir.Count; ++a)
            {
                Timer stateTimer = new Timer(tcb, ir[a], 100, ir[a].interval * 1000);
            }
        }

        public static void onTimeRulesManager(object i)
        {
            List<ontime_rules> onr = (List<ontime_rules>)i;
            while (Work)
            {
                string time = DateTime.Now.ToString("H:m");
                for (int a = 0; a < onr.Count; ++a)
                {
                    if (onr[a].time == time)
                    {
                        SendTimeMSG(onr[a]);
                    }
                }
                Thread.Sleep(59900);
            }
        }

        public static void SendIntervalMSG(object i)
        {
            interval_rules ir = (interval_rules)i;
            Packets p = new Packets();
            //ID Канала
            p.PackByte(ir.channel);
            //Эмоции
            p.PackByte(ir.emotion);
            //ID аккаунта
            p.PackInt(ir.id);
            //Текст сообщения
            p.PackString(ir.msg);
            //NULL DATA
            p.PackCuint(0);
            p.getSLenght();
            p.addCuint(120, 0);
            p.addCuint(p.s_lenght, 2);
            p.SendData(c.ip);
            Console.WriteLine(DateTime.Now.ToString("d/M/yyyy HH:mm:ss") + " | Message: " + ir.msg + " | Channel: " + ir.channel + " | Account ID: " + ir.id);
        }

        public static void SendTimeMSG(object i)
        {
            ontime_rules or = (ontime_rules)i;
            Packets p = new Packets();
            //ID Канала
            p.PackByte(or.channel);
            //Эмоции
            p.PackByte(or.emotion);
            //ID аккаунта
            p.PackInt(or.id);
            //Текст сообщения
            p.PackString(or.msg);
            //NULL DATA
            p.PackCuint(0);
            p.getSLenght();
            p.addCuint(120, 0);
            p.addCuint(p.s_lenght, 2);
            p.SendData(c.ip);
            Console.WriteLine(DateTime.Now.ToString("d/M/yyyy HH:mm:ss") + " | Message: " + or.msg + " | Channel: " + or.channel + " | Account ID: " + or.id + " | On Time: " + or.time);
        }
    }
}
