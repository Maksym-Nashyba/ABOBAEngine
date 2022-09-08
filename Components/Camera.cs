using OpenTK.Mathematics;

namespace ABOBAEngine.Components;

public class Camera : SceneObject
{
    public Matrix4 GetViewMatrix()
    {
        return Matrix4.CreateFromQuaternion(Transform.Rotation) 
            * Matrix4.CreateTranslation(Transform.Position);
    }
}