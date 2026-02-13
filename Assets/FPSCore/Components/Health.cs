using Unity.Entities;

namespace FPSCore
{
    public struct Health : IComponentData
    {
        public float value;
        public float maxValue;
    }
}
