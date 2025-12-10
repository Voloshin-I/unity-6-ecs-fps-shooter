using Unity.Entities;
using UnityEngine;

namespace FPSCore
{
    public class CameraAnchorAuthoring : MonoBehaviour
    {
        // public override void BakeEntity(Entity entity, AuthoringBaker baker)
        // {
        //     //baker.AddComponent(entity, new CameraTag());
        //     baker.AddComponent(new CameraTag());
        // }
    }
    
    public class CameraBaker : Baker<CameraAnchorAuthoring>
    {
        public override void Bake(CameraAnchorAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CameraTag());
        }
    }
    
    public struct CameraTag : IComponentData {}
}
