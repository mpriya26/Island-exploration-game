
using Godot;



public partial class Island : MeshInstance3D
{
    [Export]
    public float Size = 1.0f;

    [Export]
    public int CollectableId = -1;

    [Export]
    public bool Visited = false;


    public override void _Ready()
    {
        // var centerNode = new MeshInstance3D
        // {
        //     Mesh = new SphereMesh(),
        //     Position = new(0.0f, 100.0f, 0.0f),
        // };
        // AddChild(centerNode);
    }
}
