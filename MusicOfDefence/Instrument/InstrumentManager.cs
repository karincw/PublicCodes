using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InstrumentManager : MonoBehaviour
{
    public InstrumentSO instrument;
    public List<Btn> noteBtns;

    [HideInInspector] public GameObject zoomObj;
    [HideInInspector] public GameObject zoomBG;
    public Sorting _sorting;

    public bool isMaxBtn { get; private set; }
    public bool NoteEnd = false;
    public ushort ButtonIndex = 33;

    public UnityEvent<int> attack;

    [SerializeField] private Transform summonTransform;

    private void Awake()
    {
        instrument.rank = 0;
        zoomObj = transform.Find("ZoomIn").gameObject;
        zoomBG = zoomObj.transform.Find("BG").gameObject;
        _sorting = GetComponent<Sorting>();
        summonTransform = transform.Find("Instrument");
    }

    private void Start()
    {
        noteBtns.AddRange(transform.GetComponentsInChildren<Btn>());
        transform.Find("Instrument/Image_BG/Image").GetComponent<SpriteRenderer>().sprite = instrument.image;
        transform.Find("Instrument/Image_BG").GetComponent<SpriteRenderer>().sprite = instrument.rankImage[instrument.rank];
        StartCoroutine("StartTick");
        _sorting.HorizontalSort();
    }

    private void Update()
    {
        if (noteBtns.Count >= _sorting.MaxButton)
        {
            isMaxBtn = true;
        }
        else
        {
            isMaxBtn = false;
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            if (instrument.rank == 3)
            {
                return;
            }
            RankUp();

        }
#endif
    }


    private void RankUp()
    {
        instrument.rank++;
        transform.Find("Instrument/Image_BG/Image").GetComponent<SpriteRenderer>().sprite = instrument.image;
        transform.Find("Instrument/Image_BG").GetComponent<SpriteRenderer>().sprite =
            instrument.rankImage[instrument.rank];
    }


    public IEnumerator StartTick()
    {
        NoteEnd = false;
        foreach (var note in noteBtns)
        {
            yield return new WaitForSeconds(note.TickSpeed);
            StartCoroutine(note.MyTick());
        }

        NoteEnd = true;
        //StartCoroutine("StartTick");
        GameManager.Instance.IsTickEnd();

    }

    public void ResetTick()
    {
        StopCoroutine("StartTick");
        StartCoroutine("StartTick");
    }

    public void TickStop()
    {
        StopCoroutine("StartTick");
    }

    public void TickStart()
    {
        StartCoroutine("StartTick");
    }

    public void AddBtn(Btn newbtn)
    {
        if (isMaxBtn is true) return;
        noteBtns.Add(newbtn);
        newbtn.gameObject.transform.SetParent(summonTransform);
        _sorting.Dimension2Sort();
        ResetTick();
    }

}
