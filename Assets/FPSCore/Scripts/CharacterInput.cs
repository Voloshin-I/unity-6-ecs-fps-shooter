using Unity.Entities;
using Unity.Mathematics;

namespace FPSCore
{
    /// <summary>
    /// Per-entity input component for any character (player or NPC).
    /// Movement system reads this to move the character.
    /// For players: filled by PlayerInputSystem from global InputState.
    /// For NPCs: filled by AI systems.
    /// </summary>
    public struct CharacterInput : IComponentData
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
        
        /// <summary>
        /// Jump requested this frame.
        /// </summary>
        public bool Jump;
        
        /// <summary>
        /// Primary action (fire/attack) requested.
        /// </summary>
        public bool PrimaryAction;
        
        /// <summary>
        /// Secondary action (aim/block) requested.
        /// </summary>
        public bool SecondaryAction;
    }
}



