using Godot;
using RandomIslandExploration.Scripts.Islands;
using RandomIslandExploration.Scripts.Islands.Noise;
using System;



namespace RandomIslandExploration.Scripts.Islands;



public class IslandFactory : IIslandFactory
{
	public MeshGeneration meshGenerator;

	public RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();

	public IslandFactory()
	{
		meshGenerator = new MeshGeneration();
        var perlinNoiseSettings = new PerlinNoiseSettings
        {
            NoiseScale = 2,
			Height = 20.0f, 
			ShapeNoiseFactor = 0.2f,
			Noise = new FastNoiseLite
			{
				FractalType = FastNoiseLite.FractalTypeEnum.Ridged,
				Frequency = 0.5f,
			},
			ShapeNoise = new FastNoiseLite
			{
				FractalType = FastNoiseLite.FractalTypeEnum.Ridged,
				Frequency = 0.5f,
			}
        };

		meshGenerator.noiseSettings = perlinNoiseSettings;
		
	}
	public Node3D GenerateIsland(float size)
	{
		var noiseSettings = meshGenerator.noiseSettings as PerlinNoiseSettings;

		noiseSettings.NoiseScale = randomNumberGenerator.RandfRange(1, 5);
		noiseSettings.Height = 1.0f * randomNumberGenerator.RandfRange(1, 5);
		noiseSettings.ShapeNoiseFactor = randomNumberGenerator.RandfRange(0.2f, 0.5f);
		
		noiseSettings.Noise.Seed++;
		noiseSettings.ShapeNoise.Seed++;


		meshGenerator.scale = size;
		var newNode = new MeshInstance3D();
		newNode.Mesh = meshGenerator.GenMesh();
		return newNode;
	}	
	
}
