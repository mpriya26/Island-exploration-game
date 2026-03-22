
using System.Linq;
using Godot;



namespace RandomIslandExploration.Scripts.Islands;



[Tool]
public partial class MeshGeneration : MeshInstance3D
{
	[Export]
	public bool update = false;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	public void GenMesh()
	{
		var a_mesh = new ArrayMesh();
		var vertices = new Vector3[] {
			new(0, 1, 0),
			new(1, 1, 0), 
			new(1, 1, 1),
			new(0, 1, 1),

			new(0, 0, 0),
			new(1, 0, 0), 
			new(1, 0, 1),
			new(0, 0, 1),
		};

		var indices = new int[]
		{
			0, 1, 2, 
			0, 2, 3,
			3, 2, 7,
			2, 6, 7, 
			2, 1, 6, 
			1, 5, 6, 
			1, 4, 5, 
			1, 0, 4, 
			0, 3, 7, 
			4, 0, 7, 
			6, 5, 4, 
			4, 7, 6
		};

		var uvs = new Vector2[]
		{
			new(0, 0), 
			new(1, 0), 
			new(1, 1), 
			new(0, 1), 

			new(0, 0), 
			new(1, 0), 
			new(1, 1), 
			new(0, 1), 
		};

		var array = new Godot.Collections.Array();
		array.Resize((int)Mesh.ArrayType.Max);
		array[(int)Mesh.ArrayType.Vertex] = vertices.ToArray();
		array[(int)Mesh.ArrayType.Index] = indices.ToArray();
		array[(int)Mesh.ArrayType.TexUV] = uvs.ToArray();
		a_mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, array);
		Mesh = a_mesh;



	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(update)
		{
			GenMesh();
			update = false;
		}
	}
}
