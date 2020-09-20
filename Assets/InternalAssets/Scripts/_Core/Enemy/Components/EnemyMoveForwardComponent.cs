using Unity.Entities;

[GenerateAuthoringComponent]
public struct EnemyMoveForward : IComponentData
{
    public float Speed;
}
