
using System;
using System.Collections.Generic;
using Godot;
using RandomIslandExploration.Scripts.Collectables;



namespace RandomIslandExploration.Scripts.Boat;



public partial class BoatNode : Node3D
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

    [Export]
    private CollectablesUi _collectablesUi;

    public bool Paused = false;

    public readonly HashSet<int> CollectedItems = [];

    private Island _currentIsland = null;



    public override void _PhysicsProcess(double delta)
    {
        if (Paused) return;

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



    public void InteractWithIsland(Island island)
    {
        if (Paused || _currentIsland == island) return;
        _currentIsland = island;

        if (!_currentIsland.Visited)
        {
            _currentIsland.CollectableId = CollectablesManager.Instance.NextCollectable();
        }

        Paused = true;
        _boatController.ResetThrottle();

        _collectablesUi.OpenInfo(island, DoneIslandInteraction, LeftIslandInteraction);
    }



    private void DoneIslandInteraction(Island island, Collectable collectable)
    {
        island.Visited = true;

        if (collectable is null) return;

        CollectedItems.Add(collectable.Id);
    }



    private void LeftIslandInteraction(Island island)
    {
        Paused = false;
    }



    public void EndInteraction()
    {
        Paused = false;
        _currentIsland = null;
    }
}
