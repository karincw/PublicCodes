using System.Collections.Generic;
using UnityEngine;

public class Sorting : MonoBehaviour
{
    List<Btn> btns = new List<Btn>();
    InstrumentManager _instrumentManager;
    private readonly float sortStartPos = -7.5f + -16.5f;
    public int MaxLine = 32;
    public int MaxButton = 32;
    public bool canDoubleLine;

    [SerializeField] float xSize = 0;
    float halfX { get { return xSize / 2; } }

    private void Awake()
    {
        _instrumentManager = GetComponent<InstrumentManager>();
        btns = _instrumentManager.noteBtns;
    }
    private void Update()
    {
        MaxButton = Mathf.Clamp(MaxButton, MaxLine, MaxLine * 4);
    }

    public void HorizontalSort()
    {
        if (btns.Count == _instrumentManager.noteBtns.Count)
            btns = _instrumentManager.noteBtns;
    
        xSize = Mathf.Clamp((float)MaxLine / btns.Count, 0.5f, 1);
        int lineChange = MaxLine * 2;
        float baseY = transform.position.y;
        float posX = sortStartPos + halfX;

        List<Btn> firstLineBtns = new List<Btn>();
        List<Btn> secondLineBtns = new List<Btn>();
    
        for (int i = 0; i < btns.Count; i++)
        {
            if (i < lineChange)
                firstLineBtns.Add(btns[i]);
            else
                secondLineBtns.Add(btns[i]);
        }

        // First Line
        for (int i = 0; i < firstLineBtns.Count; i++)
        {
            Vector2 pos = new Vector2(posX + (xSize * i), baseY);
            Vector3 scale = new Vector3(xSize, 1, 1);
            SetBtnPositionAndScale(firstLineBtns[i], pos, scale);
        }

        // Second Line (if needed)
        if (secondLineBtns.Count > 0)
        {
            float upperY = baseY + halfX;
            float lowerY = baseY - halfX;

            for (int i = 0; i < firstLineBtns.Count; i++)
            {
                Vector2 pos = new Vector2(posX + (xSize * i), upperY);
                Vector3 scale = new Vector3(xSize, xSize, 1);
                SetBtnPositionAndScale(firstLineBtns[i], pos, scale);
            }

            for (int i = 0; i < secondLineBtns.Count; i++)
            {
                Vector2 pos = new Vector2(posX + (xSize * i), lowerY);
                Vector3 scale = new Vector3(xSize, xSize, 1);
                SetBtnPositionAndScale(secondLineBtns[i], pos, scale);
            }
        }
    }

    private void SetBtnPositionAndScale(Btn btn, Vector2 position, Vector3 scale)
    {
        btn.transform.position = position;
        btn.transform.localScale = scale;
        btn.ResetOrigin();
    }

    public void Dimension2Sort()
    {
        if (btns.Count == _instrumentManager.noteBtns.Count)
        {
            btns = _instrumentManager.noteBtns;
        }
        xSize = 1;

        {
            int i = 0;
            int lineChange = MaxLine;
            float posY = transform.position.y;
            float posX = sortStartPos + halfX;

            foreach (var btn in btns)
            {

                if (i >= lineChange)
                {
                    posY -= 1;
                    posX = sortStartPos + halfX;
                    lineChange += MaxLine;
                }
                Vector2 pos = new Vector2(posX++, posY);
                Vector3 scale = new Vector3(xSize, 1, 1);
                SetBtnPositionAndScale(btn, pos, scale);
                i++;
            }
        }
    }

}
