using ABOBAEngine.Rendering.Materials;
using ABOBAEngine.Rendering.Models;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ABOBAEngine.Components;

public class RenderObject : SceneObject
{
    private Model _model;
    private Material _material;

    private int _vertexArrayObject;     //this models space in V-RAM
    private int _elementBufferObject;   //order of vertices in mesh
    private int _verticesVBO;           //VertexBufferObject - array of vertices
    private int _albedoUVsVBO;          //array of texture UVs

    public static RenderObject Instantiate(Model model, Material material)
    {
        int vertexArrayObject = GL.GenVertexArray();
        int elementBufferObject = GL.GenBuffer();
        int verticesVBO = GL.GenBuffer();
        int albedoUVsVBO = GL.GenBuffer();
        GL.BindVertexArray(vertexArrayObject);
        material.Use();
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, verticesVBO);
        GL.BufferData(BufferTarget.ArrayBuffer, model.Vertices.Length * sizeof(float), model.Vertices,
            BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, model.Vertices.Length * sizeof(uint), 
            model.Triangles, BufferUsageHint.StaticDraw);
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, albedoUVsVBO);
        GL.BufferData(BufferTarget.ArrayBuffer, model.AlbedoMapUVs.Length * sizeof(float), model.AlbedoMapUVs,
            BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
        GL.EnableVertexAttribArray(1);
        
        GL.BindVertexArray(0);
        return new RenderObject
        {
            _vertexArrayObject = vertexArrayObject,
            _elementBufferObject = elementBufferObject,
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

        Matrix4 model = Matrix4.Identity;
        Matrix4 view = camera.GetViewMatrix();
        Matrix4 projection =
            Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 800f / 600f, 0.1f, 100.0f);
        
        Matrix4 transform = model * view * projection;
        _material.Shader.SetUniformMatrix4("transform", ref transform);
        
        GL.DrawElements(PrimitiveType.Triangles, _model.Triangles.Length, DrawElementsType.UnsignedInt, 0);
    }

    public void DisposeOf()
    {
        GL.DeleteBuffer(_verticesVBO);
        GL.DeleteBuffer(_albedoUVsVBO);
        GL.DeleteBuffer(_elementBufferObject);
        _material.Shader.Dispose();
    }
}