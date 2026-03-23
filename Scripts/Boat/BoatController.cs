
using Godot;



namespace RandomIslandExploration.Scripts.Boat;



public abstract partial class BoatController : Node
{
    public abstract ControlState PollCurrentControl();
}
