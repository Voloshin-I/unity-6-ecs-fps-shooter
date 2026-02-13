using Unity.Entities;
using UnityEngine;

namespace FPSCore.Weapons
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
            AddComponent(entity, new WeaponTag());
        }
    }
}
