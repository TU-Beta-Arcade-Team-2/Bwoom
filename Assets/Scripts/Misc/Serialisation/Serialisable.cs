
using System.IO;

public abstract class Serialisable
{
    public abstract void Serialise(StreamWriter writer);
    public abstract void Deserialise(StreamReader reader);

    protected void SerialiseBool(bool value, StreamWriter writer)
    {
        writer.WriteLine(value ? "1" : "0");
    }

    protected bool DeserialiseBool(StreamReader reader)
    {
        int deserialisedValue = int.Parse(reader.ReadLine());

        return deserialisedValue == 1;
    }
}
