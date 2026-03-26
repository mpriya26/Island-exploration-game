
using System.Collections.Generic;
using Godot;



namespace RandomIslandExploration.Scripts.Islands;



public partial class IslandPlacer : Node3D
{
    [Export]
    private float _islandSpacing;

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



    private readonly Dictionary<(int x, int y), Node3D> _islands = [];
    private readonly IIslandFactory _islandFactory = new IslandFactory();
    private readonly RandomNumberGenerator _rng = new();
    private readonly Queue<(Vector3 pos, float size)> _islandsToGen = [];



    public override void _Process(double delta)
    {
        int radius = Mathf.CeilToInt(_genRadius / _islandSpacing);
        var centerF = (_genCenter.GlobalPosition / _islandSpacing).Round();
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
                        var size = Mathf.Clamp(_rng.Randfn(_islandSize, 0.33f * _islandSize), 0.25f * _islandSize, _jitter * _islandSpacing);
                        _islandsToGen.Enqueue((new Vector3(point.x + jitter.x, 0.0f, point.y + jitter.x) * _islandSpacing, size));
                    }
                }
            }
        }
        
        _genFrame--;
        if (_genFrame < 0 && _islandsToGen.TryDequeue(out var islandToGen))
        {
            var island = _islandFactory.GenerateIsland(islandToGen.size);
            island.Position = islandToGen.pos;
            AddChild(island);
            _genFrame = _genFrameSpacing;
        }
    }
}
