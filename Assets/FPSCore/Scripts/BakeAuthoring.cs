// using Unity.Entities;
// using UnityEngine;
//
// namespace FPSCore
// {
//     public class BakeAuthoring : MonoBehaviour, IBakeAuthoring
//     {
//         public virtual void BakeEntity(Entity entity, AuthoringBaker baker) {}
//     }
//
//     public interface IBakeAuthoring
//     {
//         void BakeEntity(Entity entity, AuthoringBaker baker);
//     }
//
//     public class AuthoringBaker : Baker<T> where T : MonoBehaviour, IBakeAuthoring
//     {
//         public override void Bake(BakeAuthoring authoring)
//         {
//             _entity = GetEntity(TransformUsageFlags.Dynamic);
//             authoring.BakeEntity(_entity, this);
//         }
//
//         public new void AddComponent<TComponent>(in TComponent component) where TComponent : unmanaged, IComponentData
//         {
//             AddComponent(_entity, component);
//         }
//
//         private Entity _entity;
//     }
// }
//
