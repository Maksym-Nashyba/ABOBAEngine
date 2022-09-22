using ABOBAEngine.Components;
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

    protected override async void OnLoad()
    {
        _camera = new Camera
        {
            Transform =
            {
                Position = new Vector3(0, 0, -2f)
            }
        };

        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);
        GL.CullFace(CullFaceMode.Front);
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        ObjModelLoader modelLoader = new ObjModelLoader("perfection.obj");
        Model model =  modelLoader.Load();

        Shader shader = new Shader("shader.vert", "shader.frag");
        Material material = new Material(shader);
        _renderObject = RenderObject.Instantiate(model, material);
        
        base.OnLoad();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
        Vector3 up = new Vector3(0.0f, 1.0f,  0.0f);
        
        KeyboardState keyboard = KeyboardState.GetSnapshot();
        if (keyboard.IsKeyDown(Keys.Escape)) Close();
        if (keyboard.IsKeyDown(Keys.W)) _camera.Transform.Position += front * 0.05f; //Forward 
        if (keyboard.IsKeyDown(Keys.S)) _camera.Transform.Position -= front * 0.05f; //Backwards
        if (keyboard.IsKeyDown(Keys.A)) _camera.Transform.Position -= Vector3.Normalize(Vector3.Cross(front, up)) * 0.05f; //Left
        if (keyboard.IsKeyDown(Keys.D)) _camera.Transform.Position += Vector3.Normalize(Vector3.Cross(front, up)) * 0.05f; //Right
        if (keyboard.IsKeyDown(Keys.Space)) _camera.Transform.Position += up * 0.05f; //Up 
        if (keyboard.IsKeyDown(Keys.LeftShift)) _camera.Transform.Position -= up * 0.05f; //Down

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