using ABOBAEngine.Rendering.Materials;
using ABOBAEngine.Rendering.Models;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;

namespace ABOBAEngine;

public class ApplicationWindow : GameWindow
{
    private Camera _camera;
    private RenderObject _renderObject;
    
    public ApplicationWindow(int width, int height, string title) 
        : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        Size = new Vector2i(width, height);
        Title = title;
    }

    protected override void OnLoad()
    {
        _camera = new Camera
        {
            Position = new Vector3(0f, 0f, -2f)
        };

        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);
        GL.CullFace(CullFaceMode.Front);
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        float[] vertices = {
            0.5f,  0.5f, 0.0f,  // top right
            0.5f, -0.5f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f,  // bottom left
            -0.5f,  0.5f, 0.0f   // top left
        };
        uint[] indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };
        Model model = new Model(vertices, indices);
        
        Shader shader = new Shader("shader.vert", "shader.frag");
        Material material = new Material(shader);
        _renderObject = RenderObject.Instantiate(model, material);
        
        base.OnLoad();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        KeyboardState keyboard = KeyboardState.GetSnapshot();
        if (keyboard.IsKeyDown(Keys.Escape))
        {
            Close();
        }

        base.OnUpdateFrame(args);
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _renderObject.Render(_camera);
        
        Context.SwapBuffers();
        base.OnRenderFrame(e);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        GL.Viewport(0, 0, Size.X, Size.Y);
        base.OnResize(e);
    }

    protected override void OnUnload()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        _renderObject.DisposeOf();
        base.OnUnload();
    }
}