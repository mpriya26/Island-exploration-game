
using Godot;



namespace RandomIslandExploration.Scripts.Boat;



public partial class Compass : TextureRect
{
    [Export]
    private Node3D _tracking;

    [Export]
    private float _trackingSpeed;

    [Export]
    private float _trackingDamping;

    private float _velocity;



    public override void _Process(double delta)
    {
        if (_tracking is not null)
        {
            // Rotation += _velocity * (float)delta;
            // _velocity += ((_trackingSpeed * (_tracking.Rotation.Y - Rotation)) - (_trackingDamping * _velocity)) * (float)delta;

            Rotation = _tracking.Rotation.Y;
        }
    }
}
