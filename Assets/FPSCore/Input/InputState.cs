using Unity.Entities;
using Unity.Mathematics;

namespace FPSCore
{
    public struct InputState : IComponentData
    {
        public float2 move;
        public float2 look;
        public bool fire;
        public bool pickup;
        public bool drop;
    }
}
