// using Unity.Burst;
// using Unity.Entities;
// using Unity.Physics;
//
// namespace FPSCore
// {
//
//     [BurstCompile]
//     public partial struct PlayerFreezeRotationSystem : ISystem
//     {
//         private bool _initialized;
//
//         public void OnUpdate(ref SystemState state)
//         {
//             if (_initialized)
//                 return;
//
//             foreach (var mass in SystemAPI.Query<RefRW<PhysicsMass>>().WithAll<PlayerTag>())
//             {
//                 var value = mass.ValueRW;
//                 value.InverseInertia.x = 0;
//                 value.InverseInertia.z = 0;
//                 mass.ValueRW = value;
//             }
//
//             _initialized = true; // делаем это только один раз
//         }
//     }
// }