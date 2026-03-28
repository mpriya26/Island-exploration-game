
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    private string _storiesDataPath;

    [Export]
    private float _collectablesRarity = 0.1f;



    public List<IslandStory> Stories = [];
    public Dictionary<int, Collectable> Collectables = [];
    private List<int> _remainingCollectables;



    public int NextCollectable()
    {
        if (_remainingCollectables.Count <= 0 || _rng.Randf() > _collectablesRarity) return -1;
        
        int index = _rng.RandiRange(0, _remainingCollectables.Count - 1);

        int id = _remainingCollectables[index];
        _remainingCollectables.RemoveAt(index);

        GD.Print(id);

        return id;
    }



    public string GetIslandStory(float size)
    {
        var sizedStory = Stories.Where(s => s.MaxSize >= size).MinBy(s => s.MaxSize);

        GD.Print(sizedStory);
        if (sizedStory is null) return string.Empty;

        var sb = new StringBuilder();
        foreach (var storyPart in sizedStory.Stories)
        {
            int index = _rng.RandiRange(0, storyPart.Count - 1);
            sb.Append(storyPart[index]).Append('%');
        }
        
        return sb.ToString();
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


        var storiesJson = FileAccess.GetFileAsString(_storiesDataPath);

        Stories = JsonSerializer.Deserialize<List<IslandStory>>(storiesJson);

        GD.Print(string.Join('\n', Stories));
    }
}
