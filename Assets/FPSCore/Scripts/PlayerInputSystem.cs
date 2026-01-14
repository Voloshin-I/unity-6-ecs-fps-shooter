using Unity.Burst;
using Unity.Entities;

namespace FPSCore
{
    /// <summary>
    /// Copies global InputState (from InputProxy) to player's CharacterInput.
    /// Runs before CharacterMovementSystem.
    /// </summary>
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(FixedStepSimulationSystemGroup))]
    public partial struct PlayerInputSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.TryGetSingleton<InputState>(out var globalInput))
                return;

            foreach (var inputRW in SystemAPI.Query<RefRW<CharacterInput>>().WithAll<PlayerTag>())
            {
                inputRW.ValueRW = new CharacterInput
                {
                    Move = globalInput.Move,
                    Look = globalInput.Look,
                    Jump = false, // Add jump input when needed
                    PrimaryAction = globalInput.Fire,
                    SecondaryAction = false
                };
            }
        }
    }
}



