#nullable enable
using System.Linq;
using Silk.NET.Maths;
using Sokoban.primitives;
using Sokoban.primitives.components;
using Sokoban.utilities;

namespace Sokoban.entities
{
internal class Stupid : GameObject, ITransform
{
    public ITransform Transform => this;

    public Stupid(string name = "Unnamed")
        : base(GameObject)
    {
        Name = name;
    }
    public override void ShaderConfiguration()
    {
        Mesh.Material.DiffuseTexture?.Bind(0);

        Spo.SetUniform("u_Projection", Game.Camera.Projection);
        Spo.SetUniform("u_View", Game.Camera.Lens.View);
        Spo.SetUniform("u_Model", Transform.View);
    }

    public Vector3D<float> Position { get; set; } = Vector3D<float>.Zero;
    public Quaternion<float> Rotation { get; set; } = Quaternion<float>.Identity;
    public float Scale { get; set; }

    private static GameObject GameObject => ObjectLoader.Load("Stupid").First();
    public void Log(int depth = 0)
    {
        $"Stupid: {Name}".LogLine(depth);
        $"Position:\t<c25 {Position}|>".LogLine(depth + 2);
        $"Left:\t{Transform.Left}".LogLine(depth + 2);
        $"Up:\t\t{Transform.Up}".LogLine(depth + 2);
        $"Forward:\t{Transform.Forward}".LogLine(depth + 2);
        $"Roll/Pitch/Yaw | Local:\t{Rotation.ToRollPitchYaw()}".LogLine(depth + 2);
    }
}
}