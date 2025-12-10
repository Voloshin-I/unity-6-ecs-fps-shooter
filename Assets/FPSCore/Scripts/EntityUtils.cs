using Unity.Entities;
using Unity.Collections;

namespace FPSCore
{
   public static class EntityUtils
    {
        /// <summary>
        /// Ищет первую сущность, содержащую компонент типа T.
        /// Возвращает true, если такая сущность найдена.
        /// </summary>
        public static bool TryFindEntityWith<T>(this World world, out Entity entity) where T : unmanaged, IComponentData
        {
            var entityManager = world.EntityManager;

            // Создаём временный запрос
            using var query = entityManager.CreateEntityQuery(ComponentType.ReadOnly<T>());

            // Проверка — есть ли хотя бы одна сущность
            if (query.CalculateEntityCount() == 0)
            {
                entity = Entity.Null;
                return false;
            }

            // Получаем первый Chunk и первую Entity из него
            using var chunks = query.ToArchetypeChunkArray(Allocator.Temp);
            var entityType = entityManager.GetEntityTypeHandle();

            var entities = chunks[0].GetNativeArray(entityType);
            entity = entities.Length > 0 ? entities[0] : Entity.Null;

            return entity != Entity.Null;
        }
    }
}