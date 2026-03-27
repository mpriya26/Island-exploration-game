
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Godot;



namespace RandomIslandExploration.Scripts.Collectables;



public partial class CollectablesManager : Node
{
    public static CollectablesManager Instance { get; private set; }



    private readonly RandomNumberGenerator _rng = new();




    [Export]
    private string _collectablesDataPath;

    [Export]
    private float _collectablesRarity = 0.1f;



    public Dictionary<int, Collectable> Collectables = [];
    private List<int> _remainingCollectables;



    public int NextCollectable()
    {
        if (_remainingCollectables.Count <= 0 || _rng.Randf() > _collectablesRarity) return -1;
        
        int index = _rng.RandiRange(0, _remainingCollectables.Count - 1);

        int id = _remainingCollectables[index];
        _remainingCollectables.RemoveAt(index);

        return id;
    }



    public override void _Ready()
    {
        Instance = this;
        LoadCollectables();
    }



    private void LoadCollectables()
    {
        var collectablesJson = FileAccess.GetFileAsString(_collectablesDataPath);

        Collectables = JsonSerializer.Deserialize<List<Collectable>>(collectablesJson).ToDictionary(c => c.Id);
        _remainingCollectables = [.. Collectables.Keys];

        GD.Print(string.Join('\n', Collectables));
    }
}
