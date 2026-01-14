using Unity.Entities;
using UnityEngine;

namespace FPSCore
{
    public class EnemyAuthoring : MonoBehaviour
    {
        [SerializeField] private int _health = 10;

        public int Health => _health;
    }

    public class EnemyBaker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyTag());
            AddComponent(entity, new Health { Value = authoring.Health });
        }
    }
}
