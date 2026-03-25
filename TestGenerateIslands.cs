using Godot;
using System;

[Tool]
public partial class TestGenerateIslands : Node3D
{
	public IslandFactory islandFactory = new();

	[Export]
	public bool update = false;

	public void generateIslands()
	{
		for(int i = 0; i < 5; i++)
		{
			var node = islandFactory.GenerateIsland(100);
			node.Position = 120.0f*i*Vector3.Forward;
			AddChild(node);
			node.Owner = GetTree().EditedSceneRoot;
			GD.Print("island");
		}
	}

	public override void _Process(double delta)
	{

		if (update)
		{
			generateIslands();
			update = false;
		}
	}
	
}
