using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

public class Registry
{
    public string key;
    public string value;

    public Registry(string k, string v)
    {
        key = k;
        value = v;
    }
}

public class Database
{
    private static List<Registry> registries = new List<Registry>();

    public static void PushDB(List<Registry> newDb)
    {
        registries = newDb;
    }

    public static bool AddReg(Registry reg)
    {
        bool keyRepeated = false;

        foreach (Registry r in registries)
        {
            if (r.key == reg.key)
            {
                keyRepeated = true;
            }
        }

        if (keyRepeated)
        {
            return false;
        }
        else
        {
            registries.Add(reg);
            return true;
        }
    }

    public static bool ReplaceReg(Registry reg)
    {
        bool keyRepeated = false;
        Registry _r = null;

        foreach (Registry r in registries)
        {
            if (r.key == reg.key)
            {
                keyRepeated = true;
                _r = r;
            }
        }

        if (keyRepeated)
        {
            _r.value = reg.value;
            return true;
        }
        else
        {
            registries.Add(reg);
            return true;
        }
    }

    public static void RemoveReg(string key)
    {
        Registry target = null;
        foreach (Registry r in registries)
        {
            if (r.key == key)
            {
                target = r;
            }
        }

        registries.Remove(target);
    }

    public static List<Registry> GetRegistries()
    {
        return registries;
    }

    public static bool CheckReg(string regKey)
    {
        bool target = false;
        foreach (Registry r in registries)
        {
            if (r.key == regKey)
            {
                target = true;
            }
        }

        return target;
    }
}