using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace FPSCore
{
    /// <summary>
    /// Synchronizes a GameObject visual with the Player ECS entity.
    /// Uses smooth following to avoid jitter from physics tick mismatch.
    /// </summary>
    public class PlayerView : MonoBehaviour
    {
        private void Start()
        {
            if (World.DefaultGameObjectInjectionWorld != null)
                _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        private void Update()
        {
            Initialize();
            UpdateTransform();
        }

        private void LateUpdate()
        {
        }

        private void Initialize()
        {
            if (_initialized)
                return;

            if (World.DefaultGameObjectInjectionWorld != null)
            {
                _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            }

            if (_playerEntity == Entity.Null)
            {
                EntityUtils.TryFindEntityWith<PlayerTag>(
                    World.DefaultGameObjectInjectionWorld, out _playerEntity);
            }
            
            if (!_entityManager.Exists(_playerEntity))
            {
                _playerEntity = Entity.Null;
            }

            if (World.DefaultGameObjectInjectionWorld != null
                && _playerEntity != Entity.Null)
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
