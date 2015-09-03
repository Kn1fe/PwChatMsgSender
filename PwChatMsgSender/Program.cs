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
            Thread t = new Thread(new ParameterizedThreadStart(IntervalRulesManager));
            t.Start(rule.ir);
            Thread t1 = new Thread(new ParameterizedThreadStart(onTimeRulesManager));
            t1.Start(rule.onr);
        }

        public static void IntervalRulesManager(object i)
        {
            List<Rule> ir = (List<Rule>)i;
            TimerCallback tcb = SendMessage;
            for (int a = 0; a < ir.Count; ++a)
            {
                Timer stateTimer = new Timer(tcb, ir[a], 100, Convert.ToInt32(ir[a].time) * 1000);
            }
        }

        public static void onTimeRulesManager(object i)
        {
            List<Rule> onr = (List<Rule>)i;
            while (Work)
            {
                string time = DateTime.Now.ToString("H:m");
                for (int a = 0; a < onr.Count; ++a)
                {
                    if (onr[a].time == time)
                    {
                        SendMessage(onr[a]);
                    }
                }
                Thread.Sleep(59900);
            }
        }

        public static void SendMessage(object i)
        {
            Rule r = (Rule)i;
            Packets p = new Packets();
            //ID Канала
            p.PackByte(r.channel);
            //Эмоции
            p.PackByte(r.emotion);
            //ID аккаунта
            p.PackInt(r.id);
            //Текст сообщения
            p.PackString(r.msg);
            //NULL DATA
            p.PackCuint(0);
            p.getSLenght();
            p.addCuint(120, 0);
            p.addCuint(p.s_lenght, 2);
            p.SendData(c.ip);
            Console.WriteLine(DateTime.Now.ToString("d/M/yyyy HH:mm:ss") + " | Message: " + r.msg + " | Channel: " + r.channel + " | Account ID: " + r.id);
        }
    }
}
