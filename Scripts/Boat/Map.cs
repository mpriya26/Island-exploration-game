
using System.Collections.Generic;
using Godot;
using RandomIslandExploration.Scripts.Islands;



namespace RandomIslandExploration.Scripts.Boat;



public partial class Map : Control
{
    [Export]
    private Node3D _boat;

    [Export]
    private IslandPlacer _islands;

    [Export]
    private Texture2D _boatIcon;

    [Export]
    private Texture2D _islandIcon;

    [Export]
    private Texture2D _hiddenAreaIcon;

    [Export]
    private Button _exitMapButton;


    private readonly List<Control> _icons = [];



    public override void _Ready()
    {
        _exitMapButton.Pressed += Clear;
    }



    public void Toggle()
    {
        if (_icons.Count > 0)
        {
            Clear();
        }
        else
        {
            GenerateMap();
        }
    }



    public void Clear()
    {
        foreach (var icon in _icons)
        {
            if (!icon.IsQueuedForDeletion())
            {
                icon.QueueFree();
            }
        }
        _icons.Clear();

        Hide();
    }



    public void GenerateMap()
    {
        Clear();

        var shownPoints = new HashSet<Vector2I>();
        var islandPoints = new Dictionary<Vector2, Node3D>();
        var max = new Vector2I();
        var min = new Vector2I();

        foreach (var kvp in _islands.Islands)
        {
            var gridLocation = new Vector2I(kvp.Key.x, kvp.Key.y);
            shownPoints.Add(gridLocation);

            max = max.Max(gridLocation);
            min = min.Min(gridLocation);

            if (kvp.Value is null) continue;

            islandPoints.Add(gridLocation, kvp.Value);
        }

        var gridSize = max - min;
        if (gridSize.X > gridSize.Y)
        {
            min.Y -= gridSize.X - gridSize.Y;
        }
        else
        {
            max.X += gridSize.Y - gridSize.X;
        }
        max += Vector2I.One;
        min -= Vector2I.One;
        gridSize = max - min;

        var gridSquareSize = Size.X / (2.0f * gridSize.X);
        var offset = -new Vector2(min.X, -max.Y) * gridSquareSize;
        GD.Print(gridSquareSize);
        GD.Print(offset);
        GD.Print(gridSize);
        GD.Print(max);
        GD.Print(min);

        for (int x = min.X; x <= max.X; x++)
        {
            for (int y = min.Y; y <= max.Y; y++)
            {
                var gridPoint = new Vector2I(x, y);
                if (!shownPoints.Contains(gridPoint))
                {
                    var hiddenIcon = new TextureRect
                    {
                        Size = 1.05f * new Vector2(gridSquareSize, gridSquareSize),
                        Position = offset + (gridSquareSize * new Vector2(x, y)),
                        Texture = _hiddenAreaIcon,
                        ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                        MouseFilter = MouseFilterEnum.Ignore,
                        Name = $"HiddenIcon({x}, {y})",
                    };
                    _icons.Add(hiddenIcon);
                }
                else if (islandPoints.TryGetValue(gridPoint, out var island))
                {
                    var islandIcon = new TextureRect
                    {
                        Size = 0.3f * new Vector2(gridSquareSize, gridSquareSize),
                        Position = offset + (gridSquareSize * new Vector2(island.Position.X, island.Position.Z) / _islands.IslandSpacing),
                        Texture = _islandIcon,
                        ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                        MouseFilter = MouseFilterEnum.Ignore,
                        Name = $"IslandIcon({x}, {y})",
                    };
                    _icons.Add(islandIcon);
                }
            }
        }

        var boatIcon = new TextureRect
        {
            Size = 0.2f * new Vector2(gridSquareSize, gridSquareSize),
            Position = offset + (gridSquareSize * new Vector2(_boat.Position.X, _boat.Position.Z) / _islands.IslandSpacing),
            Texture = _boatIcon,
            ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
            MouseFilter = MouseFilterEnum.Ignore,
            Name = $"BoatIcon",
        };
        _icons.Add(boatIcon);

        foreach (var icon in _icons)
        {
            AddChild(icon);
        }

        Show();
    }
}
