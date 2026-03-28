
using Godot;



namespace RandomIslandExploration.Scripts.Boat;



public partial class BoatUi : BoatController
{
    [Export]
    private Slider _throttleSlider;

    [Export]
    private Wheel _wheel;

    [Export]
    private Map _map;

    [Export]
    private TextureButton _mapButton;



    public override void _Ready()
    {
        _mapButton.Pressed += ShowMap;
    }



    private void ShowMap()
    {
        _map.Toggle();
    }



    public override ControlState PollCurrentControl()
        => new((float)(_throttleSlider.Value / _throttleSlider.MaxValue), _wheel.Steering);



    public override void ResetThrottle() => _throttleSlider.Value = 0.0f;
}
