using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Karin.InputSystem;

namespace Karin
{

    [CreateAssetMenu(menuName = "Karin/InputReader")]
    public class InputReaderSO : ScriptableObject, IInGameActions
    {
        public event Action<Vector2> CameraMovementEvent;
        public event Action<Vector2> InterectionEvent;
        public event Action<Vector2> SelectEvent;
        public event Action EscEvent;

        private InputSystem _inputSystem;
        public InputSystem InputSystem => _inputSystem;

        private Camera _mainCam;

        private void OnEnable()
        {
            _mainCam = Camera.main;

            if (_inputSystem == null)
            {
                _inputSystem = new InputSystem();
                _inputSystem.InGame.SetCallbacks(this);
            }

            _inputSystem.InGame.Enable();
        }

        public void SwapActionInGame()
        {
            _inputSystem.InGame.Enable();
        }
        public void SwapActionUI()
        {
            _inputSystem.InGame.Enable();
        }

        public void OnCameraMovement(InputAction.CallbackContext context)
        {
            if (context.performed == true)
            {
                Vector2 value = context.ReadValue<Vector2>();
                CameraMovementEvent?.Invoke(value);
            }
            if (context.canceled == true)
            {
                CameraMovementEvent?.Invoke(Vector2.zero);
            }
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            if (context.performed == true)
            {
                if(_mainCam == null) _mainCam = Camera.main;
                Vector3 mPos = Input.mousePosition;
                Vector3 world = _mainCam.ScreenToWorldPoint(mPos);
                world.z = 0;
                InterectionEvent?.Invoke(world);
            }
        }

        public void OnCharacterSelect(InputAction.CallbackContext context)
        {
            if (context.performed == true)
            {
                if (_mainCam == null) _mainCam = Camera.main;
                Vector3 mPos = Input.mousePosition;
                Vector3 world = _mainCam.ScreenToWorldPoint(mPos);
                world.z = 0;
                SelectEvent?.Invoke(world);
            }
        }

        public void OnCancle(InputAction.CallbackContext context)
        {
            if (context.performed == true)
            {
                EscEvent?.Invoke();
            }
        }
    }
}
