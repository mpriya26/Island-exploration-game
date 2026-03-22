
using System.Collections.Generic;
using System.Linq;
using Godot;



namespace RandomIslandExploration.Scripts.Islands;



[Tool]
public partial class MeshGeneration : MeshInstance3D
{
	[Export]
	public bool update = false;

	[Export]
	public int width = 1;

	[Export]
	public int height = 1;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	public void GenMesh()
	{
		var aMesh = new ArrayMesh();

		var indices = new List<int>();

		var vertices = new List<Vector3>();
			// new(0, 1, 0),
			// new(1, 1, 0), 
			// new(1, 1, 1),
			// new(0, 1, 1),

			// new(0, 0, 0),
			// new(1, 0, 0),  
			// new(1, 0, 1),
			// new(0, 0, 1),

			for(int x = 0; x < width; x++)
			{
				for(int y = 0; y < height; y++)
				{
					vertices.Add(new Vector3(x, y, 0));

					if(y < width-1 && x < height-1)
					{
					
						indices.Add(y*width+x);
						indices.Add((y+1)*width+x);
						indices.Add(y*width+x+1);

						indices.Add((y+1)*width+x+1);
						indices.Add(y*width+x+1);
						indices.Add((y+1)*width+x);
					}
				}
			}
	

		
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
		//array[(int)Mesh.ArrayType.TexUV] = uvs.ToArray();
		aMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, array);
		Mesh = aMesh;

		




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
