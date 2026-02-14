using Unity.Entities;
using UnityEngine;

namespace FPSCore
{
    public abstract class PickupAuthoringBase: MonoBehaviour
    {
        public float radius = 0.5f;
        public abstract void BakePickup(Entity entity, PickupBaker baker);
    }

    public class PickupBaker: Baker<PickupAuthoringBase>
    {
        public override void Bake(PickupAuthoringBase authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new PickupTag()
            {
                radius = authoring.radius
            });
            authoring.BakePickup(entity, this);
        }
    }
}
