using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class IO_Shared
{
    public static string dbName;

    public static void SetDBName(string dn)
    {
        dbName = dn;
    }

    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    public static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}

public class Save
{
    public static void SaveToFile()
    {
        string parsedData = "";
        foreach (Registry r in Database.GetRegistries())
        {
            parsedData += "{" + r.key + "~" + r.value + "}|";
        }

				if(!IO_Shared.dbName.Contains(".kvdb"))
				{
					Console.WriteLine("Cannot Save DB -> " + IO_Shared.dbName + ". Missing format (.kvdb)");
				}
			
        FileStream fs = File.Create("./Databases/" + IO_Shared.dbName);
				fs.Close();

        if (parsedData.Length - 1 < 0)
        {
            return;
        }

        parsedData = parsedData.Substring(0, parsedData.Length - 1);

        File.WriteAllText("./Databases/" + IO_Shared.dbName, IO_Shared.Base64Encode(parsedData));
    }
}


public class Load
{
    public static void LoadDBFile()
    {
        string dbContents = ""; // db compressed contents

				if(!IO_Shared.dbName.Contains(".kvdb"))
				{
					Console.WriteLine("Cannot Load DB -> " + IO_Shared.dbName + ". Missing format (.kvdb)");
				}

if(File.Exists("./Databases/" + IO_Shared.dbName))
			{dbContents = File.ReadAllText("./Databases/" + IO_Shared.dbName);

        if (dbContents.Length - 1 < 0)
        {
            return;
        }

        dbContents = IO_Shared.Base64Decode(dbContents);

        List<Registry> regs = new List<Registry>();
        // begin parse
        string[] dbC = dbContents.Split("|");

        foreach (string data in dbC)
        {
            string s = "";
            // remove unuseful things
            s = data.Substring(0, data.Length - 1);
            s = s.Substring(1, data.Length - 2);

            // extract k&v
            string[] formatData = s.Split('~');

            Registry regKey = new Registry(formatData[0], formatData[1]);
            regs.Add(regKey);
        }

        Database.PushDB(regs);
			}
			else
			{
				Console.WriteLine("DB File does not exist.");
			}
			
        
    }
}