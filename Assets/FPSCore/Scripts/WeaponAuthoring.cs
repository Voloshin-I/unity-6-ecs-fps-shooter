using Unity.Entities;
using UnityEngine;

namespace FPSCore
{
    public class WeaponAuthoring : MonoBehaviour
    {
        public float range = 100f;
        public int damage = 1;
    }

    public class WeaponBaker : Baker<WeaponAuthoring>
    {
        public override void Bake(WeaponAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Weapon { Range = authoring.range, Damage = authoring.damage });
            // Tag so systems can find loose weapon entities in the world
            AddComponent(entity, new WeaponTag());
        }
    }
}