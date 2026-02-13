using Unity.Entities;
using Unity.Mathematics;

namespace FPSCore.Movement
{
    /// <summary>
    /// Movement configuration and state for a character.
    /// </summary>
    public struct MovementData : IComponentData
    {
        public float moveSpeed;
        public float lookSpeed;
        public float3 velocity;
        public bool isGrounded;
    }
}
