using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Temp : MonoBehaviour
{
    private PlayerInputAction action;

    // Start is called before the first frame update
    void Start()
    {
        action = new PlayerInputAction();
        action.PlayerAction.Enable();

        var leftMouseClick = new InputAction(binding: "<Mouse>/leftButton");
        leftMouseClick.performed += ctx => LeftMouseClicked();
        leftMouseClick.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Debug.Log("q");
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Inputsystem Ŭ��");
        }
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("InputManager Ŭ��");
        }
    }

    private void LeftMouseClicked()
    {
        Debug.Log("Ŭ��");
    }
}
