using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{

    [SerializeField] Animator ani;

    public string SceneName;

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DIeAnimation", 1);
    }

    public void DIeAnimation()
    {
        ani.SetTrigger("Die");
    }

    public void TitleGo()
    {
        SceneManager.LoadScene(SceneName);
    }
}
