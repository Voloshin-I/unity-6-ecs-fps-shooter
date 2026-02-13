using Unity.Entities;

namespace FPSCore.Weapons
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
