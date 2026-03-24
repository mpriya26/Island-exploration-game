
using System;
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
	public int resolution = 1;


	[Export]
	public float scale = 1;



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

			for(int z = 0; z <= resolution; z++)
			{
				for(int x = 0; x <= resolution; x++)
				{
					var point = new Vector2(x, z)/resolution;
					var height = PointHeight(point);

					vertices.Add(new Vector3(x, height, z) * scale/resolution);

					if(z < resolution && x < resolution)
					{
					
						indices.Add((z*(resolution+1))+x);
						indices.Add((z*(resolution+1))+x+1);
						indices.Add(((z+1)*(resolution+1))+x);

						indices.Add(((z+1)*(resolution+1))+x+1);
						indices.Add(((z+1)*(resolution+1))+x);
						indices.Add((z*(resolution+1))+x+1);
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

	public float PointHeight(Vector2 point)
	{
		return point.Y*2.0f;
	}
}


