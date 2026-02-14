using Unity.Entities;

namespace FPSCore
{
    public struct PickupTag : IComponentData
    {
        public float radius;
    }

    public struct PickedUpTag : IComponentData
    {
        public Entity owner;
    }
    
    public struct TestPickup : IComponentData {}
}