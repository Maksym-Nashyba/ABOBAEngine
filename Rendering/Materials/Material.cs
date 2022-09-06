using OpenTK.Graphics.OpenGL4;

namespace ABOBAEngine.Rendering.Materials;

public class Material
{
    public readonly Shader Shader;
    private readonly Texture _texture;

    public Material(Shader shader)
    {
        Shader = shader;
        _texture = new Texture("perfection.png");
    }

    public virtual void Use()
    {
        Shader.Use();
        _texture.Use(TextureUnit.Texture0);
    }
}