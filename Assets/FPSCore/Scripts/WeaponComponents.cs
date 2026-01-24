using Unity.Entities;

namespace FPSCore
{
    public struct WeaponTag : IComponentData {}

    public struct Weapon : IComponentData
    {
        public float FireRate;
        public float Damage;
        public float LastFireTime;
        public bool IsShooting;
        public float Range;
    }
}