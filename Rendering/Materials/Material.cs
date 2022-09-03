﻿namespace ABOBAEngine.Rendering.Materials;

public abstract class Material
{
    private Shader _shader;

    public Material(Shader shader)
    {
        _shader = shader;
    }

    public virtual void Use()
    {
        _shader.Use();
    }
}