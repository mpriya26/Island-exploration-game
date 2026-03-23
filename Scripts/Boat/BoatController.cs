
using Godot;



namespace RandomIslandExploration.Scripts.Boat;



public abstract partial class BoatController : GodotObject
{
    public abstract ControlState PollCurrentControl();
}
