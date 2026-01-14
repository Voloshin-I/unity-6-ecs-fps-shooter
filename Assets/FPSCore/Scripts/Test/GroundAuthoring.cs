using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;

namespace FPSCore.Test
{
    public class GroundAuthoring : MonoBehaviour
    {
        [SerializeField] private float _width = 10f;
        [SerializeField] private float _length = 10f;
        [SerializeField] private float _thickness = 0.1f;

        public float Width => _width;
        public float Length => _length;
        public float Thickness => _thickness;
    }
    
    public class GroundBaker : Baker<GroundAuthoring>
    {
        private const float BevelRadius = 0.01f;

        public override void Bake(GroundAuthoring authoring)
        {
            // Use Renderable for static physics bodies
            var entity = GetEntity(TransformUsageFlags.Renderable);

            // Create BoxCollider
            var boxGeometry = new BoxGeometry
            {
                Center = float3.zero,
                Size = new float3(authoring.Width, authoring.Thickness, authoring.Length),
                Orientation = quaternion.identity,
                BevelRadius = BevelRadius
            };

            var filter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = ~0u,
                GroupIndex = 0
            };

            var collider = Unity.Physics.BoxCollider.Create(boxGeometry, filter);

            // Add PhysicsCollider - this is all that's needed for a static body
            AddComponent(entity, new PhysicsCollider { Value = collider });
            
            // Add physics world index for proper simulation
            AddSharedComponent(entity, new PhysicsWorldIndex());
        }
    }
}
