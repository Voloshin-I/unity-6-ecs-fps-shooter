using FPSCore;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

[BurstCompile(FloatMode = FloatMode.Default, FloatPrecision = FloatPrecision.Standard, CompileSynchronously = true)]
public partial struct PhysicsDebugSystem : ISystem
{
    // public void OnUpdate(ref SystemState state)
    // {
    //     var em = state.EntityManager;
    //     var q = SystemAPI.QueryBuilder().WithAll<PhysicsCollider>().Build();
    //     UnityEngine.Debug.Log($"PhysicsColliders: {q.CalculateEntityCount()}");
    //
    //     var q2 = SystemAPI.QueryBuilder().WithAll<PlayerTag>().Build();
    //     UnityEngine.Debug.Log($"Players: {q2.CalculateEntityCount()}");
    // }
}