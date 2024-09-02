using CW;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    [SerializeField] private GameObject[] panels = new GameObject[0];
    [SerializeField] private int currentIdx = 0;

    private void Start()
    {
        CrowManager.Instance.StopSummonCrow();
    }

    public void NextScene()
    {
        SceneManager.LoadScene("InGameScene");
    }
    public void Next()
    {
        panels[++currentIdx].SetActive(true);
    }
    public void Undo()
    {
        panels[currentIdx--].SetActive(false);
    }

}
