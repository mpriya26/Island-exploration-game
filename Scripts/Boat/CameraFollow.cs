
using Godot;



namespace RandomIslandExploration.Scripts.Boat;



public partial class CameraFollow : Node3D
{
    [Export]
    private Node3D _toFollow;



    [Export]
    private Vector3 _offset;



    [Export]
    private float _rotationOffset;



    [Export]
    private float _followSpeed;



    [Export]
    private float _rotationFollowSpeed;



    public override void _PhysicsProcess(double delta)
    {
        Position = Position.Lerp(_toFollow.GlobalPosition + _offset, _followSpeed);
        Rotation = new(Rotation.X, Mathf.LerpAngle(Rotation.Y, _toFollow.Rotation.Y + _rotationOffset, _rotationFollowSpeed), Rotation.Z);
    }
}
