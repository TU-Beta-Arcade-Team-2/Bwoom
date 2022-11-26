
using System.IO;

public abstract class Serialisable
{
    public abstract void Serialise(StreamWriter writer);
    public abstract void Deserialise(StreamReader reader);
}
