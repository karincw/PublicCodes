using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameInput;

public enum ActionMaps
{
    CameraMove,
    UI,
}
[CreateAssetMenu(menuName = "Karin/SO/InputReader")]
public class InputReader : ScriptableObject, ICameraMoveActions
{
    public event Action<Vector2> CameraMovementEvent;
    public event Action<Vector2> CameraScrollEvent;
    public event Action<bool> SelectEvent;
    public event Action<bool> UnSelectEvent;

    private GameInput _inputSystem;
    public GameInput InputSystem => _inputSystem;

    private void OnEnable()
    {
        if (_inputSystem == null)
        {
            _inputSystem = new GameInput();
            _inputSystem.CameraMove.SetCallbacks(this);
        }

        _inputSystem.CameraMove.Enable();
    }

    public void ChangeActionMap(ActionMaps map)
    {
        switch (map)
        {
            case ActionMaps.CameraMove:

                _inputSystem.UI.Disable();
                _inputSystem.CameraMove.Enable();

                break;
            case ActionMaps.UI:

                _inputSystem.UI.Enable();
                _inputSystem.CameraMove.Disable();

                break;
        }
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        CameraMovementEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (context.performed == true)
        {
            SelectEvent?.Invoke(true);
        }
        else if (context.canceled == true)
        {
            SelectEvent?.Invoke(false);
        }
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        CameraScrollEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnSelectCancel(InputAction.CallbackContext context)
    {
        if (context.performed == true)
        {
            UnSelectEvent?.Invoke(true);
        }
        else if (context.canceled == true)
        {
            UnSelectEvent?.Invoke(false);
        }
    }
}
