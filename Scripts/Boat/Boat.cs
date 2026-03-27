
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

    [Export]
    private float _sideDrag;

    [Export]
    private float _sideMaxFactor;



    public override void _PhysicsProcess(double delta)
    {
        var controls = _boatController.PollCurrentControl();
        var forward = -_boatRigidBody.Transform.Basis.Column2;

        _boatRigidBody.ApplyForce(_propThrust * forward.Rotated(Vector3.Up, controls.Steering * Mathf.DegToRad(_maxEngineAngle)) * controls.Throttle, -forward * _enginePoint.Position.Z);

        var sideVel = _boatRigidBody.LinearVelocity.Project(_boatRigidBody.Transform.Basis.Column0);
        var sideSpeed = sideVel.Length();
        if (sideSpeed >= 0.001f)
        {
            var sideDragForce = Mathf.Clamp(_sideDrag * sideSpeed, -_sideMaxFactor * _boatRigidBody.Mass, _sideMaxFactor * _boatRigidBody.Mass);
            _boatRigidBody.ApplyForce(sideVel * (sideDragForce / sideSpeed));
        }
    }
}
