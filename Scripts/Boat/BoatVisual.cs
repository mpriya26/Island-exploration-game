
using Godot;



namespace RandomIslandExploration.Scripts.Boat;




public partial class BoatVisual : Node3D
{
    [Export]
    private Vector3 _period;

    [Export]
    private Vector3 _amount;

    [Export]
    private Vector3 _offset;

    private double _time = 0.0;



    public override void _Process(double delta)
    {
        _time += delta;
        RotationDegrees = _amount * SinWave(_offset + (_period.Inverse() * (float)_time * Mathf.Tau));
    }



    private Vector3 SinWave(Vector3 t)
    {
        return new(Mathf.Cos(t.X), Mathf.Cos(t.Y), Mathf.Cos(t.Z));
    }
}
