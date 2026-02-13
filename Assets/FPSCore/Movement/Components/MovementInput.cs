using Unity.Entities;
using Unity.Mathematics;

namespace FPSCore.Movement
{
    /// <summary>
    /// Per-entity input component for movement.
    /// For players: filled by PlayerMovementInputSystem from global InputState.
    /// For NPCs: filled by AI systems.
    /// </summary>
    public struct MovementInput : IComponentData
    {
        /// <summary>
        /// Movement input. X = strafe (left/right), Y = forward/back.
        /// Values should be in range [-1, 1].
        /// </summary>
        public float2 Move;
        
        /// <summary>
        /// Look input. X = yaw (horizontal), Y = pitch (vertical).
        /// </summary>
        public float2 Look;
    }
}
