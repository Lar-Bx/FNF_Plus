using System.Threading;
using Godot;
using Godot.Collections;

namespace FNF_plus.Resource.AnimatedCustom;

public partial class AnimatedSet : Godot.Resource
{
    
    public Array<AnimatedCollection> Collections { get; set; } = [];

    public Texture2D GetFrame<T>(T name, int index)
    {

        if (name is string str)
        {
            foreach (var collection in Collections)
            {
                if (collection.Name == str) return collection.Frames[index];
            }
        }
        else if (name is int i)
        {
            return Collections[i].Frames[index];
        }
        else if (name is StringName strN)
        {
            foreach (var collection in Collections)
            {
                if (collection.Name == strN) return collection.Frames[index];
            }
        }
        else if (name is AnimatedCollection c)
        {
            if (Collections.Contains(c)) return c.Frames[index];
        }

        throw new AbandonedMutexException();

    }
    
    public AnimatedCollection GetCollection<T>(T name)
    {
        if (name is string str)
        {
            foreach (var collection in Collections)
            {
                if (collection.Name == str) return collection;
            }
        }
        else if (name is int i)
        {
            return Collections[i];
        }
        else if (name is StringName strN)
        {
            foreach (var collection in Collections)
            {
                if (collection.Name == strN) return collection;
            }
        }
        else if (name is AnimatedCollection c)
        {
            if (Collections.Contains(c)) return c;
        }

        throw new AbandonedMutexException();
    }
    
    
}