using Godot;



public partial class Follow : MeshInstance3D
{
    [Export]
    public Node3D ToFollow;
    
    [Export]
    public Vector3 Offset;




    public override void _PhysicsProcess(double delta)
    {
        if (ToFollow is not null)
        {
            GlobalPosition = Offset + ToFollow.GlobalPosition;
        }
    }
}
