namespace ABOBAEngine.Rendering.Models;

public sealed class ObjModelLoader : ModelLoader
{
    private const string FileExtension = ".obj";

    public ObjModelLoader(string path) : base(path)
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
        
        using (StreamReader reader = GetStream())
        {
            //TODO read and process data in chunks
            Memory<char> buffer = new Memory<char>(new char[reader.BaseStream.Length]);
            await reader.ReadAsync(buffer);
            vertices = await ReadVertices(ref buffer);
            triangles = await ReadTriangles(ref buffer);
        }
        

        return Model.FromMesh(vertices, triangles).Build();
    }

    private Task<float[]> ReadVertices(ref Memory<char> buffer)
    {
        throw new NotImplementedException();
    }
    
    
    
    private Task<uint[]> ReadTriangles(ref Memory<char> buffer)
    {
        return null;
    }
}