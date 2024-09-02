using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[Flags]
public enum Actions : int
{
    InGame = 1,
    UI = 2,
}

[CreateAssetMenu(menuName = "SO/InputReader")]
public class InputReader : ScriptableObject, IInGameActions, IUIActions
{
    #region InGame
    public event Action OnAttackEvent;
    public event Action OnAvoidanceEvent;
    public event Action OnInteractionEvent;

    public event Action OnSkill1Event;
    public event Action OnSkill2Event;
    public event Action OnSkill3Event;
    public event Action OnSkill4Event;

    public Vector2 movement = Vector2.zero;
    #endregion

    #region UI
    public event Action OnMLBDownEvent;
    public event Action OnMLBUpEvent;
    public event Action<bool> OnMLBHoldEvent;

    public event Action<bool> OnLCtrlEvent;
    #endregion

    public Controls control;

    private void OnEnable()
    {
        if (control == null)
        {
            control = new Controls();

            control.InGame.SetCallbacks(this);
            control.UI.SetCallbacks(this);
        }
        //control.InGame.Enable();
        //control.UI.Enable();
        ChangeAction((int)(Actions.InGame | Actions.UI));
    } 

    /// <summary>
    /// Actions Enum을 가져와서 |(or) 연산으로 넣으면 여래개가 같이 켜짐
    ///  ex) ChangeAction((int)(Actions.InGame | Actions.UI)); => InGame : On, UI : On
    /// </summary>
    /// <param name="action"> Enum:Actions를 Int로 형변환해 넣으면 됨</param>
    public void ChangeAction(int action)
    {

        if ((action & (int)Actions.InGame) > 0)
        {
            control.InGame.Enable();
        }
        else
        {
            control.InGame.Disable();
        }
        if ((action & (int)Actions.UI) > 0)
        {
            control.UI.Enable();
        }
        else
        {
            control.UI.Disable();
        }

    }

    #region InGame
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnAttackEvent?.Invoke();
        }
    }

    public void OnAvoidance(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnAvoidanceEvent?.Invoke();
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnInteractionEvent?.Invoke();
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movement = context.ReadValue<Vector2>();
        }
        if (context.canceled)
        {
            movement = Vector2.zero;
        }
    }

    public void OnSkill1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnSkill1Event?.Invoke();
        }
    }

    public void OnSkill2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnSkill2Event?.Invoke();
        }
    }

    public void OnSkill3(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnSkill3Event?.Invoke();
        }
    }

    public void OnSkill4(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnSkill4Event?.Invoke();
        }
    }
    #endregion

    public void OnLCtrl(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnLCtrlEvent?.Invoke(true);
        }
        else
        {
            OnLCtrlEvent?.Invoke(false);
        }
    }

    public void OnMLB(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnMLBDownEvent?.Invoke();
            OnMLBHoldEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            OnMLBHoldEvent?.Invoke(false);
            OnMLBUpEvent?.Invoke();
        }
    }
}
