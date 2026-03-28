
using System.Collections.Generic;
using Godot;
using RandomIslandExploration.Scripts.Boat;
using RandomIslandExploration.Scripts.Collectables;



namespace RandomIslandExploration.Scripts.Islands;



public partial class IslandPlacer : Node3D
{
    [Export]
    private float _islandInteractionDistance;


    [Export]
    public float IslandSpacing { get; private set; }

    [Export]
    private float _jitter;

    [Export]
    private float _islandSize;

    [Export]
    private float _genRadius;

    [Export]
    private int _centerNoGenRadius;

    [Export]
    private float _spawnChance;

    [Export]
    private int _genFrameSpacing = 10;
    private int _genFrame = 0;


    [Export]
    private BoatNode _boat;

    [Export]
    private Node3D _genCenter;

    [Export]
    private Material _islandMaterial;



    public IReadOnlyDictionary<(int x, int y), Island> Islands => _islands;
    private readonly Dictionary<(int x, int y), Island> _islands = [];
    private readonly IIslandFactory _islandFactory = new IslandFactory();
    private readonly RandomNumberGenerator _rng = new();
    private readonly Queue<((int x, int y) point, Vector3 pos, float size)> _islandsToGen = [];



    public override void _Ready()
    {
        for (int x = -_centerNoGenRadius; x <= _centerNoGenRadius; x++)
        {
            for (int y = -_centerNoGenRadius; y <= _centerNoGenRadius; y++)
            {
                var point = (x: x, y: y);
                _islands.Add(point, null);
            }
        }
    }



    public override void _Process(double delta)
    {
        int radius = Mathf.CeilToInt(_genRadius / IslandSpacing);
        var centerF = (_genCenter.GlobalPosition / IslandSpacing).Round();
        var center = new Vector2I((int)centerF.X, (int)centerF.Z);
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                var point = (x: x + center.X, y: y + center.Y);
                if (!_islands.ContainsKey(point))
                {
                    _islands.Add(point, null);
                    if (_rng.Randf() < _spawnChance)
                    {
                        var jitter = (x: _rng.RandfRange(0.0f, _jitter), y: _rng.RandfRange(0.0f, _jitter));
                        var size = Mathf.Clamp(_rng.Randfn(_islandSize, 0.3f * _islandSize), 0.1f * _islandSize, (1.0f - _jitter) * IslandSpacing);
                        _islandsToGen.Enqueue((point, new Vector3(point.x + jitter.x, 0.0f, point.y + jitter.x) * IslandSpacing, size));

                        _islands.TryAdd((point.x + 0, point.y + 0), null);
                        _islands.TryAdd((point.x + 0, point.y + 1), null);
                        _islands.TryAdd((point.x + 1, point.y + 0), null);
                        _islands.TryAdd((point.x + 1, point.y + 1), null);
                    }
                }
            }
        }

        _genFrame--;
        if (_genFrame < 0 && _islandsToGen.TryDequeue(out var islandToGen))
        {
            var island = _islandFactory.GenerateIsland(islandToGen.size);
            island.Name = $"Island({islandToGen.point.x}, {islandToGen.point.y})";
            island.MaterialOverride = _islandMaterial;
            island.Position = islandToGen.pos;
            _islands[islandToGen.point] = island;
            AddChild(island);
            _genFrame = _genFrameSpacing;
        }

        Island closestIsland = null;
        var closestDistance = (_jitter * IslandSpacing) + _islandInteractionDistance;
        foreach (var island in Islands.Values)
        {
            if (island is null) continue;
            var distance = island.Position.DistanceTo(_genCenter.Position);
            if (distance < island.Size + _islandInteractionDistance && distance < closestDistance)
            {
                closestIsland = island;
                closestDistance = distance;
            }
        }

        if (closestIsland is not null)
        {
            _boat.InteractWithIsland(closestIsland);
        }
        else
        {
            _boat.EndInteraction();
        }
    }
}
