
using Godot;



namespace RandomIslandExploration.Scripts.Islands;



public interface IIslandFactory
{
    MeshInstance3D GenerateIsland(float size);
}