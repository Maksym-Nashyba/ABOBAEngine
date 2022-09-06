namespace ABOBAEngine.Rendering.Models;

public class Model
{
    public readonly float[] Vertices;
    public readonly uint[] Triangles;
    public float[] AlbedoMapUVs => OptionalData[AlbedoUVsKey];
    protected const byte AlbedoUVsKey = 0;
    public float[] NormalMapUVs => OptionalData[NormalUVsKey];
    protected const byte NormalUVsKey = 1;
    public float[] VertexNormalsUVs => OptionalData[VertexNormalsKey];
    protected const byte VertexNormalsKey = 2;
    
    protected readonly Dictionary<byte, float[]> OptionalData;

    public Model(float[] vertices, uint[] triangles)
    {
        Vertices = vertices;
        Triangles = triangles;
        OptionalData = new Dictionary<byte, float[]>();
        OptionalData.Add(AlbedoUVsKey, new[] {
            1.0f, 1.0f,
            1.0f, 0.0f,
            0.0f, 0.0f,
            0.0f, 1.0f
        });
    }
}