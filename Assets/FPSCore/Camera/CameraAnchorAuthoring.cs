using Unity.Entities;
using UnityEngine;

namespace FPSCore.Camera
{
    public class CameraAnchorAuthoring : MonoBehaviour { }
    
    public class CameraBaker : Baker<CameraAnchorAuthoring>
    {
        public override void Bake(CameraAnchorAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CameraAnchorTag());
        }
    }
    
    public struct CameraAnchorTag : IComponentData { }
}
