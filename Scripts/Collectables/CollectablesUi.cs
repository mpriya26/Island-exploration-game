
using System;
using System.Collections.Generic;
using Godot;



namespace RandomIslandExploration.Scripts.Collectables;



public partial class CollectablesUi : Control
{
    [Export]
    private Button _continueButton;

    [Export]
    private Button _leaveButton;

    [Export]
    private Label _storyLabel;

    [Export]
    private Label _titleLabel;


    private Action<Island, Collectable> _dialogFinishedAction = null;
    private Action<Island> _leaveIslandAction = null;
    private Collectable _currentCollectable;
    private Island _currentIsland;
    private readonly Queue<string> _currentStory = [];



    public override void _Ready()
    {
        _continueButton.Pressed += AdvanceStory;
        _leaveButton.Pressed += Leave;
    }



    private void Leave()
    {
        _leaveIslandAction?.Invoke(_currentIsland);
        CloseInfo();
    }



    private void AdvanceStory()
    {
        if (!_currentStory.TryDequeue(out var nextPart))
        {
            _dialogFinishedAction?.Invoke(_currentIsland, _currentCollectable);
            Leave();
            return;
        }

        _storyLabel.Text = nextPart;
    }



    public void OpenInfo(Island island, Action<Island, Collectable> dialogFinishedAction, Action<Island> leaveAction)
    {
        _dialogFinishedAction = dialogFinishedAction;
        _leaveIslandAction = leaveAction;
        _currentIsland = island;

        _currentStory.Clear();

        if (island.Visited)
        {
            _titleLabel.Text = "Island";
            _currentStory.Enqueue("You've already seen everything to see here.");
            _currentStory.Enqueue("There's nothing else for you.");

            GD.Print("You've already been here.");
        }
        else if (CollectablesManager.Instance.Collectables.TryGetValue(island.CollectableId, out var collectable))
        {
            _titleLabel.Text = collectable.Name;

            foreach (var part in CollectablesManager.Instance.GetIslandStory(island.Size).Split('%'))
            {
                if (!string.IsNullOrWhiteSpace(part)) _currentStory.Enqueue(part.StripEdges());
            }

            foreach (var part in collectable.Story.Split('%'))
            {
                if (!string.IsNullOrWhiteSpace(part)) _currentStory.Enqueue(part.StripEdges());
            }

            GD.Print(collectable);
        }
        else
        {
            _titleLabel.Text = "Island";

            foreach (var part in CollectablesManager.Instance.GetIslandStory(island.Size).Split('%'))
            {
                if (!string.IsNullOrWhiteSpace(part)) _currentStory.Enqueue(part.StripEdges());
            }

            _currentStory.Enqueue("There's nothing here");

            GD.Print("There's nothing here");
        }

        AdvanceStory();

        Show();
    }



    private void CloseInfo()
    {
        _dialogFinishedAction = null;
        _currentCollectable = null;
        _currentStory.Clear();
        Hide();
    }
}
