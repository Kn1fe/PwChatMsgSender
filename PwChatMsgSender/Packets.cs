using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PwChatMsgSender
{
    class Packets
    {
        List<byte> Data = new List<byte>();
        public uint s_lenght;

        public void getSLenght()
        {
            s_lenght = (uint)Data.Count;
        }

        public void PackInt(uint a)
        {
            byte[] d = BitConverter.GetBytes(a);
            Array.Reverse(d);
            Data.AddRange(d);
        }

        public void PackCuint(uint a)
        {
            Data.AddRange(cuint(a));
        }
        public void addCuint(uint a, int pos)
        {
            Data.InsertRange(pos, cuint(a));
        }

        public void PackBytes(byte[] b)
        {
            Data.AddRange(b);
        }

        public void PackByte(byte b)
        {
            Data.Add(b);
        }

        public void PackString(string s)
        {
            byte[] r = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, Encoding.UTF8.GetBytes(s));
            Data.AddRange(cuint((uint)r.Length));
            Data.AddRange(r);
        }

        public byte[] cuint(uint v)
        {
            if (v < 64)
            {
                return new byte[] { (byte)v };
            }
            if (v < 16384)
            {
                byte[] b = BitConverter.GetBytes((ushort)(v + 0x8000));
                Array.Reverse(b);
                return b;
            }
            if (v < 536870912)
            {
                byte[] b = BitConverter.GetBytes(v + 536870912);
                Array.Reverse(b);
                return b;
            }

            return new byte[] { };
        }

        public void debugData()
        {
            foreach (byte b in Data)
            {
                Console.Write(b);
            }
            Console.WriteLine("");
            Console.WriteLine("");
        }

        public void SendData(IPEndPoint ip)
        {
            Socket s = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                s.Connect(ip);
                s.Send(Data.ToArray());
                s.Close();
            } catch (Exception){ Program.Work = false; }
        }
    }
}
