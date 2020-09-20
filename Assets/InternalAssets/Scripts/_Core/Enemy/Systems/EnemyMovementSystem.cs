using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class EnemyMovementSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<EnemyMoveForward>().ForEach((ref Translation trans, ref Rotation rot, ref EnemyMoveForward moveForward) => 
        {
            trans.Value += moveForward.Speed * Time.DeltaTime * math.forward(rot.Value);
        });
    }
}
