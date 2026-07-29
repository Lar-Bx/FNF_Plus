using FNF_plus.Tool.ConfigTool;
using Godot;

namespace FNF_plus.Base.Note;

public partial class NoteGroup : RefCounted
{
    
    public string Name;
    public NoteInstantiation[] Instantiations;
    public int PassedIndex = 0;
    
    public Texture2D BaseTexture;
    public Texture2D[] NoteTextures;
    public Vector2 Position;
    public Vector2 Scale;
    public float Rotation;
    public bool AutoPlay;
    
    public bool IsJustPressed;
    public bool IsPressed;
    
    public GameSetting.KeyBinding.Key BindKey = GameSetting.KeyBinding.Key.None;
    
    public NoteGroup() {}
    
    public NoteGroup(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        var result = $"{Name}:";
        foreach (var p in Instantiations)
        {
            result += p + ",";
        }
        return result;
    }
    
    
}