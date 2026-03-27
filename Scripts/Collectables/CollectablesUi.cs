
using System;
using System.Collections.Generic;
using Godot;



namespace RandomIslandExploration.Scripts.Collectables;



public partial class CollectablesUi : Control
{
    [Export]
    private Button _continueButton;

    [Export]
    private Label _storyLabel;

    [Export]
    private Label _titleLabel;


    private Action<Island, Collectable> _dialogFinishedAction = null;
    private Collectable _currentCollectable;
    private Island _currentIsland;
    private readonly Queue<string> _currentStory = [];



    public override void _Ready()
    {
        _continueButton.Pressed += AdvanceStory;
    }



    private void AdvanceStory()
    {
        if (!_currentStory.TryDequeue(out var nextPart))
        {
            CloseInfo();
            return;
        }

        _storyLabel.Text = nextPart;
    }



    public void OpenInfo(Island island, Action<Island, Collectable> dialogFinishedAction)
    {
        _dialogFinishedAction = dialogFinishedAction;
        _currentIsland = island;
        if (island.Visited)
        {
            _titleLabel.Text = island.Name;
            _currentStory.Enqueue("You've already seen everything to see here.");
            _currentStory.Enqueue("There's nothing else for you.");

            GD.Print("You've already been here.");
        }
        else if (CollectablesManager.Instance.Collectables.TryGetValue(island.CollectableId, out var collectable))
        {
            _titleLabel.Text = collectable.Name;
            
            _currentStory.Clear();
            foreach (var part in collectable.Story.Split('%'))
            {
                _currentStory.Enqueue(part);
            }

            GD.Print(collectable);
        }
        else
        {
            _titleLabel.Text = island.Name;
            _currentStory.Enqueue("There's nothing here");

            GD.Print("There's nothing here");
        }

        AdvanceStory();

        Show();
    }



    private void CloseInfo()
    {
        _dialogFinishedAction?.Invoke(_currentIsland, _currentCollectable);
        _dialogFinishedAction = null;
        _currentCollectable = null;
        Hide();
    }
}
