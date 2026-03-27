
using Godot;



namespace RandomIslandExploration.Scripts.Islands;



public interface IIslandFactory
{
    Island GenerateIsland(float size);
}