using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace FPSCore
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct PlayerLookSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.TryGetSingleton<InputState>(out var input))
                return;

            float dt = SystemAPI.Time.DeltaTime;

            foreach (var transformRW in
                     SystemAPI.Query<RefRW<LocalTransform>>()
                         .WithAll<PlayerTag>())
            {
                float yaw = input.Look.x * dt;
                transformRW.ValueRW.Rotation = math.mul(
                    quaternion.Euler(0f, yaw, 0f),
                    transformRW.ValueRW.Rotation
                );
            }
        }
    }
}