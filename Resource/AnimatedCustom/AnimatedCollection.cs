namespace FNF_plus.Resource.AnimatedCustom;

using Godot;

public partial class AnimatedCollection : Resource
{
    [Export]
    public StringName Name { get; set; } = "";
    [Export]
    public float Fps { get; set; } = 12f;
    [Export]
    public bool AutoPlay { get; set; }
    [Export]
    public Texture2D[] Frames;
    

}