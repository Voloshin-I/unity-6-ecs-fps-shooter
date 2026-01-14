// using UnityEngine;
// using Unity.Entities;
// using Unity.Mathematics;
// using Unity.Physics;
// using BoxCollider = Unity.Physics.BoxCollider;
// using Material = Unity.Physics.Material;
//
// namespace FPSCore
// {
//     public class GroundAuthoring : MonoBehaviour
//     {
//         [Header("Размеры")]
//         [SerializeField] private float3 _size = new float3(10f, 1f, 10f);
//
//         [Header("Физические свойства")]
//         [SerializeField] private float _friction = 0.8f;
//         [SerializeField] private float _restitution = 0f;
//
//         public float3 Size => _size;
//         public float Friction => _friction;
//         public float Restitution => _restitution;
//
//         private class Baker : Baker<GroundAuthoring>
//         {
//             private const float BevelRadius = 0.01f;
//
//             public override void Bake(GroundAuthoring authoring)
//             {
//                 var entity = GetEntity(TransformUsageFlags.Renderable | TransformUsageFlags.WorldSpace);
//
//                 // --- 1. Создаём BoxCollider ---
//                 var boxGeometry = new BoxGeometry
//                 {
//                     Center = float3.zero,
//                     Size = authoring.Size,
//                     Orientation = quaternion.identity,
//                     BevelRadius = BevelRadius
//                 };
//
//                 var collider = BoxCollider.Create(boxGeometry,
//                     new CollisionFilter
//                     {
//                         BelongsTo = ~0u,
//                         CollidesWith = ~0u,
//                         GroupIndex = 0
//                     },
//                     new Material
//                     {
//                         Friction = authoring.Friction,
//                         Restitution = authoring.Restitution
//                     });
//
//                 // --- 2. Добавляем ECS-компоненты ---
//                 AddComponent(entity, new PhysicsCollider { Value = collider });
//             }
//         }
//     }
// }
