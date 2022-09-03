using OpenTK.Mathematics;

namespace ABOBAEngine;

public class Camera
{
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }

    public Camera() : this(Vector3.Zero, Quaternion.Identity) { }
    
    public Camera(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }

    public Matrix4 GetViewMatrix()
    {
        return Matrix4.CreateFromQuaternion(Rotation) 
            * Matrix4.CreateTranslation(Position);
    }
}