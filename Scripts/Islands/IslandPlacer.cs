
using System.Collections.Generic;
using Godot;



namespace RandomIslandExploration.Scripts.Islands;



public partial class IslandPlacer : Node3D
{
    [Export]
    public float IslandSpacing { get; private set; }

    [Export]
    private float _jitter;

    [Export]
    private float _islandSize;

    [Export]
    private float _genRadius;

    [Export]
    private float _spawnChance;

    [Export]
    private int _genFrameSpacing = 10;
    private int _genFrame = 0;


    [Export]
    private Node3D _genCenter;



    public IReadOnlyDictionary<(int x, int y), Node3D> Islands => _islands;
    private readonly Dictionary<(int x, int y), Node3D> _islands = [];
    private readonly IIslandFactory _islandFactory = new IslandFactory();
    private readonly RandomNumberGenerator _rng = new();
    private readonly Queue<((int x, int y) point, Vector3 pos, float size)> _islandsToGen = [];



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
                        var size = Mathf.Clamp(_rng.Randfn(_islandSize, 0.33f * _islandSize), 0.25f * _islandSize, _jitter * IslandSpacing);
                        _islandsToGen.Enqueue((point, new Vector3(point.x + jitter.x, 0.0f, point.y + jitter.x) * IslandSpacing, size));
                    }
                }
            }
        }

        _genFrame--;
        if (_genFrame < 0 && _islandsToGen.TryDequeue(out var islandToGen))
        {
            var island = _islandFactory.GenerateIsland(islandToGen.size);
            island.Name = $"Island({islandToGen.point.x}, {islandToGen.point.y})";
            island.Position = islandToGen.pos;
            _islands[islandToGen.point] = island;
            AddChild(island);
            _genFrame = _genFrameSpacing;
        }
    }
}
