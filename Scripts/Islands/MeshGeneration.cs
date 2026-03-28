
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
	public int resolution = 96;


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

		var pointsAboveZero = 0;
		var midPoint = new Vector2();
		for (int z = 0; z <= resolution; z++)
		{
			for (int x = 0; x <= resolution; x++)
			{
				var point = new Vector2(x, z) / resolution;
				var height = noiseSettings.PointHeight(point);
				if (height > 0.0f)
				{
					pointsAboveZero++;
					midPoint += point;
				}

				vertices.Add(new Vector3(point.X, height, point.Y));
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

		float radius = 0.0f;
		if (pointsAboveZero > 0)
		{
			midPoint /= pointsAboveZero;
			for (int i = 0; i < vertices.Count; i++)
			{
				vertices[i] = vertices[i] - new Vector3(midPoint.X, 0.0f, midPoint.Y);
				
				if (vertices[i].Y > 0.0f)
				{
					radius = Mathf.Max(radius, new Vector3(vertices[i].X, 0.0f, vertices[i].Z).Length());
				}
			}

			GD.Print($"{radius}\t{scale}\t{scale / radius}");

			for (int i = 0; i < vertices.Count; i++)
			{
				var normalizedVert = new Vector3(vertices[i].X, 0.0f, vertices[i].Z) * scale / radius;
				vertices[i] = normalizedVert + new Vector3(0.0f, vertices[i].Y, 0.0f);
			}
		}
		else
		{
			GD.Print("Submerged island");
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


