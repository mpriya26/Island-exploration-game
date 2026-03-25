using Godot;

namespace RandomIslandExploration.Scripts.Islands.Noise;

[Tool]
[GlobalClass]
public partial class PerlinNoiseSettings : NoiseSettings
{
    [Export]
    public float NoiseScale = 1.0f;

    [Export]
    public float Height = 1.0f;

    [Export]
    public float ShapeNoiseFactor = 0.5f;

    [Export]
    public FastNoiseLite Noise;

    [Export]
    public FastNoiseLite ShapeNoise;



    public override float PointHeight(Vector2 point)
    {
        point = (point - new Vector2(0.5f, 0.5f)) * 2.0f;
        var offsetPoint = 2.0f * point.Lerp(new Vector2(ShapeNoise.GetNoise2Dv(point), ShapeNoise.GetNoise2Dv(point + new Vector2(0.0f, 1000.0f))), ShapeNoiseFactor);

        return ((NoiseScale * Noise.GetNoise2Dv(offsetPoint) + (Height * (1.0f - offsetPoint.LengthSquared()))) * Mathf.Clamp(0.95f - point.LengthSquared(), 0.0f, 1.0f)) - 0.01f;
    }
}