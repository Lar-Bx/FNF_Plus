using FNF_plus.Resource.ResourceLoader;
using Godot;
using Godot.Collections;
using CollectionExtensions = System.Collections.Generic.CollectionExtensions;

namespace FNF_plus.Resource.Note;

public static class NotePresetRegister
{
    
    public static Dictionary<ushort, NotePreset> Presets { get; private set; } = new Dictionary<ushort, NotePreset>();


    public static void RegisterPreset(NotePreset preset)
    {
        CollectionExtensions.TryAdd(Presets, preset.Id, preset);
    }
    
    
    public static NotePreset GetPreset(ushort id) => Presets[id];
    
    
    public static void Initialize()
    {
        RegisterPreset(new NotePreset(GetNextId(), StaticResourceLoader.GetLoadedResource("noteLeft0001") as Texture2D));
        RegisterPreset(new NotePreset(GetNextId(), StaticResourceLoader.GetLoadedResource("noteDown0001") as Texture2D));
        RegisterPreset(new NotePreset(GetNextId(), StaticResourceLoader.GetLoadedResource("noteUp0001") as Texture2D));
        RegisterPreset(new NotePreset(GetNextId(), StaticResourceLoader.GetLoadedResource("noteRight0001") as Texture2D));
    }
    
    public static ushort GetNextId()
    {
        ushort id = 0;
        while (Presets.ContainsKey(id))
        {
            id++;
        }
        return id;
    }
    
    
    
    
}