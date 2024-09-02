using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    public string LoadIndex = "";
    public string SaveIndex = "";

    string StageSummonJson = "";
    WriteBtnListClass leadJson = new WriteBtnListClass();

    Summoner _summoner;

    [SerializeField] private bool CanSave = true;
    [SerializeField] private bool CanLoad = true;

    private void Awake()
    {
        _summoner = FindObjectOfType<Summoner>();
    }

    public void Play()
    {
        if (leadJson == null)
        {
            Debug.LogWarning("LeadJson Is Null");
            return;
        }
        _summoner.Play(leadJson);
    }

    [ContextMenu("WriteData")]
    public bool WriteData()
    {
        Btn[] btns;

        btns = FindObjectsOfType<Btn>();

        btns = btns.OrderBy(t => t.name, StringComparer.Ordinal).ToArray();
        btns = btns.OrderBy(t => t.transform.parent.parent.name, StringComparer.Ordinal).ToArray();

        WriteBtnListClass buttons = new WriteBtnListClass();
        foreach (var btn in btns)
        {
            BtnState state;
            if ((int)btn.myState == 2)
                state = (BtnState)0;
            else if ((int)btn.myState == 3)
                state = (BtnState)1;
            else
                state = btn.myState;

            WriteBtnData data = new WriteBtnData(btn.TickSpeed / 100f, btn.tone, state, btn.transform.parent.parent.name, btn.name);

            buttons.buttons.Add(data);
        }
        StageSummonJson = JsonUtility.ToJson(buttons);

        return true;
    }

    [ContextMenu("SetData")]
    public bool SetData()
    {
        if (leadJson == null)
        { Debug.LogError("LeadJson이 Null임"); return false; }

        Btn[] btns;

        btns = FindObjectsOfType<Btn>();

        btns = btns.OrderBy(t => t.name, StringComparer.Ordinal).ToArray();
        btns = btns.OrderBy(t => t.transform.parent.parent.name, StringComparer.Ordinal).ToArray();

        for (int i = 0; i < btns.Length; i++)
        {

            btns[i].TickSpeed = leadJson.buttons[i].tickSpeed;
            btns[i].tone = leadJson.buttons[i].tone;
            btns[i].myState = leadJson.buttons[i].state;
            btns[i].ImageReRender();
        }

        return true;
    }

    public bool SetData(WriteBtnListClass set)
    {
        if (set == null)
        { Debug.LogError("LeadJson이 Null임"); return false; }

        Btn[] btns;

        btns = FindObjectsOfType<Btn>();

        btns = btns.OrderBy(t => t.name, StringComparer.Ordinal).ToArray();
        btns = btns.OrderBy(t => t.transform.parent.parent.name, StringComparer.Ordinal).ToArray();

        for (int i = 0; i < btns.Length; i++)
        {

            btns[i].TickSpeed = set.buttons[i].tickSpeed;
            btns[i].tone = set.buttons[i].tone;
            btns[i].myState = set.buttons[i].state;
            btns[i].ImageReRender();
        }

        return true;
    }

    [ContextMenu("SaveData")]
    public bool FileSave()
    {
        File.WriteAllText(Application.persistentDataPath + $"/SaveMusic{SaveIndex}.json", StageSummonJson);

        Debug.Log("SaveEnd" + $"{Application.persistentDataPath}");
        return true;
    }

    [ContextMenu("LeadData")]
    public bool FileLoad()
    {
        leadJson = JsonUtility.FromJson<WriteBtnListClass>(File.ReadAllText(Application.persistentDataPath + $"/{LoadIndex}.json"));

        Debug.Log("LoadEnd");
        return true;
    }
    [ContextMenu("EditorSave")]
    public void EditorFileSave()
    {
        File.WriteAllText(Application.dataPath + $"/{SaveIndex}.json", StageSummonJson);
    }

    [ContextMenu("EditorLead")]
    public void EditorFileLoad()
    {
        leadJson = JsonUtility.FromJson<WriteBtnListClass>(File.ReadAllText(Application.dataPath + $"/{LoadIndex}.json"));
    }

    public IEnumerator Save()
    {
        if(CanSave == false)
        {
            Debug.Log("Can't Save");
            yield break;
        }
        yield return new WaitUntil(() => WriteData() == true);
        yield return new WaitUntil(() => FileSave() == true);
    }

    public IEnumerator Load()
    {
        if (CanLoad == false)
        {
            Debug.Log("Can't Load");
            yield break;
        }
        yield return new WaitUntil(() => FileLoad() == true);
        yield return new WaitUntil(() => SetData() == true);
    }

    public void SetLeadjson(WriteBtnListClass set)
    {
        leadJson = set;
    }
}
