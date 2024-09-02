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
        bool secondLine = false;
        if (btns.Count == _instrumentManager.noteBtns.Count)
        {
            btns = _instrumentManager.noteBtns;
        }
        xSize = (float)MaxLine / btns.Count;
        xSize = Mathf.Clamp(xSize, 0.5f, 1);

        {
            int lineChange = MaxLine * 2;
            int i = 0;
            float posY1 = transform.position.y;
            float posY2 = transform.position.y;
            float posX = sortStartPos + halfX;
            bool secondLineSorting = false;
            List<Btn> sortingBtns = new List<Btn>();
            foreach (var btn in btns)
            {
                sortingBtns.Add(btn);
                if (i >= lineChange)
                {
                    secondLine = true;
                    posY1 = transform.position.y + halfX;
                    posY2 = transform.position.y - halfX;
                    posX = sortStartPos + halfX;
                }

                if (secondLine)
                {
                    if (secondLineSorting == false)
                    {
                        int i2 = 0;
                        foreach (var btn2 in sortingBtns)
                        {
                            btn2.transform.position = new Vector2(posX + (xSize * i2), posY1);
                            btn2.transform.localScale = new Vector3(xSize, xSize, 1);
                            btn2.ResetOrigin();
                            i2++;
                        }
                        secondLineSorting = true;
                        i = 0;
                    }
                    btn.transform.localScale = new Vector3(xSize, xSize, 1);
                    btn.transform.position = new Vector2(posX + (xSize * i), posY2);
                    btn.ResetOrigin();
                }
                else
                {
                    btn.transform.position = new Vector2(posX + (xSize * i), transform.position.y);
                    btn.transform.localScale = new Vector3(xSize, 1, 1);
                    btn.ResetOrigin();
                }

                i++;
            }
        }
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
                btn.gameObject.transform.position = new Vector2(posX++, posY);
                btn.gameObject.transform.localScale = new Vector3(xSize, 1, 1);
                btn.ResetOrigin();

                i++;
            }
        }
    }

}