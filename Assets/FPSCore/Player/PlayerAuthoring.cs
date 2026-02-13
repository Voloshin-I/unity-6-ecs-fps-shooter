using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using FPSCore.Movement;

namespace FPSCore
{
    /// <summary>
    /// Player authoring for FPS character.
    /// Creates physics capsule, freezes all rotation, adds gameplay components.
    /// For NPCs, create a similar authoring without PlayerTag.
    /// </summary>
    public class PlayerAuthoring : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _lookSpeed = 2f;

        [Header("Physics")]
        [SerializeField] private float _mass = 1f;
        [SerializeField] private float _radius = 0.5f;
        [SerializeField] private float _height = 2f;

        public float MoveSpeed => _moveSpeed;
        public float LookSpeed => _lookSpeed;
        public float Mass => _mass;
        public float Radius => _radius;
        public float Height => _height;
    }

    public class PlayerBaker : Baker<PlayerAuthoring>
    {
        private const float LinearDamping = 0.1f;
        private const float AngularDamping = 0f;
        private const float GravityFactor = 1f;

        public override void Bake(PlayerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            // =============================
            // 1. Create capsule collider (centered, like Unity's capsule mesh)
            // =============================
            PhysicsCollider collider = CreateCapsuleCollider(authoring);
            AddComponent(entity, collider);

            // =============================
            // 2. Create dynamic physics mass with ALL rotation frozen
            // =============================
            var physicsMass = PhysicsMass.CreateDynamic(collider.MassProperties, authoring.Mass);
            physicsMass.InverseInertia = float3.zero;
            AddComponent(entity, physicsMass);

            // =============================
            // 3. Add required physics components
            // =============================
            AddComponent(entity, new PhysicsVelocity());
            AddComponent(entity, new PhysicsGravityFactor { Value = GravityFactor });
            AddComponent(entity, new PhysicsDamping { Linear = LinearDamping, Angular = AngularDamping });
            AddSharedComponent(entity, new PhysicsWorldIndex());

            // =============================
            // 4. Add movement components (from Movement plugin)
            // =============================
            AddComponent(entity, new MovementInput());
            AddComponent(entity, new MovementData 
            { 
                moveSpeed = authoring.MoveSpeed, 
                lookSpeed = authoring.LookSpeed 
            });

            // =============================
            // 5. Add player-specific tag
            // =============================
            AddComponent(entity, new PlayerTag());
        }

        private PhysicsCollider CreateCapsuleCollider(PlayerAuthoring authoring)
        {
            float halfHeight = authoring.Height * .5f;
            float vertexOffset = halfHeight - authoring.Radius;

            var capsuleGeometry = new CapsuleGeometry
            {
                Vertex0 = new float3(0, -vertexOffset, 0),
                Vertex1 = new float3(0, vertexOffset, 0),
                Radius = authoring.Radius
            };

            var filter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = ~0u,
                GroupIndex = 0
            };

            var collider = Unity.Physics.CapsuleCollider.Create(capsuleGeometry, filter);
            PhysicsCollider result = new PhysicsCollider { Value = collider };
            return result;
        }
    }
}
