// using Unity.Entities;
// using Unity.Mathematics;
// using Unity.Physics;
// using UnityEngine;
// using BoxCollider = Unity.Physics.BoxCollider;
//
// namespace FPSCore
// {
//     /// <summary>
//     /// Static floor/ground authoring - creates physics collider directly.
//     /// Attach to any GameObject that should be a static physics surface.
//     /// </summary>
//     public class StaticFloorAuthoring : MonoBehaviour
//     {
//         [Header("Collider Size")]
//         [SerializeField] private float3 _size = new float3(10f, 0.1f, 10f);
//         
//         [Header("Physics Material")]
//         [SerializeField] private float _friction = 0.5f;
//         [SerializeField] private float _restitution = 0f;
//
//         public float3 Size => _size;
//         public float Friction => _friction;
//         public float Restitution => _restitution;
//     }
//
//     public class StaticFloorBaker : Baker<StaticFloorAuthoring>
//     {
//         private const float BevelRadius = 0.01f;
//
//         public override void Bake(StaticFloorAuthoring authoring)
//         {
//             // Use Renderable for static bodies
//             var entity = GetEntity(TransformUsageFlags.Renderable);
//
//             // Create BoxCollider
//             var boxGeometry = new BoxGeometry
//             {
//                 Center = float3.zero,
//                 Size = authoring.Size,
//                 Orientation = quaternion.identity,
//                 BevelRadius = BevelRadius
//             };
//
//             var filter = new CollisionFilter
//             {
//                 BelongsTo = ~0u,
//                 CollidesWith = ~0u,
//                 GroupIndex = 0
//             };
//
//             var material = new Unity.Physics.Material
//             {
//                 Friction = authoring.Friction,
//                 Restitution = authoring.Restitution,
//                 CollisionResponse = CollisionResponsePolicy.Collide
//             };
//
//             var collider = BoxCollider.Create(boxGeometry, filter, material);
//
//             // Add PhysicsCollider - this creates a static body (no mass/velocity)
//             AddComponent(entity, new PhysicsCollider { Value = collider });
//             
//             // Add physics world index
//             AddSharedComponent(entity, new PhysicsWorldIndex());
//         }
//     }
// }
