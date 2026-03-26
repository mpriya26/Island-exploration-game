using Godot;
using System;


namespace RandomIslandExploration.Scripts.Islands.Noise;



[Tool]
[GlobalClass]
public abstract partial class NoiseSettings : Resource
{
    public abstract float PointHeight(Vector2 point);
}
