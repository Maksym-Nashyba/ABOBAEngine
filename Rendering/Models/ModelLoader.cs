namespace ABOBAEngine.Rendering.Models;

public abstract class ModelLoader
{
    protected string Path;

    protected ModelLoader(string path)
    {
        Path = path;
    }

    public ModelLoader ForPath(string path)
    {
        if (!IsValidFile(path)) return null!;
    }

    protected abstract bool IsValidFile(string path);

    public abstract Model Load();
}