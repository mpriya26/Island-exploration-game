
using Godot;
using RandomIslandExploration.Scripts.Boat;



public partial class Boat : Node3D
{
    [Export]
    private BoatController _boatController;

    [Export]
    private RigidBody3D _boatRigidBody;

    [Export]
    private Node3D _enginePoint;

    [Export]
    private float _propThrust;

    [Export]
    private float _maxEngineAngle;



    public override void _PhysicsProcess(double delta)
    {
        var controls = _boatController.PollCurrentControl();
        var forward = -_boatRigidBody.Transform.Basis.Column2;

        _boatRigidBody.ApplyForce(_propThrust * forward.Rotated(Vector3.Up, controls.Steering * Mathf.DegToRad(_maxEngineAngle)) * controls.Throttle, -forward * _enginePoint.Position.Z);
    }
}
