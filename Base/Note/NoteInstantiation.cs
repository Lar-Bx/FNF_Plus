namespace FNF_plus.Base.Note;

public struct NoteInstantiation
{
    
    public long DistanceOfHit;
    public bool IsHit;
    
    public NoteInstantiation(long distance)
    {
        DistanceOfHit = distance;
    }

    public override string ToString()
    {
        return DistanceOfHit.ToString();
    }
}