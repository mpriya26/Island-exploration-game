
using Godot;
using RandomIslandExploration.Scripts.Islands.Noise;



namespace RandomIslandExploration.Scripts.Islands;



public class IslandFactory : IIslandFactory
{
	public MeshGeneration meshGenerator;

	public RandomNumberGenerator randomNumberGenerator = new();



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
				Frequency = 0.7f,
			},
			ShapeNoise = new FastNoiseLite
			{
				FractalType = FastNoiseLite.FractalTypeEnum.Ridged,
				Frequency = 0.5f,
			}
		};

		meshGenerator.noiseSettings = perlinNoiseSettings;

	}



	public Island GenerateIsland(float size)
	{
		var noiseSettings = meshGenerator.noiseSettings as PerlinNoiseSettings;

		noiseSettings.NoiseScale = 0.1f * size * randomNumberGenerator.RandfRange(0.5f, 1.0f);
		noiseSettings.Height = 0.25f * size * randomNumberGenerator.RandfRange(0.1f, 1.0f);
		noiseSettings.ShapeNoiseFactor = randomNumberGenerator.RandfRange(0.2f, 0.4f);

		noiseSettings.Noise.Seed = (int)randomNumberGenerator.Randi();
		noiseSettings.ShapeNoise.Seed = (int)randomNumberGenerator.Randi();


		meshGenerator.scale = size;
		var newNode = new Island
		{
			Mesh = meshGenerator.GenMesh(),
			Size = size,
		};
		return newNode;
	}

}
