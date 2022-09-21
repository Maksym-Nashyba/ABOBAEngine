using ABOBAEngine.Utils;

namespace ABOBAEngine.Rendering.Models;

public sealed class ObjModelLoader : ModelLoader
{
    private const string FileExtension = ".obj";

    private ObjLineParser[] _parsers = new ObjLineParser[]
    {
        new VertexObjLineParser(),
        new AlbedoUVObjLineParser(),
        new TriangleObjLineParser()
    };

    public ObjModelLoader(string path) : base(path)
    {
    }

    protected override bool IsValidFile(string path)
    {
        return path.EndsWith(FileExtension) && File.Exists(path);
    }

    public override Model Load()
    {
        using LineByLineReader reader = new LineByLineReader(GetStream());

        while (!reader.IsEmpty)
        {
            ReadOnlyMemory<char> line = reader.ReadLine();
            for (int i = 0; i < _parsers.Length; i++)
            {
                if (_parsers[i].LineFits(line))
                {
                    _parsers[i].Parse(line);
                    break;
                }
            }
        }
        
        return Model.FromMesh(
                ((VertexObjLineParser)_parsers[0]).Vertices.ToArray(),
                ((TriangleObjLineParser)_parsers[2]).Triangles.ToArray())
            .WithAlbedoUVs(((AlbedoUVObjLineParser)_parsers[1]).AlbedoUVs.ToArray())
            .Build();
    }
}