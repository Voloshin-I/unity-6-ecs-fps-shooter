using Unity.Entities;
using Unity.Collections;

namespace FPSCore
{
    public static class EntityUtils
    {
        /// <summary>
        /// Finds the first entity containing component of type T.
        /// Returns true if such entity is found.
        /// </summary>
        public static bool TryFindEntityWith<T>(this World world, out Entity entity) where T : unmanaged, IComponentData
        {
            entity = Entity.Null;
            var entityManager = world.EntityManager;

            using var query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<T>());

            if (query.CalculateEntityCount() == 0)
                return false;

            using var chunks = query.ToArchetypeChunkArray(Allocator.Temp);
            var entityType = entityManager.GetEntityTypeHandle();

            var entities = chunks[0].GetNativeArray(entityType);
            if (entities.Length == 0)
                return false;

            entity = entities[0];
            return true;
        }
    }
}
