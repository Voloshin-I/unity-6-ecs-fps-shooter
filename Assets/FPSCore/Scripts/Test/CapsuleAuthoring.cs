using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using CapsuleCollider = Unity.Physics.CapsuleCollider;

namespace FPSCore.Test
{
    public class CapsuleAuthoring : MonoBehaviour
    {
        [SerializeField] private float _mass = 1f;
        [SerializeField] private float _radius = 0.5f;
        [SerializeField] private float _height = 2f;

        public float Mass => _mass;
        public float Radius => _radius;
        public float Height => _height;
    }
    
    public class PlayerCapsuleBaker : Baker<CapsuleAuthoring>
    {
        private const float HeightHalfMultiplier = 0.5f;
        private const float GravityFactor = 1f;

        public override void Bake(CapsuleAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            // 1. Создаём CapsuleCollider
            var capsuleGeometry = new CapsuleGeometry
            {
                Vertex0 = new float3(0, -authoring.Height * HeightHalfMultiplier, 0),
                Vertex1 = new float3(0, authoring.Height * HeightHalfMultiplier, 0),
                Radius = authoring.Radius
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
            var mass = PhysicsMass.CreateDynamic(collider.Value.MassProperties, authoring.Mass);
            mass.InverseInertia = new float3(0f, mass.InverseInertia.y, 0f); // Freeze X/Z

            AddComponent(entity, mass);

            // 4. Добавляем Velocity и Gravity
            AddComponent(entity, new PhysicsVelocity());
            AddComponent(entity, new PhysicsGravityFactor { Value = GravityFactor });

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
