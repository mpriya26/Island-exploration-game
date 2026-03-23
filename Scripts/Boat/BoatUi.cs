
using Godot;



namespace RandomIslandExploration.Scripts.Boat;



public partial class BoatUi : BoatController
{
    [Export]
    private Slider _throttleSlider;

    [Export]
    private Wheel _wheel;



    public override ControlState PollCurrentControl()
        => new((float)(_throttleSlider.Value / _throttleSlider.MaxValue), _wheel.Steering);
}
