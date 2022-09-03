using ABOBAEngine.Rendering.Materials;
using OpenTK.Graphics.OpenGL4;

namespace ABOBAEngine;

public class RenderObject
{
    private Model _model;
    private Material _material;

    private int _vertexArrayObject;

    public static RenderObject Instantiate()
    {
        int vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(vertexArrayObject);
        
        GL.BindVertexArray(0);
        return new RenderObject
        {
            _vertexArrayObject = vertexArrayObject
        };
    }
    
    public void Render(Camera camera)
    {
        GL.BindVertexArray(_vertexArrayObject);
        _material.Use();
        
        
    }

    public void DisposeOf()
    {
        
    }
}