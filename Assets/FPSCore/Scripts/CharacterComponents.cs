using Unity.Entities;
using Unity.Mathematics;

namespace FPSCore
{
    public struct PlayerTag : IComponentData {}
    
    public struct EnemyTag : IComponentData {}

    public struct Health : IComponentData
    {
        public float Value;
        public float MaxValue;
    }

    public struct Movement : IComponentData
    {
        public float MoveSpeed;
        public float LookSpeed;
        public float3 Velocity;
        public bool IsGrounded;
    }
}