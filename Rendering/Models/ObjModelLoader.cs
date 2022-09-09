namespace ABOBAEngine.Rendering.Models;

public sealed class ObjModelLoader : ModelLoader
{
    private const string FileExtension = ".obj";
    private const int ReadChunkSize = 6400;

    private ObjModelLoader(string path) : base(path)
    {
    }

    protected override bool IsValidFile(string path)
    {
        return path.EndsWith(FileExtension) && File.Exists(path);
    }

    public override async Task<Model> Load()
    {
        float[] vertices;
        uint[] triangles;
        ReadOnlySpan<char> z

        using (StreamReader reader = GetStream())
        {
            vertices = await ReadVertices(reader);
            triangles = await ReadTriangles(reader);
        }

        return Model.FromMesh(vertices, triangles).Build();
    }

    private async Task<float[]> ReadVertices(StreamReader reader)
    {
        
    }
    
    private Task<uint[]> ReadTriangles(StreamReader reader)
    {
        return null;
    }
}