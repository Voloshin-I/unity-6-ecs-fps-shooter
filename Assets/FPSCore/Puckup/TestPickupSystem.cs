using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace FPSCore
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.Default)]
    public partial struct TestPickupSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            //var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            foreach (var (testPickup, pickedUp, entity) in
                     SystemAPI.Query<RefRO<TestPickup>, RefRO<PickedUpTag>>()
                         .WithEntityAccess())
            {
                //ecb.DestroyEntity(entity);
                //Debug.LogError("Pickup taken");
                
                //entityManager.DestroyEntity(entity);
                if (SystemAPI.HasBuffer<LinkedEntityGroup>(entity))
                {
                    var buffer = SystemAPI.GetBuffer<LinkedEntityGroup>(entity);
                
                    foreach (var linked in buffer)
                    {
                        ecb.DestroyEntity(linked.Value);
                    }
                }
                else
                {
                    ecb.DestroyEntity(entity);
                }
            }
        }
    }
}