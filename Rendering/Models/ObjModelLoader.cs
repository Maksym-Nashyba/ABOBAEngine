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
        
        using (StreamReader reader = GetStream())
        {
            vertices = await ReadVertices(reader);
            triangles = await ReadTriangles(reader);
        }

        return Model.FromMesh(vertices, triangles).Build();
    }

    private async Task<float[]> ReadVertices(StreamReader reader)
    {
        List<Task> chunkProcesses = new List<Task>();
        List<float[]> resultChunks = new List<float[]>();
        
        int iteration = 0;
        bool reachedEnd;
        do
        {
            char[] buffer = new char[ReadChunkSize];
            reader.Read(buffer, 0, ReadChunkSize);

            resultChunks.EnsureCapacity(iteration + 1);
            chunkProcesses.Add(ReadVerticesFromChunk(buffer, ref resultChunks, iteration));
            iteration++;
            reachedEnd = reader.EndOfStream || !buffer[6350..6399].Contains('v');
        } while (!reachedEnd);

        await Task.WhenAll(chunkProcesses);
        List<float> vertices = new List<float>();
        return vertices.ToArray();
    }

    private Task ReadVerticesFromChunk(char[] inputBuffer, 
        ref List<float[]> outputBufferCollection, int outputBufferIndex)
    {
        return Task.FromResult<float[]>(null);
    }

    private Task<uint[]> ReadTriangles(StreamReader reader)
    {
        return null;
    }
}