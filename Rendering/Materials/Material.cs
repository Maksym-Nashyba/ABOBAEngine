namespace ABOBAEngine.Rendering.Materials;

public class Material
{
    public readonly Shader Shader;

    public Material(Shader shader)
    {
        Shader = shader;
    }

    public virtual void Use()
    {
        Shader.Use();
    }
}