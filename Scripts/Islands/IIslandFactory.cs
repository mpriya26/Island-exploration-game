
using Godot;



namespace RandomIslandExploration.Scripts.Islands;



public interface IIslandFactory
{
    Node3D GenerateIsland(float size);
}