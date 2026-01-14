using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FPSCore
{
    [DisallowMultipleComponent]
    public class InputProxy : MonoBehaviour
    {
        [SerializeField] private InputActionAsset _actions;
        [SerializeField] private string _mapName = "Player";

        private const string MoveActionName = "Move";
        private const string LookActionName = "Look";
        private const string FireActionName = "Fire";
        private const string PickupActionName = "Pickup";
        private const string DropActionName = "Drop";
        private const string LegacyVerticalAxis = "Vertical";
        private const string LegacyHorizontalAxis = "Horizontal";
        private const string LegacyMouseXAxis = "Mouse X";
        private const string LegacyMouseYAxis = "Mouse Y";

        private InputActionMap _map;
        private EntityManager _entityManager;
        private Entity _inputEntity;

        private void OnEnable()
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
            if (_actions != null)
            {
                _map = _actions.FindActionMap(_mapName);
                if (_map != null)
                    _map.Enable();
            }
        }

        private void OnDisable()
        {
            if (_map != null) _map.Disable();
        }

        private void Update()
        {
            if (_inputEntity == Entity.Null) return;
            InputState inputState = _entityManager.GetComponentData<InputState>(_inputEntity);
            
            if (_map != null)
            {
                InputAction moveAction = _map.FindAction(MoveActionName);
                InputAction lookAction = _map.FindAction(LookActionName);
                InputAction fireAction = _map.FindAction(FireActionName);
                InputAction pickupAction = _map.FindAction(PickupActionName);
                InputAction dropAction = _map.FindAction(DropActionName);

                if (moveAction != null)
                {
                    Vector2 move = moveAction.ReadValue<Vector2>();
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
                //inputState.Move = new Vector2(Input.GetAxisRaw(LegacyVerticalAxis), Input.GetAxisRaw(LegacyHorizontalAxis));                inputState.Move = new Vector2(Input.GetAxisRaw(LegacyVerticalAxis), Input.GetAxisRaw(LegacyHorizontalAxis));
                inputState.Move = new Vector2(Input.GetAxisRaw(LegacyHorizontalAxis), Input.GetAxisRaw(LegacyVerticalAxis));
                inputState.Look = new Vector2(Input.GetAxis(LegacyMouseXAxis), Input.GetAxis(LegacyMouseYAxis));
                inputState.Fire = Input.GetMouseButtonDown(0);
                inputState.Pickup = Input.GetKeyDown(KeyCode.E);
                inputState.Drop = Input.GetKeyDown(KeyCode.G);
            }
            
            _entityManager.SetComponentData(_inputEntity, inputState);
        }
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
