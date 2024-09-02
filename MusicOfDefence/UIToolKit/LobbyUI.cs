using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] UIDocument _uiDocument;

    [SerializeField] string createSceneName;
    [SerializeField] string stageSceneName;

    private void OnEnable()
    {
        var root = _uiDocument.rootVisualElement;

        var leaveButton = root.Q<Button>("leave-button");
        leaveButton.RegisterCallback<ClickEvent>(evt =>
        {
            Application.Quit();
        });

        var CreateButton = root.Q<Button>("createMode-button");
        CreateButton.RegisterCallback<ClickEvent>(evt =>
        {
            SceneManager.LoadScene(createSceneName);
        });

        var StageButton = root.Q<Button>("stageMode-button");
        StageButton.RegisterCallback<ClickEvent>(evt =>
        {
            SceneManager.LoadScene(stageSceneName);
        });
    }
}
