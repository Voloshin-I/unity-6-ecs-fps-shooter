using Unity.Burst;
using Unity.Entities;

namespace FPSCore.Movement
{
    /// <summary>
    /// Copies global InputState (from InputProxy) to player's MovementInput.
    /// Runs before CharacterMovementSystem.
    /// </summary>
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(FixedStepSimulationSystemGroup))]
    public partial struct PlayerMovementInputSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.TryGetSingleton<InputState>(out var globalInput))
                return;

            foreach (var inputRW in SystemAPI.Query<RefRW<MovementInput>>().WithAll<PlayerTag>())
            {
                inputRW.ValueRW = new MovementInput
                {
                    Move = globalInput.move,
                    Look = globalInput.look
                };
            }
        }
    }
}
