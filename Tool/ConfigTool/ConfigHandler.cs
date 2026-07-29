using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FNF_plus.Tool.ConfigTool;

using Godot;

public static class ConfigHandler
{
    public const string ConfigPath = "res://GameSetting.cfg";
    public static readonly Dictionary<string, Dictionary<string, string>> RequireKeys = new Dictionary<string, Dictionary<string, string>>()
    {
        {"KeyBind", new Dictionary<string, string>()
        {
            {"left", "d"}, {"down", "f"}, {"up", "j"}, {"right", "k"}
        }}
    };
    
    public static readonly ConfigFile DefaultConfig = new ConfigFile();
    public static ConfigFile Config;

    static ConfigHandler()
    {
        //GD.Print("asdasdsadsadddddddddddddddd");
        foreach (var key in RequireKeys)
        {
            foreach (var value in key.Value)
            {
                DefaultConfig.SetValue(key.Key, value.Key, value.Value);
            }
        }
        Config = GetGameSetting();
    }
    
    
    public static ConfigFile GetGameSetting()
    {
        
        var result = new ConfigFile();
        if (FileAccess.FileExists(ConfigPath))
        {
            var loadErr = result.Load(ConfigPath);
            if (loadErr == Error.Ok)
            {
                //GD.Print("asdasdsaddddddddddddd");
                return CheckConfigValid(result);
            }
        }
        //GD.Print("asdasdasdasds");
        DefaultConfig.Save(ConfigPath);
        return DefaultConfig;
    }

    public static ConfigFile CheckConfigValid(ConfigFile config)
    {
        foreach (var key in RequireKeys)
        {
            foreach (var value in key.Value.Where(value => !config.HasSectionKey(key.Key, value.Key)))
            {
                config.SetValue(key.Key, value.Key, DefaultConfig.GetValue(key.Key, value.Key));
            }
        }
        config.Save(ConfigPath);
        return config;
    }
    
    public static void Init()
    {
        
    }
    
}