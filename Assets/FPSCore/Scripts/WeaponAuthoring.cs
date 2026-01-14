using Unity.Entities;
using UnityEngine;

namespace FPSCore
{
    public class WeaponAuthoring : MonoBehaviour
    {
        [SerializeField] private float _range = 100f;
        [SerializeField] private int _damage = 1;

        public float Range => _range;
        public int Damage => _damage;
    }

    public class WeaponBaker : Baker<WeaponAuthoring>
    {
        public override void Bake(WeaponAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Weapon { Range = authoring.Range, Damage = authoring.Damage });
            // Tag so systems can find loose weapon entities in the world
            AddComponent(entity, new WeaponTag());
        }
    }
}
