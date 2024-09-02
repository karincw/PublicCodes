using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.GameInput.TutorialInputEnable();
    }
}
