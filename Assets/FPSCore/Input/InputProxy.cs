using Unity.Entities;
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
                    inputState.move = move;
                }
                if (lookAction != null)
                    inputState.look = lookAction.ReadValue<Vector2>();
                if (fireAction != null)
                    inputState.fire = fireAction.WasPressedThisFrame();
                if (pickupAction != null)
                    inputState.pickup = pickupAction.WasPressedThisFrame();
                if (dropAction != null)
                    inputState.drop = dropAction.WasPressedThisFrame();
            }
            else
            {
                inputState.move = new Vector2(Input.GetAxisRaw(LegacyHorizontalAxis), Input.GetAxisRaw(LegacyVerticalAxis));
                inputState.look = new Vector2(Input.GetAxis(LegacyMouseXAxis), Input.GetAxis(LegacyMouseYAxis));
                inputState.fire = Input.GetMouseButtonDown(0);
                inputState.pickup = Input.GetKeyDown(KeyCode.E);
                inputState.drop = Input.GetKeyDown(KeyCode.G);
            }
            
            _entityManager.SetComponentData(_inputEntity, inputState);
        }
    }
}
