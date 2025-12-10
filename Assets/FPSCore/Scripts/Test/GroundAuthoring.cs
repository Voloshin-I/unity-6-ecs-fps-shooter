using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace FPSCore.Test
{
    public class GroundAuthoring : MonoBehaviour
    {
        public float width = 10f;
        public float length = 10f;

    }
    
    public class GroundBaker : Baker<GroundAuthoring>
    {
        public override void Bake(GroundAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None); // Static

            // 1. Создаём BoxCollider
            var boxGeometry = new BoxGeometry
            {
                Center = float3.zero,
                Size = new float3(authoring.width, 0.1f, authoring.length),
                Orientation = quaternion.identity,
                BevelRadius = 0f
            };

            var filter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = ~0u,
                GroupIndex = 0
            };

            var collider = Unity.Physics.BoxCollider.Create(boxGeometry, filter);

            // 2. Добавляем PhysicsCollider
            AddComponent(entity, new PhysicsCollider { Value = collider });

            // 3. Добавляем PhysicsMass с InverseMass = 0 (Static)
            var mass = PhysicsMass.CreateKinematic(collider.Value.MassProperties);
            AddComponent(entity, mass);

            // 4. Добавляем LocalTransform
            var transform = authoring.transform;
            AddComponent(entity, new LocalTransform
            {
                Position = transform.position,
                Rotation = transform.rotation,
                Scale = transform.localScale.x
            });
        }
    }
}