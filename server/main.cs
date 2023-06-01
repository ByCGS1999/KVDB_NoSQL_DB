using System;
using System.IO;

class Program
{
    static int port;
    static bool runICLI = false;
    public static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            if (args[0] != null)
            {
                port = int.Parse(args[0]) | 32505;
            }
					
            if (args[1] != null)
            {
                IO_Shared.SetDBName(args[1]);
            }
        }
        else
        {
            port = 32505;
            IO_Shared.SetDBName("untitledDB.kvdb");
        }

        if (!Directory.Exists("./Databases/"))
        {
            Directory.CreateDirectory("./Databases");
        }
        else
        {
            if (File.Exists("./Databases/" + IO_Shared.dbName))
            {
                Load.LoadDBFile();
            }
        }

        Console.WriteLine("Setting Up Server...");

        ServerSocket socket = new ServerSocket(port);

        Console.Clear();
        Console.WriteLine("Interactive CLI");
        Console.WriteLine("---------------");
        Console.WriteLine("Type 'help' to see the commands!");
        runICLI = true;
        while (runICLI)
        {
            Console.Write("> ");
            string cmd = Console.ReadLine().ToLower();

            string key, value, res = "";

            switch (cmd)
            {
                case "help":
                    Console.WriteLine("Interactive CLI Commands");
                    Console.WriteLine("------------------------");
                    Console.WriteLine("- save - saves current selected database.");
                    Console.WriteLine("- load - loads current selected database.");
                    Console.WriteLine("- loaddb - loads the inputted database.");
                    Console.WriteLine("- setdb - changes the current selected database to the inputted one.");
                    Console.WriteLine("- savedb - saves to the inputted database. (OVERWRITES IF EXISTS)");
                    Console.WriteLine("- removereg - Removes the inputted registry.");
                    Console.WriteLine("- addreg - Inserts the inputted registry.");
                    Console.WriteLine("- getregs - Displays all the registries {key & value}.");
                    Console.WriteLine("- help - Displays this message.");
                    Console.WriteLine("- clear - Clears the console.");
                    Console.WriteLine("- exit - Closes the database, saves automatically the currently selected database");
                    break;
                case "save":
                    Save.SaveToFile();
                    break;
                case "load":
                    Load.LoadDBFile();
                    break;
                case "loaddb":
                    Console.WriteLine("Database Name");
                    Console.WriteLine("-------------");
                    Console.Write("DB Name -> ");
                    key = Console.ReadLine();
                    IO_Shared.SetDBName(key);
                    Load.LoadDBFile();
                    break;
                case "setdb":
                    Console.WriteLine("Database Name");
                    Console.WriteLine("-------------");
                    Console.Write("DB Name -> ");
                    key = Console.ReadLine();
                    IO_Shared.SetDBName(key);
                    break;
                case "savedb":
                    Console.WriteLine("Database Name");
                    Console.WriteLine("-------------");
                    Console.Write("DB Name -> ");
                    key = Console.ReadLine();
                    IO_Shared.SetDBName(key);
                    Save.SaveToFile();
                    break;
                case "removereg":
                    Console.Clear();
                    Console.WriteLine("Registry Remove");
                    Console.WriteLine("---------------");
                    Console.Write("Key -> ");
                    key = Console.ReadLine();
                    Database.RemoveReg(key);
                    break;
                case "clear":
                    Console.Clear();
                    break;
                case "getregs":
                    Console.Clear();
                    Console.WriteLine(" Registries ");
                    Console.WriteLine("------------");
                    foreach (Registry r in Database.GetRegistries())
                    {
                        res += "{" + r.key + "~" + r.value + "}\n";
                    }
                    Console.WriteLine(res);
                    break;
                case "addreg":
                    Console.Clear();
                    Console.WriteLine("Registry Add");
                    Console.WriteLine("------------");
                    Console.Write("Key -> ");
                    key = Console.ReadLine();
                    Console.WriteLine("------------");
                    Console.Write("Value -> ");
                    value = Console.ReadLine();
                    Console.WriteLine("------------");
                    Console.Clear();
                    res = Database.AddReg(new Registry(key, value)) ? "Success" : "Failed";
                    Console.WriteLine("Status -> " + res);
                    break;
                case "exit":
                    Console.Clear();
                    Console.WriteLine("Saving and Exiting the Database");
                    runICLI = false;
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("The command -> " + cmd + " does not exist, please, check if its written correctly.\nIf you want to display all the commands type in 'help'.");
                    break;
            }
        }
        Save.SaveToFile();
        return;
    }
}