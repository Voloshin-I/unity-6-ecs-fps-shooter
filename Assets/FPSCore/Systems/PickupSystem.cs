using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace FPSCore
{
    [BurstCompile]
    public partial struct PickupDetectionSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            float3 playerPos = float3.zero;
            Entity player = Entity.Null;

            foreach (var (transform, entity) in
                     SystemAPI.Query<RefRO<LocalTransform>>()
                         .WithAll<PlayerTag>()
                         .WithEntityAccess())
            {
                playerPos = transform.ValueRO.Position;
                player = entity;
                break;
            }

            if (player == Entity.Null)
                return;

            foreach (var (pickup, transform, entity) in
                     SystemAPI.Query<RefRO<PickupTag>, RefRO<LocalTransform>>()
                         .WithNone<PickedUpTag>()
                         .WithEntityAccess())
            {
                float r = pickup.ValueRO.radius;
                if (math.distancesq(playerPos, transform.ValueRO.Position) <= r * r)
                {
                    ecb.AddComponent(entity, new PickedUpTag() { owner = player });
                    ecb.RemoveComponent<PickupTag>(entity);
                }
            }
        }
    }
}