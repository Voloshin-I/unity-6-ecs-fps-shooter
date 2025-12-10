using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace FPSCore
{
    [BurstCompile]
    public partial struct PlayerMovementSystem : ISystem
    {
        public void OnCreate(ref SystemState state) {}


        public void OnUpdate(ref SystemState state)
        {
            
            
            if (!SystemAPI.TryGetSingleton<InputState>(out var input)) return;

            float dt = SystemAPI.Time.DeltaTime;

// Move & Look for player
            foreach ((RefRW<PhysicsVelocity> physicsVelocityRW,
                         RefRW<LocalTransform> localTransformRW,
                         RefRO<Movement> movementRO,
                         Entity entity)
                     in SystemAPI.Query<RefRW<PhysicsVelocity>, RefRW<LocalTransform>, RefRO<Movement>>()
                         .WithAll<PlayerTag>().WithEntityAccess())
            {
                PhysicsVelocity physicsVelocity = physicsVelocityRW.ValueRW;
                LocalTransform localTransform = localTransformRW.ValueRW;
                Movement movement = movementRO.ValueRO;
                
// movement (local forward/right)
                var s = movement.MoveSpeed;
                float2 move = input.Move; // x,y
                


// move.x = forward/back, move.y = strafe -> convert to world dir
                float3 forward = math.forward(localTransform.Rotation);
                float3 right = math.cross(forward, new float3(0,1,0));

                float3 moveDirection = right * move.x + forward * move.y;
                float3 delta = moveDirection * s * dt;
#if UNITY_EDITOR
                // bool3 isMove = delta != float3.zero;
                // if (math.distancesq(delta, float3.zero) > 0.001f)
                // {
                //     UnityEngine.Debug.LogError($"right:{right}, move x:{move.x}");
                //     UnityEngine.Debug.Log($"Movement: {delta}");
                // }
#endif

                //var lt = transform.ValueRO;
                //lt.Position += delta;
                float3 movementLinear = moveDirection * movement.MoveSpeed;
                physicsVelocity.Linear = new float3(movementLinear.x, movementLinear.y, physicsVelocity.Linear.z);


// look: apply yaw and pitch (simple yaw only for FPS horizontal look)
                float yaw = input.Look.x * movement.LookSpeed;
                quaternion rot = math.mul(quaternion.EulerYXZ(new float3(0, yaw * dt, 0)), localTransform.Rotation);
                localTransform.Rotation = rot;

// save changes in entity components
                localTransformRW.ValueRW = localTransform;
                physicsVelocityRW.ValueRW = physicsVelocity;
            }
        }
    }
}