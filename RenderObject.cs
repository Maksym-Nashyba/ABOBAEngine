using ABOBAEngine.Rendering.Materials;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ABOBAEngine;

public class RenderObject
{
    private Model _model;
    private Material _material;

    private int _vertexArrayObject;
    private int _verticesVBO;
    private int _albedoUVsVBO;

    public static RenderObject Instantiate(Model model, Material material)
    { 
        model = Model.FromVertexArray(new[] {        
            -0.5f, -0.5f, -0.5f, 
            0.5f, -0.5f, -0.5f, 
            0.5f, 0.5f, -0.5f, 
            0.5f, 0.5f, -0.5f, 
            -0.5f, 0.5f, -0.5f, 
            -0.5f, -0.5f, -0.5f})
            .WithAlbedoUVs(new []
            {
                0.0f, 0.0f,
                1.0f, 0.0f,
                1.0f, 1.0f,
                1.0f, 1.0f,
                0.0f, 1.0f,
                0.0f, 0.0f
            }).Build();
        
        int vertexArrayObject = GL.GenVertexArray();
        int verticesVBO = GL.GenBuffer();
        int albedoUVsVBO = GL.GenBuffer();
        GL.BindVertexArray(vertexArrayObject);
        material.Use();
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, verticesVBO);
        GL.BufferData(BufferTarget.ArrayBuffer, model.Vertices.Length * sizeof(float), model.Vertices,
            BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, albedoUVsVBO);
        GL.BufferData(BufferTarget.ArrayBuffer, model.AlbedoUVs.Length * sizeof(float), model.AlbedoUVs,
            BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
        GL.EnableVertexAttribArray(1);
        
        GL.BindVertexArray(0);
        return new RenderObject
        {
            _vertexArrayObject = vertexArrayObject,
            _albedoUVsVBO = albedoUVsVBO,
            _verticesVBO = verticesVBO,
            _material = material,
            _model = model
        };
    }
    
    public void Render(Camera camera)
    {
        GL.BindVertexArray(_vertexArrayObject);
        _material.Use();
        
        double time = DateTime.Now.Millisecond / 1000d * 180d;
        time = time * 2d - 1d;
        Matrix4 transform = Matrix4.Identity;
        Matrix4 model = Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(time))
                        * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(time));
        Matrix4 view = camera.GetViewMatrix();
        Matrix4 projection =
            Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 800f / 600f, 0.1f, 100.0f);
        transform = transform * model * view * projection;
        _material.Shader.SetUniformMatrix4("transform", ref transform);
        
        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }

    public void DisposeOf()
    {
        GL.DeleteBuffer(_verticesVBO);
        GL.DeleteBuffer(_albedoUVsVBO);
        _material.Shader.Dispose();
    }
}