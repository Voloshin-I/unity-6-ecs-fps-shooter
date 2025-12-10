using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using Unity.Transforms;
//using UnityEngine;

namespace FPSCore
{
    public class GroundAuthoring : UnityEngine.MonoBehaviour
    {
        [UnityEngine.Header("Размеры")]
        public float3 size = new float3(10f, 1f, 10f);

        [UnityEngine.Header("Физические свойства")]
        public float friction = 0.8f;
        public float restitution = 0f; // упругость

        class Baker : Baker<GroundAuthoring>
        {
            public override void Bake(GroundAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Renderable | TransformUsageFlags.WorldSpace);

                // --- 1. Создаём BoxCollider ---
                var boxGeometry = new BoxGeometry
                {
                    Center = float3.zero,
                    Size = authoring.size,
                    Orientation = quaternion.identity,
                    BevelRadius = 0.01f
                };

                var collider = BoxCollider.Create(boxGeometry,
                    new CollisionFilter
                    {
                        BelongsTo = ~0u,
                        CollidesWith = ~0u,
                        GroupIndex = 0
                    },
                    new Material
                    {
                        Friction = authoring.friction,
                        Restitution = authoring.restitution
                    });

                // --- 2. Добавляем ECS-компоненты ---
                AddComponent(entity, new PhysicsCollider { Value = collider });
                //AddComponent(entity, new physbod);

                // Статическое тело — без массы и скорости
                //AddComponent<LocalToWorld>(entity);
            }
        }
    }

}