namespace FNF_plus.Resource.Note;

using Godot;

public partial class NotePreset : RefCounted , INoteEffect
{

    public readonly ushort Id;
    public readonly Texture2D Texture;
    
    private INoteEffect _noteEffectImplementation;
    public void OnHit(int grade)
    {
        _noteEffectImplementation.OnHit(grade);
    }

    public void OnMiss()
    {
        _noteEffectImplementation.OnMiss();
    }
    
    
    public NotePreset(ushort id, Texture2D texture, INoteEffect noteEffectImplementation = null)
    {
        Id = id;
        Texture = texture;
        _noteEffectImplementation = noteEffectImplementation;
    }
    
    public NotePreset() {}
    
}