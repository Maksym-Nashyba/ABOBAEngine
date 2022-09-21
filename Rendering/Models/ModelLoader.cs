namespace ABOBAEngine.Rendering.Models;

public abstract class ModelLoader
{
    protected string Path
    {
        get => _path;
        set
        {
            if (!IsValidFile(value)) throw new InvalidDataException($"{value} isn't a valid model file");
            _path = value;
        }
    }
    private string _path;

    public ModelLoader(string path)
    {
        Path = path;
    }
    
    protected abstract bool IsValidFile(string path);

    public abstract Model Load();

    protected StreamReader GetStream()
    {
        return File.OpenText(Path);
    }
}