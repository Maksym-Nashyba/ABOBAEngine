namespace ABOBAEngine.Rendering.Models;

public sealed class ObjModelLoader : ModelLoader
{
    private const string FileExtension = ".obj";

    private ObjModelLoader(string path) : base(path)
    {
    }

    public override ModelLoader ForPath(string path)
    {
        if (!path.EndsWith(FileExtension))
            throw new ArgumentException($"Wrong file extension at:{path}. Should be .obj");

        return new ObjModelLoader(path);
    }

    public override Model Load()
    {
        throw new NotImplementedException();
    }
}