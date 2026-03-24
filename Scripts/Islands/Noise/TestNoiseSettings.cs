using Godot;

namespace RandomIslandExploration.Scripts.Islands.Noise;

[Tool]
[GlobalClass]

public partial class TestNoiseSettings : NoiseSettings
{
    public override float PointHeight(Vector2 point)
    {
        return point.X * 5.0f;
    }
}