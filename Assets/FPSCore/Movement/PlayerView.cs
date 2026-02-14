using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace FPSCore.Movement
{
    /// <summary>
    /// Synchronizes a GameObject visual with the Player ECS entity.
    /// Uses smooth following to avoid jitter from physics tick mismatch.
    /// </summary>
    public class PlayerView : MonoBehaviour
    {
        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            Initialize();
            if (_initialized)
                UpdateTransform();
        }

        // TODO: check if can move to separate one-shot system
        private void Initialize()
        {
            if (_initialized)
                return;
            
            if (World.DefaultGameObjectInjectionWorld == null)
                return;
            
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery query = _entityManager.CreateEntityQuery(typeof(PlayerTag));
            if (query.IsEmptyIgnoreFilter)
                return;
            
            _playerEntity = query.GetSingletonEntity();
            if (_playerEntity != Entity.Null)
            {
                _initialized = true;
            }
        }
        
        private void UpdateTransform()
        {
            var ltw = _entityManager.GetComponentData<LocalToWorld>(_playerEntity);
            transform.position = ltw.Position;
            transform.rotation = ltw.Rotation;
        }

        private Entity _playerEntity;
        private EntityManager _entityManager;
        private bool _initialized;
    }
}
