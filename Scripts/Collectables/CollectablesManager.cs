
using System.Collections.Generic;
using Godot;
using System.Text.Json;



namespace RandomIslandExploration.Scripts.Collectables;



public partial class CollectablesManager: Node
{
    public static CollectablesManager Instance { get; private set; }
    



    [Export]
    private string _collectablesDataPath;



    public List<Collectable> Collectables = [];



    public override void _Ready()
    {
        Instance = this;
        LoadCollectables();
    }



    private void LoadCollectables()
    {
        var collectablesJson = FileAccess.GetFileAsString(_collectablesDataPath);

        Collectables = JsonSerializer.Deserialize<List<Collectable>>(collectablesJson);

        GD.Print(string.Join('\n', Collectables));
    }
}
