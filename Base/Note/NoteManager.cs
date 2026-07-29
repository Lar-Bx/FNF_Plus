using System;
using System.Collections.Generic;
using FNF_plus.Resource.ResourceLoader;
using FNF_plus.Tool.ConfigTool;
using Microsoft.VisualBasic.CompilerServices;
using Array = Godot.Collections.Array;

namespace FNF_plus.Base.Note;

using Godot;

[GlobalClass]
public sealed partial class NoteManager : Control
{
    public readonly List<NoteGroup> NoteGroups = [];

    public long Progress;
    public bool DoRunning = false;

    public float ScrollScale = 1f;
    public float BPM = 120f;
    public long DecisionInterval = 180L;
    public long DestroyDistance = 300L;
    
    public const float DefaultScrollPixelSize = 0.5f;
    
    
    [Signal]
    public delegate void OnNoteJustPressedEventHandler(NoteGroup group);
    [Signal]
    public delegate void OnNoteJustReleasedEventHandler(NoteGroup group);
    [Signal]
    public delegate void OnNoteHitEventHandler(NoteGroup group, int index);
    
    
    public void ParseJson(Json json)
    {
        var data = (Godot.Collections.Dictionary<string, Array>)json.Data;

        foreach (var pair in data)
        {
            var sq = new NoteGroup(pair.Key)
            {
                Instantiations = new NoteInstantiation[pair.Value.Count],
                NoteTextures = new Texture2D[1],
                Position = Vector2.Zero,
                Scale = Vector2.One,
                Rotation = 0f
            };
            for (int i = 0; i < pair.Value.Count; i++)
            {
                sq.Instantiations[i].DistanceOfHit = (long)((Array)pair.Value[i])[0];
            }
            NoteGroups.Add(sq);

        }
        
    }

    public override void _Ready()
    {
        base._Ready();
        OnNoteJustPressed += PressHandle;
        OnNoteJustReleased += ReleaseHandle;

        var ks = GD.Load<Json>("res://Resource/Example/Note.json");
        
        ParseJson(ks);

        NoteGroups[0].BaseTexture = StaticResourceLoader.GetResource("note.base") as Texture2D;
        NoteGroups[0].NoteTextures[0] = StaticResourceLoader.GetResource("note.base") as Texture2D;
        NoteGroups[0].AutoPlay = false;
        NoteGroups[0].BindKey = GameSetting.KeyBinding.Key.Left;
        DoRunning = true;

    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (!DoRunning) return;
        QueueRedraw();
        
        if (DoRunning) UpdateProgress((float)delta);

    }


    public override void _Draw()
    {
        base._Draw();
        foreach (var group in NoteGroups)
        {
            var xform = new Transform2D(group.Rotation, group.Position).Scaled(group.Scale);
            DrawSetTransformMatrix(xform);
            DrawTexture(group.BaseTexture, Vector2.Zero);
            if (group.PassedIndex >= group.Instantiations.Length) continue;
            
            var dir = Vector2.Up.Rotated(group.Rotation) * DefaultScrollPixelSize / ScrollScale;
            for (var i = group.PassedIndex; i < group.Instantiations.Length; i++)
            {
                var note  =  group.Instantiations[i];
                if (note.IsHit) continue;
                var trueDistance = note.DistanceOfHit - Progress;
                if (trueDistance < 0 && group.AutoPlay)
                {
                    group.Instantiations[i].IsHit = true;
                }
                else
                {
                    if (trueDistance < -DecisionInterval)
                    {
                        GD.Print("MISS");
                        group.Instantiations[i].IsHit = true;
                        
                    }
                    
                    DrawSetTransformMatrix(xform.Translated(dir * (trueDistance))); 
                    DrawTexture(group.NoteTextures[0], Vector2.Zero);
                    
                }

                if (trueDistance >= -DestroyDistance) continue;
                group.PassedIndex++;
                group.Instantiations[i].IsHit = true;


            }
        }
    }

    public void UpdateProgress(float delta)
    {
        Progress += (long)Math.Round(delta * 1000);
    }


    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventKey eventKey)
        {
            // Region Key
            var keyName = eventKey.Keycode.ToString().ToLower();
            
            for (int i = 0; i < NoteGroups.Count; i++)
            { 
                if (IsGroupCompleted(NoteGroups[i])) continue;
                
                
                if (GameSetting.KeyBinding.GetKey(NoteGroups[i].BindKey) == keyName) 
                {
                    if (eventKey.IsPressed())
                    {
                        if (eventKey.IsEcho())
                        { 
                            NoteGroups[i].IsJustPressed = false;
                            NoteGroups[i].IsPressed = true;
                        }
                        else
                        {
                            NoteGroups[i].IsJustPressed = true;
                            NoteGroups[i].IsPressed = true;
                            EmitSignal(SignalName.OnNoteJustPressed, NoteGroups[i]);
                        }
                    }
                    else
                    {
                        NoteGroups[i].IsJustPressed = false;
                        NoteGroups[i].IsPressed = false;
                        EmitSignal(SignalName.OnNoteJustReleased, NoteGroups[i]);
                    }
                }
            }
            
            
            // Region Key End 
            
        }
    }


    public void PressHandle(NoteGroup group)
    {
        for (int i = group.PassedIndex; i < group.Instantiations.Length; i++)
        {
            var trueDistance = group.Instantiations[i].DistanceOfHit - Progress;
            if (group.Instantiations[i].IsHit || Math.Abs(trueDistance) > DecisionInterval) continue;
            
            EmitSignal(SignalName.OnNoteHit, group, i);
            group.PassedIndex++;
            break;
        }
    }

    public void ReleaseHandle(NoteGroup group)
    {
        
    }

    public bool IsGroupCompleted(NoteGroup group) => group.Instantiations[^1].IsHit ||
                                                     group.Instantiations[^1].DistanceOfHit - Progress < -DestroyDistance;

}