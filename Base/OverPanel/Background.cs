using System.IO;
using System.Xml.Linq;
using FNF_plus.Tool.ConfigTool;

namespace FNF_plus.Base.OverPanel;

using Godot;

[GlobalClass]
public partial class Background : Node
{
    public static readonly XDocument ProjectConfig;

    static Background()
    {
        using var file = FileAccess.Open("res://Config/ProjectConfig.xml", FileAccess.ModeFlags.Read);
        if (file is null)
        {
            throw new FileNotFoundException("Config file not found");
        }

        ProjectConfig = XDocument.Parse(file.GetAsText());



    }
    
    public override void _Ready()
    {
        
        // Init
        ConfigHandler.Init();
        GameSetting.KeyBinding.Initialize();
        
        var scPath = ProjectConfig.Root?.Element("run_config")?.Element("main_scene")?.Value;
        var pck = GD.Load<PackedScene>(scPath);
        AddChild(pck.Instantiate());
        
        
        
        
    }
    
}