namespace FNF_plus.Base.Note;

public struct NoteInstantiation
{
    
    public long DistanceOfHit;
    public bool IsHit;
    public ushort Preset;
    
    public NoteInstantiation(long distance, ushort preset)
    {
        DistanceOfHit = distance;
        Preset = preset;
    }

    public override string ToString()
    {
        return DistanceOfHit.ToString();
    }
}