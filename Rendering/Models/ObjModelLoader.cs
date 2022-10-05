using ABOBAEngine.Utils;

namespace ABOBAEngine.Rendering.Models;

public sealed class ObjModelLoader : ModelLoader
{
    private const string FileExtension = ".obj";

    private ObjLineParser[] _parsers = {
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
        UnfinishedObjData rawData = ReadRawData();
        
        return Model.FromMesh(
                rawData.Vertices,
                rawData.Triangles)
            .WithAlbedoUVs(rawData.AlbedoUVs)
            .Build();
    }

    private UnfinishedObjData ReadRawData()
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

        float[] vertices = ((VertexObjLineParser)_parsers[0]).Vertices.ToArray();
        float[] albedoUVs = ((AlbedoUVObjLineParser)_parsers[1]).AlbedoUVs.ToArray();
        uint[] triangles = ((TriangleObjLineParser)_parsers[2]).Triangles.ToArray();
        return new UnfinishedObjData(vertices, albedoUVs, triangles);
    }
    
    private struct UnfinishedObjData
    {
        public float[] Vertices;
        public float[] AlbedoUVs;
        public uint[] Triangles;

        public UnfinishedObjData(float[] vertices, float[] albedoUVs, uint[] triangles)
        {
            Vertices = vertices;
            AlbedoUVs = albedoUVs;
            Triangles = triangles;
        }
    }
}