#nullable enable
using System.Linq;
using Silk.NET.Maths;
using Sokoban.primitives;
using Sokoban.primitives.components;
using Sokoban.utilities;

namespace Sokoban.entities
{
internal class Cube : GameObject, ITransform
{
    public ITransform Transform => this;

    public Cube(string name = "unnamed")
        : base(GameObject)
    {
        Name = name;
    }

    public override void ShaderConfiguration()
    {
        Spo.Bind();
        Mesh.Material.DiffuseTexture?.Bind(0);
        Mesh.Material.NormalTexture?.Bind(1);
        Mesh.Material.DisplacementTexture?.Bind(2);

        Spo.SetUniform("projection", Game.Camera.Projection);
        Spo.SetUniform("view", Game.Camera.Lens.View);
        Spo.SetUniform("model", Transform.View);

        Spo.SetUniform("lightPos", new Vector3D<float>(1,0,0));
        Spo.SetUniform("viewPos", Game.Camera.Position);

        Spo.SetUniform("heightScale", 0.001);
    }

    public Vector3D<float> Position { get; set; } = Vector3D<float>.Zero;
    public Quaternion<float> Rotation { get; set; } = Quaternion<float>.Identity;
    public float Scale { get; set; }

    private static GameObject GameObject => ObjectLoader.Load("basic").First();
    public void Log(int depth = 0)
    {
        $"Cube: {Name}".LogLine(depth);
        $"Position:\t<c25 {Position}|>".LogLine(depth + 2);
        $"Left:\t<c25{Transform.Left}|>".LogLine(depth + 2);
        $"Up:\t\t<c25{Transform.Up}|>".LogLine(depth + 2);
        $"Forward:\t<c25{Transform.Forward}|>".LogLine(depth + 2);
        $"Roll/Pitch/Yaw | Local:\t<c25{Rotation.ToRollPitchYaw()}|>".LogLine(depth + 2);
        Mesh.Material.Log(depth + 2);
    }
}
}