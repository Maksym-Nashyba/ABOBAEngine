namespace ABOBAEngine.Rendering.Models;

public abstract class ObjLineParser
{
    public abstract char[] Pattern();
        
    public bool LineFits(ReadOnlyMemory<char> line)
    {
        throw new NotImplementedException();
    }
        
    public abstract void Parse(ReadOnlyMemory<char> line);
}

public sealed class VertexObjLineParser : ObjLineParser
{
    public readonly List<float> Vertices = new List<float>();
    private readonly char[] _pattern = new[] { 'v', ' ' };

    public override char[] Pattern() => _pattern;

    public override void Parse(ReadOnlyMemory<char> line)
    {
        throw new NotImplementedException();
    }
}

public sealed class TriangleObjLineParser : ObjLineParser
{
    public readonly List<uint> Triangles = new List<uint>();
    private readonly char[] _pattern = new[] { 'f', ' ' };

    public override char[] Pattern() => _pattern;

    public override void Parse(ReadOnlyMemory<char> line)
    {
        throw new NotImplementedException();
    }
}

public sealed class AlbedoUVObjLineParser : ObjLineParser
{
    public readonly List<float> AlbedoUVs = new List<float>();
    private readonly char[] _pattern = new[] { 'v', 't', ' ' };

    public override char[] Pattern() => _pattern;

    public override void Parse(ReadOnlyMemory<char> line)
    {
        throw new NotImplementedException();
    }
}