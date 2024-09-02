using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StageUI : MonoBehaviour
{

    private UIDocument _uiDocumunt;
    private VisualElement root;

    [SerializeField] private Sprite mapImage;
    [SerializeField] private Sprite leftArrow;
    [SerializeField] private Sprite rightArrow;
    private bool isLeft = false;

    [SerializeField] private string nextStageName;
    private bool isClear = false;

    private Label resultLabel;
    private VisualElement endingScreen;

    private void Awake()
    {
        _uiDocumunt = GetComponent<UIDocument>();
        root = _uiDocumunt.rootVisualElement;
    }

    private void OnEnable()
    {
        var image = root.Q<VisualElement>("Image");
        var stageImage = image.Q<VisualElement>("StageImage");
        stageImage.style.backgroundImage = new StyleBackground(mapImage);
        var doorButton = image.Q<Button>("door_Btn");

        endingScreen = root.Q<VisualElement>("EndingScreen");
        endingScreen.style.top = new Length(-100f, LengthUnit.Percent);

        resultLabel = endingScreen.Q<Label>("result-Label");
        var leavebtn = endingScreen.Q<Button>("leave-btn");
        var replaybtn = endingScreen.Q<Button>("replay-btn");
        var continuebtn = endingScreen.Q<Button>("continue-btn");

        leavebtn.RegisterCallback<ClickEvent>(evt =>
        {
            SceneManager.LoadScene("Lobby");
        });
        replaybtn.RegisterCallback<ClickEvent>(evt =>
        {
            SceneManager.LoadScene(gameObject.scene.name);
        });
        continuebtn.RegisterCallback<ClickEvent>(evt =>
        {
            if(nextStageName == "" || isClear == false)
            {
                return;
            }
            SceneManager.LoadScene(nextStageName);
        });

        doorButton.style.backgroundImage = new StyleBackground(rightArrow);
        doorButton.RegisterCallback<ClickEvent>(evt =>
        {
            isLeft = !isLeft;
            doorButton.style.backgroundImage = new StyleBackground(isLeft ? leftArrow : rightArrow);
            image.style.right = isLeft ? new Length(-40f, LengthUnit.Percent) : new Length(0f, LengthUnit.Percent);
        });

        image.RegisterCallback<PointerEnterEvent>(evt =>
        {
            GameManager.Instance.GameInput.UIInputEnable();
        });
        image.RegisterCallback<PointerLeaveEvent>(evt =>
        {
            GameManager.Instance.GameInput.GameInputEnable();
        });


    }

    public void Ending(string result, bool clear)
    {
        resultLabel.text = result;

        endingScreen.style.top = new Length(0f, LengthUnit.Percent);
        isClear = clear;
    }

}
