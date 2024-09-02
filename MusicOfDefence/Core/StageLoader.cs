using UnityEngine;
using UnityEngine.SceneManagement;

public class StageLoader : MonoBehaviour
{
    [SerializeField] private string StageName;

    public void SceneLoad()
    {
        SceneManager.LoadScene(StageName);
    }
}
