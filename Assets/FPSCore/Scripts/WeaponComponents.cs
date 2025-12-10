using Unity.Entities;
using Unity.Mathematics;

namespace FPSCore
{
    public struct WeaponTag : IComponentData {}

    public struct WeaponHolder : IComponentData
    {
        public Entity WeaponEntity; // Entity.Null when empty
    }
    
    // Компонент оружия
    public struct Weapon : IComponentData
    {
        //public Entity WeaponEntity;
        public float FireRate;
        public float Damage;
        public float LastFireTime;
        public bool IsShooting;
        
        // TODO: raycast/projectile is ammo behaviour
        public float Range;
    }
    
    //public struct 
}