
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using RandomIslandExploration.Scripts.Islands.Noise;



namespace RandomIslandExploration.Scripts.Islands;



[Tool]
public partial class MeshGeneration : MeshInstance3D
{
	[Export]
	public bool update = false;

	[Export]
	public int resolution = 100;


	[Export]
	public float scale = 1;

	[Export]
	public NoiseSettings noiseSettings;



	public ArrayMesh GenMesh()
	{
		var aMesh = new ArrayMesh();

		var uvs = new List<Vector2>();

		var indices = new List<int>();

		var vertices = new List<Vector3>();
		var normals = new List<Vector3>();


		for (int z = 0; z <= resolution; z++)
		{
			for (int x = 0; x <= resolution; x++)
			{
				var point = new Vector2(x, z) / resolution;
				var height = 10.0f * noiseSettings.PointHeight(point);

				vertices.Add(new Vector3(x, height, z) * scale / resolution);
				uvs.Add(point);
				normals.Add(Vector3.Up);

				if (z < resolution && x < resolution)
				{

					indices.Add((z * (resolution + 1)) + x);
					indices.Add((z * (resolution + 1)) + x + 1);
					indices.Add(((z + 1) * (resolution + 1)) + x);

					indices.Add(((z + 1) * (resolution + 1)) + x + 1);
					indices.Add(((z + 1) * (resolution + 1)) + x);
					indices.Add((z * (resolution + 1)) + x + 1);
				}
			}
		}





		var array = new Godot.Collections.Array();
		array.Resize((int)Mesh.ArrayType.Max);
		array[(int)Mesh.ArrayType.Vertex] = vertices.ToArray();
		array[(int)Mesh.ArrayType.Index] = indices.ToArray();
		array[(int)Mesh.ArrayType.Normal] = normals.ToArray();
		array[(int)Mesh.ArrayType.TexUV] = uvs.ToArray();
		array[(int)Mesh.ArrayType.TexUV2] = uvs.ToArray();
		var st = new SurfaceTool();
		st.CreateFromArrays(array, Mesh.PrimitiveType.Triangles);
		st.GenerateNormals();
		st.GenerateTangents();
		return st.Commit();






	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (update)
		{
			Mesh = GenMesh();
			update = false;
		}
	}

}


