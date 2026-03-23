
using Godot;



namespace RandomIslandExploration.Scripts.Boat;



public partial class Wheel : Control
{
    private bool _mouseDown = false;
    private Vector2 _mouseStartPos = Vector2.Zero;
    private float _startAngle = 0.0f;
    private float _prevAngle = 0.0f;

    public float CurrentAngle { get; private set; }

    [Export]
    private float _autoCenterRevolutions = 0.1f;

    [Export]
    private float _autoCenterSpeed = 0.1f;

    [Export]
    private float _fullRevolutionAngleLimit = 0.9f;

    [Export]
    private float _maxRevolutions = 2.0f;



    public float Steering => CurrentAngle / _maxRevolutions;



    public override void _Process(double delta)
    {
        if (!Input.IsMouseButtonPressed(MouseButton.Left))
        {
            _mouseDown = false;
            // GD.Print("Mouse up");
        }
        else if (!_mouseDown)
        {
            // _mouseDown = true;
            // _mouseStartPos = GetViewport().GetMousePosition();
            // _startAngle = CurrentAngle;
            // GD.Print("Mouse down");
        }

        if (_mouseDown)
        {
            var m = _mouseStartPos - Position - (PivotOffsetRatio * Size);
            var p = GetViewport().GetMousePosition() - Position - (PivotOffsetRatio * Size);
            var angle = m.AngleTo(p);

            // Handle full wheel revolution
            if (angle < _fullRevolutionAngleLimit * -Mathf.Pi && _prevAngle > _fullRevolutionAngleLimit * Mathf.Pi)
            {
                _startAngle += Mathf.Tau;
            }
            else if (angle > _fullRevolutionAngleLimit * Mathf.Pi && _prevAngle < _fullRevolutionAngleLimit * -Mathf.Pi)
            {
                _startAngle -= Mathf.Tau;
            }

            _prevAngle = m.AngleTo(p);
            CurrentAngle = Mathf.Clamp(_startAngle + _prevAngle, -_maxRevolutions * Mathf.Tau, _maxRevolutions * Mathf.Tau);
        }
        else if (Mathf.Abs(CurrentAngle) <= _autoCenterRevolutions * Mathf.Tau)
        {
            CurrentAngle = Mathf.Lerp(CurrentAngle, 0.0f, _autoCenterSpeed);
            if (Mathf.Abs(CurrentAngle) < 0.001f)
            {
                CurrentAngle = 0.0f;
            }
        }

        Rotation = CurrentAngle;
    }



    public override void _Input(InputEvent @event)
    {
        if (!_mouseDown && @event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.ButtonIndex == MouseButton.Left)
        {
            _mouseDown = true;
            _mouseStartPos = GetViewport().GetMousePosition();
            _startAngle = CurrentAngle;
            _prevAngle = 0.0f;
        }
    }
}
