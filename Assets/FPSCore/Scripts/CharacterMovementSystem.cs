using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

namespace FPSCore
{
    /// <summary>
    /// Universal character movement with wall sliding.
    /// Works for any entity with CharacterInput, Movement, PhysicsVelocity, PhysicsCollider.
    /// Detects nearby walls BEFORE physics and pre-projects velocity to slide along them.
    /// </summary>
    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(PhysicsSystemGroup))]
    public partial struct CharacterMovementSystem : ISystem
    {
        private const float InputDeadzone = 0.001f;
        private const float WallQueryDistance = 0.15f;
        private const float MinNormalLength = 0.1f;
        private const float WallDotThreshold = -0.1f;
        private const float SpeedReductionThreshold = 0.9f;
        private const float SlideSpeedMultiplier = 0.8f;
        private const float MinResultSpeed = 0.01f;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsWorldSingleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float dt = SystemAPI.Time.DeltaTime;
            var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld;

            foreach (var (velocityRW, transformRW, movementRO, inputRO, colliderRO, entity) in 
                SystemAPI.Query<
                    RefRW<PhysicsVelocity>, 
                    RefRW<LocalTransform>, 
                    RefRO<Movement>, 
                    RefRO<CharacterInput>,
                    RefRO<PhysicsCollider>>()
                    .WithEntityAccess())
            {
                var velocity = velocityRW.ValueRW;
                var transform = transformRW.ValueRW;
                var movement = movementRO.ValueRO;
                var input = inputRO.ValueRO;

                // =============================
                // 1. Prevent angular velocity
                // =============================
                velocity.Angular = float3.zero;

                // =============================
                // 2. Calculate desired movement direction
                // =============================
                float3 forward = math.forward(transform.Rotation);
                float3 right = math.cross(math.up(), forward);
                
                forward.y = 0;
                right.y = 0;
                forward = math.normalizesafe(forward);
                right = math.normalizesafe(right);

                float3 inputDir = forward * input.Move.y + right * input.Move.x;
                
                float3 desiredVelocity = float3.zero;
                if (math.lengthsq(inputDir) > InputDeadzone)
                {
                    inputDir = math.normalize(inputDir);
                    desiredVelocity = inputDir * movement.MoveSpeed;
                }

                // =============================
                // 3. Wall sliding: project velocity along nearby walls
                // =============================
                float3 finalHorizontalVel = desiredVelocity;
                
                int rigidBodyIndex = physicsWorld.GetRigidBodyIndex(entity);
                if (rigidBodyIndex >= 0 && rigidBodyIndex < physicsWorld.NumBodies)
                {
                    var body = physicsWorld.Bodies[rigidBodyIndex];
                    if (body.Collider.IsCreated && math.lengthsq(desiredVelocity) > InputDeadzone)
                    {
                        CalculateSlideVelocity(
                            ref physicsWorld,
                            in body,
                            rigidBodyIndex,
                            in desiredVelocity,
                            movement.MoveSpeed,
                            out finalHorizontalVel
                        );
                    }
                }

                // =============================
                // 4. Apply velocity (preserve vertical for gravity)
                // =============================
                velocity.Linear = new float3(
                    finalHorizontalVel.x,
                    velocity.Linear.y,
                    finalHorizontalVel.z
                );

                // =============================
                // 5. Rotate character (yaw) based on look input
                // =============================
                float yaw = input.Look.x * movement.LookSpeed * dt;
                transform.Rotation = math.mul(
                    quaternion.RotateY(yaw),
                    transform.Rotation
                );

                velocityRW.ValueRW = velocity;
                transformRW.ValueRW = transform;
            }
        }

        [BurstCompile]
        private static void CalculateSlideVelocity(
            ref PhysicsWorld physicsWorld,
            in RigidBody body,
            int rigidBodyIndex,
            in float3 desiredVelocity,
            float maxSpeed,
            out float3 result)
        {
            float speed = math.length(desiredVelocity);
            
            unsafe
            {
                var distanceInput = new ColliderDistanceInput
                {
                    Collider = (Collider*)body.Collider.GetUnsafePtr(),
                    Transform = body.WorldFromBody,
                    MaxDistance = WallQueryDistance
                };

                NativeList<DistanceHit> hits = new NativeList<DistanceHit>(Allocator.Temp);
                
                if (physicsWorld.CalculateDistance(distanceInput, ref hits))
                {
                    float3 resultVelocity = desiredVelocity;
                    
                    for (int i = 0; i < hits.Length; i++)
                    {
                        var hit = hits[i];
                        
                        if (hit.RigidBodyIndex == rigidBodyIndex)
                            continue;

                        float3 normal = hit.SurfaceNormal;
                        
                        float3 horizontalNormal = new float3(normal.x, 0, normal.z);
                        float normalLen = math.length(horizontalNormal);
                        if (normalLen < MinNormalLength)
                            continue;

                        horizontalNormal /= normalLen;

                        float3 currentMoveDir = math.normalizesafe(resultVelocity);
                        float dot = math.dot(currentMoveDir, horizontalNormal);
                        
                        if (dot >= WallDotThreshold)
                            continue;

                        float proximityFactor = 1f - math.saturate(hit.Distance / WallQueryDistance);
                        
                        float3 intoWall = horizontalNormal * math.dot(resultVelocity, horizontalNormal);
                        float3 projected = resultVelocity - intoWall * proximityFactor;
                        
                        resultVelocity = projected;
                    }
                    
                    float resultSpeed = math.length(new float3(resultVelocity.x, 0, resultVelocity.z));
                    if (resultSpeed > MinResultSpeed && resultSpeed < speed * SpeedReductionThreshold)
                    {
                        resultVelocity = math.normalize(new float3(resultVelocity.x, 0, resultVelocity.z)) * speed * SlideSpeedMultiplier;
                    }
                    
                    hits.Dispose();
                    result = new float3(resultVelocity.x, 0, resultVelocity.z);
                    return;
                }
                
                hits.Dispose();
            }
            
            result = desiredVelocity;
        }
    }
}
