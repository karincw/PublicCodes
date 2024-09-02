using System.Collections;
using System.IO;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private string path;

    public string LoadBackgroundName;
    public string loadStageName;

    WriteBtnListClass load = new WriteBtnListClass();

    private void Awake()
    {
        path = Application.dataPath;
    }
    private void Start()
    {
        StartCoroutine(LoadAndSet());
    }

    public IEnumerator LoadAndSet()
    {
        yield return new WaitUntil(() => Load(LoadBackgroundName));
        yield return new WaitUntil(() => GameManager.Instance.FileManager.SetData(load));
        yield return new WaitUntil(() => Load(loadStageName));
        GameManager.Instance.FileManager.SetLeadjson(load);
    }

    public bool Load(string name)
    {
        load = JsonUtility.FromJson<WriteBtnListClass>(File.ReadAllText(Application.dataPath + $"/{name}.json"));
        return true;
    }

}
