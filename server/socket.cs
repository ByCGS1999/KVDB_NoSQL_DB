using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class ServerSocket
{
    TcpListener _listener;

    static ServerSocket singleton;

    public ServerSocket(int port)
    {
        singleton = this;

        _listener = new TcpListener(port);

        _listener.Start();

        Thread t = new Thread(() =>
            {
                while (true)
                {
                    TcpClient cli = _listener.AcceptTcpClient();

                    if (cli != null)
                    {
                        ParsePackets(cli);
                    }
                }
            });
        t.IsBackground = true;
        t.Start();
    }



    public void ParsePackets(TcpClient cli)
    {
        NetworkStream s = cli.GetStream();

        int i;


        Byte[] bytes = new Byte[256];
        string data = null;

        string response = null;

        while ((i = s.Read(bytes, 0, bytes.Length)) != 0)
        {
            data = Encoding.ASCII.GetString(bytes, 0, i);

            if (data.Contains("addReg"))
            {
                string[] subData = data.Split("|");

                Console.WriteLine("Adding a new registry to the database");
                Registry reg = new Registry(subData[1], subData[2]);
                response = Database.AddReg(reg) ? "Success" : "Key_already_exists";
            }
            else if (data.StartsWith("replaceReg"))
            {
                string[] subData = data.Split((char)178);

                Console.WriteLine("Adding a new registry to the database");
                Registry reg = new Registry(subData[1], subData[2]);
                response = Database.ReplaceReg(reg) ? "Success" : "Success";
            }
            else if (data.StartsWith("deleteReg"))
            {
                string[] subData = data.Split((char)178);

                Console.WriteLine("Adding a new registry to the database");
                Database.RemoveReg(subData[1]);
            }
            else if (data.StartsWith("checkReg"))
            {
                string[] subData = data.Split((char)178);

                Console.WriteLine("Adding a new registry to the database");
                response = Database.CheckReg(subData[1]) ? "Exists" : "Non Existant";
            }
            else if (data.StartsWith("getRegistries"))
            {
                List<Registry> regs = Database.GetRegistries();
                foreach (Registry r in regs)
                {
                    response += "{" + r.key + "~" + r.value + "}|";
                }
            }
        }
    }
}