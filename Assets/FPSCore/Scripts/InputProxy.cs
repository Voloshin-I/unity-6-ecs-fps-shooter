using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FPSCore
{
    [DisallowMultipleComponent]
    public class InputProxy : MonoBehaviour
    {
        public InputActionAsset actions; // assign the InputActions asset
        public string mapName = "Player";
        
        InputActionMap map;

        void OnEnable()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            // create InputState
            EntityQuery query = _entityManager.CreateEntityQuery(typeof(InputState));
            if (query.IsEmptyIgnoreFilter)
            {
                _inputEntity = _entityManager.CreateEntity(typeof(InputState));
                _entityManager.SetComponentData(_inputEntity, new InputState());
            }
            else
            {
                _inputEntity = query.GetSingletonEntity();
            }

            // initialize input actions map
            if (actions != null)
            {
                map = actions.FindActionMap(mapName);
                if (map != null)
                    map.Enable();
            }
        }

        void OnDisable()
        {
            if (map != null) map.Disable();
        }

        void Update()
        {
            if (_inputEntity == Entity.Null) return;
            InputState inputState = _entityManager.GetComponentData<InputState>(_inputEntity);
            
            if (map != null)
            {
                InputAction moveAction = map.FindAction("Move");
                InputAction lookAction = map.FindAction("Look");
                InputAction fireAction = map.FindAction("Fire");
                InputAction pickupAction = map.FindAction("Pickup");
                InputAction dropAction = map.FindAction("Drop");

                if (moveAction != null)
                {
                    Vector2 move = moveAction.ReadValue<Vector2>();
                    move.x *= -1;
                    inputState.Move = move;
                }
                if (lookAction != null)
                    inputState.Look = lookAction.ReadValue<Vector2>();
                if (fireAction != null)
                    inputState.Fire = fireAction.WasPressedThisFrame();
                if (pickupAction != null)
                    inputState.Pickup = pickupAction.WasPressedThisFrame();
                if (dropAction != null)
                    inputState.Drop = dropAction.WasPressedThisFrame();
            }
            else
            {
// fallback to old Input (useful for quick testing)
                inputState.Move = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
                inputState.Look = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                inputState.Fire = Input.GetMouseButtonDown(0);
                inputState.Pickup = Input.GetKeyDown(KeyCode.E);
                inputState.Drop = Input.GetKeyDown(KeyCode.G);
            }
            
            _entityManager.SetComponentData(_inputEntity, inputState);
        }

        private EntityManager _entityManager;
        private Entity _inputEntity;
    }
    
    public struct InputState : IComponentData
    {
        public float2 Move; // x = forward/back, y = strafe
        public float2 Look; // x = yaw, y = pitch
        public bool Fire;
        public bool Pickup;
        public bool Drop;
    }
}