// dllmain
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class DBSocket
{
    TcpClient cli;

    NetworkStream s;

    static DBSocket singleton;

    public DBSocket(string host, int port)
    {
        singleton = this;
        cli = new TcpClient(host, port);

        singleton.s = singleton.cli.GetStream();

        Thread t = new Thread(() =>
        {
            GetPacketOut();
        });

        t.IsBackground = true;
        t.Start();
    }

    public static void SendPacket(string content)
    {
        if (singleton.cli.Connected)
        {
            Byte[] bufferData = Encoding.ASCII.GetBytes(content);

            singleton.s = singleton.cli.GetStream();
            singleton.s.Write(bufferData, 0, bufferData.Length);
        }
    }

    public void GetPacketOut()
    {
        while (true)
        {
            byte[] buffer = new byte[256];
            string response = "";

            Int32 bytes = singleton.s.Read(buffer, 0, buffer.Length);
            response = System.Text.Encoding.ASCII.GetString(buffer, 0, bytes);

            Console.WriteLine("Packet Data -> {0}", response);
        }
    }

    public static void AddRegistry(string key, string content)
    {
        string packetData = "addReg" + "|" + key + "|" + content;
        SendPacket(packetData);
    }

    public static void ReplaceRegistry(string key, string content)
    {
        string packetData = "replaceReg" + "|" + key + "|" + content;
        SendPacket(packetData);
    }

    public static void GetRegistries()
    {
        string packetData = "getRegistries";
        SendPacket(packetData);
    }

    public static void CheckRegistry(string key)
    {
        string packetData = "checkReg|" + key;
        SendPacket(packetData);
    }

    public static void DeleteRegistry(string key)
    {
        string packetData = "deleteReg|" + key;
        SendPacket(packetData);
    }
}