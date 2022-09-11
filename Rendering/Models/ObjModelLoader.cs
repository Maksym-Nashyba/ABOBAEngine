using ABOBAEngine.Utils;

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
        
        using (LineByLineReader reader = new LineByLineReader(GetStream()))
        {
            
        }
        

        return Model.FromMesh(null, null).Build();
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