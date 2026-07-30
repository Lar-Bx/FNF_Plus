using System.IO;
using System.Xml.Linq;
using Godot;
using Godot.Collections;
using Array = System.Array;
using FileAccess = Godot.FileAccess;

namespace FNF_plus.Resource.ResourceLoader;

public static class StaticResourceLoader
{
    
    public static Dictionary<string, Godot.Resource> LoadedResources { get; } = [];
    public static System.Collections.Generic.Dictionary<string, XDocument> LoadedXml { get; } = [];
    
    
    public static void RegisterResource(string path, string id)
    {
        var pr = GD.Load<Godot.Resource>(path);
        if (pr == null) throw new FileNotFoundException("Resource not found: " + path);
        
        LoadedResources.Add(id, pr);
        
    }

    public static Godot.Resource GetResource(string id)
    {
        return LoadedResources[id];
    }
    
    

    static StaticResourceLoader()
    {
        RegisterResource("res://icon.svg", "note.base");
        LoadTexturesWithXml([
            ["res://Assets/Shared/Images/Notes.png", ProjectSettings.GlobalizePath("res://Assets/Shared/Images/Notes.xml"), "color_note"]
        ]);
        //GD.Print(LoadedResources);
        AddFromAnalysis(AnalysisXmlToTexture(LoadedXml["color_note"], LoadedResources["color_note"] as Texture2D));
        //GD.Print(LoadedResources);
    }


    public static void LoadTexturesWithXml(string[][] imageAndXmlAndId)
    {
        foreach (var path in imageAndXmlAndId)
        {
            if (!FileAccess.FileExists(path[0]) || !FileAccess.FileExists(path[1]))
            {
                GD.PrintErr(path + "Is not valid");
                continue;
            }
            
            LoadedResources.Add(path[2], GD.Load<Godot.Resource>(path[0]));
            LoadedXml.Add(path[2], XDocument.Load(path[1]));
        }
        
    }


    public static Dictionary<string, Texture2D> AnalysisXmlToTexture(XDocument xml, Texture2D originalTexture)
    {
        var result = new Dictionary<string, Texture2D>();
        
        
        foreach (var item in xml.Root!.Elements("SubTexture"))
        {
            var name = item.Attribute("name")?.Value ?? "";

            var atlas = new AtlasTexture();
            atlas.Atlas = originalTexture;
            
            atlas.Region = new Rect2(
                float.Parse(item.Attribute("x")?.Value ?? "0"),
                float.Parse(item.Attribute("y")?.Value ?? "0"),
                float.Parse(item.Attribute("width")?.Value ?? "0"),
                float.Parse(item.Attribute("height")?.Value ?? "0")
            );
            result[name] = atlas;
            
        }
        return result;
    }
    
    public static void AddFromAnalysis(Dictionary<string, Texture2D> textures)
    {
        foreach (var kvp in textures)
        {
            LoadedResources.Add(kvp.Key, kvp.Value);
        }
    }
    
    
    public static Godot.Resource GetLoadedResource(string id)
    {
        return LoadedResources[id];
    }
    

}