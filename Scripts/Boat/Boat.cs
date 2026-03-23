
using Godot;
using RandomIslandExploration.Scripts.Boat;



public partial class Boat : Node3D
{
    [Export]
    private BoatController _boatController;

    [Export]
    private RigidBody3D _boatRigidBody;

    [Export]
    private float _propThrust;

    [Export]
    private float _turnSpeed;



    public override void _PhysicsProcess(double delta)
    {
        var controls = _boatController.PollCurrentControl();

        var forward = -_boatRigidBody.Transform.Basis.Column2;

        _boatRigidBody.ApplyForce(_propThrust * forward * controls.Throttle);
        _boatRigidBody.ApplyTorque(_turnSpeed * Vector3.Up * _boatRigidBody.LinearVelocity.LengthSquared() * -controls.Steering);
    }
}
