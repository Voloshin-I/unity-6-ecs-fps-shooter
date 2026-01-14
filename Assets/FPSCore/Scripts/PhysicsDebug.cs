// using Unity.Entities;
// using Unity.Physics;
// using UnityEngine;
//
// namespace FPSCore
// {
//     [UpdateInGroup(typeof(InitializationSystemGroup))]
//     public partial class PhysicsDebugSystem : SystemBase
//     {
//         private bool _logged;
//
//         protected override void OnUpdate()
//         {
//             if (_logged) return;
//             _logged = true;
//
//             // Count entities with physics components
//             int colliderCount = 0;
//             int dynamicCount = 0;
//             int playerCount = 0;
//
//             foreach (var (collider, entity) in SystemAPI.Query<RefRO<PhysicsCollider>>().WithEntityAccess())
//             {
//                 colliderCount++;
//                 
//                 if (SystemAPI.HasComponent<PhysicsVelocity>(entity))
//                     dynamicCount++;
//                     
//                 if (SystemAPI.HasComponent<PlayerTag>(entity))
//                     playerCount++;
//             }
//
//             Debug.Log($"[PhysicsDebug] PhysicsColliders: {colliderCount}, Dynamic bodies: {dynamicCount}, Players with collider: {playerCount}");
//             
//             // Check if player has all required components
//             foreach (var (player, mass, velocity, collider, entity) in 
//                 SystemAPI.Query<RefRO<PlayerTag>, RefRO<PhysicsMass>, RefRO<PhysicsVelocity>, RefRO<PhysicsCollider>>()
//                     .WithEntityAccess())
//             {
//                 Debug.Log($"[PhysicsDebug] Player entity {entity.Index}: Has PhysicsMass, PhysicsVelocity, PhysicsCollider - OK!");
//                 Debug.Log($"[PhysicsDebug] Player InverseMass: {mass.ValueRO.InverseMass}, InverseInertia: {mass.ValueRO.InverseInertia}");
//             }
//             
//             // Check for static bodies (collider but no velocity)
//             int staticCount = 0;
//             foreach (var (collider, entity) in SystemAPI.Query<RefRO<PhysicsCollider>>().WithEntityAccess())
//             {
//                 if (!SystemAPI.HasComponent<PhysicsVelocity>(entity))
//                 {
//                     staticCount++;
//                 }
//             }
//             Debug.Log($"[PhysicsDebug] Static colliders (no velocity): {staticCount}");
//         }
//     }
// }
