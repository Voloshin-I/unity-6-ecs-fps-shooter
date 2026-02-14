using Unity.Entities;
using UnityEngine;

namespace FPSCore
{
    // public class TestPickupAuthoring : PickupAuthoringBase
    // {
    //     public override void BakePickup(Entity entity, PickupBaker baker)
    //     {
    //         Debug.Log("Baking test Pickup");
    //         baker.AddComponent<TestPickup>(entity);
    //     }
    // }
    
    public class TestPickupAuthoring: MonoBehaviour
    {
        public float radius = 0.5f;
    }

    public class TestPickupBaker: Baker<TestPickupAuthoring>
    {
        public override void Bake(TestPickupAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<TestPickup>(entity);
            AddComponent(entity, new PickupTag()
            {
                radius = authoring.radius
            });
            
            authoring.transform.LinkChildren(this);
        }
    }
}