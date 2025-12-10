using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace FPSCore
{
    public class CameraTransformController : MonoBehaviour
    {
        private EntityManager entityManager;
        private Entity cameraEntity;
        private bool found;

        void Start()
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        void LateUpdate()
        {
            if (!found)
            {
                if (EntityUtils.TryFindEntityWith<CameraTag>(World.DefaultGameObjectInjectionWorld, out cameraEntity))
                {
                    Debug.Log($"Нашёл игрока: {cameraEntity.Index}");
                    found = true;
                }
                else
                {
                    Debug.Log($"Не нашёл игрока");
                    found = false;
                    return;
                }
            }
            
            // if (entityManager.HasComponent<LocalTransform>(cameraEntity))
            // {
            //     var t = entityManager.GetComponentData<LocalTransform>(cameraEntity);
            //     transform.position = t.Position;
            //     transform.rotation = t.Rotation;
            // }
            
            if (entityManager.HasComponent<LocalToWorld>(cameraEntity))
            {
                var t = entityManager.GetComponentData<LocalToWorld>(cameraEntity);
                transform.position = t.Position;
                transform.rotation = t.Rotation;
            }
        }
    }

}