using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
// using UnityEngine;
// using CapsuleCollider = Unity.Physics.CapsuleCollider;

namespace FPSCore
{
    public class CharacterAuthoring : UnityEngine.MonoBehaviour
    {
        public int health = 100;
        public float moveSpeed = 5f;
        public float lookSpeed = 1f;
    }

    public class PlayerBaker : Baker<CharacterAuthoring>
    {
        public override void Bake(CharacterAuthoring authoring)
        {
            // var entity = GetEntity(TransformUsageFlags.Dynamic);
            // AddComponent(entity, new PlayerTag());
            // AddComponent(entity, new Health { Value = authoring.health });
            // AddComponent(entity, new Movement { MoveSpeed = authoring.moveSpeed, LookSpeed = authoring.lookSpeed });
            // AddComponent(entity, new WeaponHolder { WeaponEntity = Entity.Null });
            
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
                //var entity = GetEntity(TransformUsageFlags.Dynamic);

            // Создаём массу с разрешённой инерцией только по Y
            var mass = PhysicsMass.CreateDynamic(MassProperties.UnitSphere, 1f);
            mass.InverseInertia = new Unity.Mathematics.float3(0f, mass.InverseInertia.y, 0f); // X и Z заморожены

            AddComponent(entity, mass);

            // --- 1. Создаём CapsuleCollider ---
            // BlobAssetReference<Collider> collider = CapsuleCollider.Create(new CapsuleGeometry
            //     {
            //         Vertex0 = new float3(0, 0.5f, 0),
            //         Vertex1 = new float3(0, 1.5f, 0),
            //         Radius = 0.5f
            //     },
            //     new CollisionFilter
            //     {
            //         BelongsTo = ~0u,
            //         CollidesWith = ~0u,
            //         GroupIndex = 0
            //     });
                
            // var collider = CapsuleCollider.Create(
            //     new float3(0, authoring.radius, 0),
            //     new float3(0, authoring.height - authoring.radius, 0),
            //     authoring.radius
            // );

            // AddComponent(entity, new PhysicsCollider { Value = collider });
            //
            // // --- 2. Создаём физическую массу (динамическое тело) ---
            // // var mass = PhysicsMass.CreateDynamic(collider.Value.MassProperties, 1);
            // var massProperties = collider.Value.MassProperties;
            // bool invalidMass =
            //     massProperties.Volume <= 0 ||
            //     math.all(massProperties.MassDistribution.InertiaTensor == 0);
            // if (invalidMass)
            // {
            //     massProperties = new MassProperties
            //     {
            //         MassDistribution = new MassDistribution
            //         {
            //             Transform = RigidTransform.identity,
            //             InertiaTensor = new float3(1, 1, 1)
            //         },
            //         Volume = 1,
            //         AngularExpansionFactor = 1
            //     };
            // }
            // var mass = PhysicsMass.CreateDynamic(massProperties, 1f);

            // Зафиксировать вращение по X и Z, оставить только Y
            // mass.InverseInertia.x = 0;
            // mass.InverseInertia.z = 0;

            // AddComponent(entity, mass);

            // --- 3. Добавляем вспомогательные физические компоненты ---
            // AddComponent<PhysicsVelocity>(entity);
            // AddComponent(entity, new PhysicsDamping
            // {
            //     Linear = 0.01f,
            //     Angular = 0.05f
            // });

            // --- 4. Добавляем пользовательские компоненты игры ---
            AddComponent(entity, new PlayerTag());
            AddComponent(entity, new Movement { MoveSpeed = authoring.moveSpeed, LookSpeed = authoring.lookSpeed });
            AddComponent(entity, new Health { Value = authoring.health });
            // AddComponent(entity, new PhysicsGravityFactor { Value = 1f });
        }
    }
}