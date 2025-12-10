using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using CapsuleCollider = Unity.Physics.CapsuleCollider;

namespace FPSCore.Test
{
    public class CapsuleAuthoring : MonoBehaviour
    {
        public float mass = 1f;
        public float radius = 0.5f;
        public float height = 2f;
    }
    
    public class PlayerCapsuleBaker : Baker<CapsuleAuthoring>
    {
        public override void Bake(CapsuleAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            // 1. Создаём CapsuleCollider
            var capsuleGeometry = new CapsuleGeometry
            {
                Vertex0 = new float3(0, -authoring.height * 0.5f, 0),
                Vertex1 = new float3(0, authoring.height * 0.5f, 0),
                Radius = authoring.radius
            };

            var filter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = ~0u,
                GroupIndex = 0
            };

            var collider = CapsuleCollider.Create(capsuleGeometry, filter);

            // 2. Добавляем PhysicsCollider
            AddComponent(entity, new PhysicsCollider { Value = collider });

            // 3. Создаём PhysicsMass с заморозкой вращения по X и Z
            var mass = PhysicsMass.CreateDynamic(collider.Value.MassProperties, authoring.mass);
            mass.InverseInertia = new float3(0f, mass.InverseInertia.y, 0f); // Freeze X/Z

            AddComponent(entity, mass);

            // 4. Добавляем Velocity и Gravity
            AddComponent(entity, new PhysicsVelocity());
            AddComponent(entity, new PhysicsGravityFactor { Value = 1f });

            // 5. Устанавливаем позицию из Transform
            var transform = authoring.transform;
            AddComponent(entity, new Unity.Transforms.LocalTransform
            {
                Position = transform.position,
                Rotation = transform.rotation,
                Scale = transform.localScale.x
            });


        }
    }

}