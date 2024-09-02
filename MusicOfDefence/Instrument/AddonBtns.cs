using DG.Tweening;
using UnityEngine;

enum AddonType
{
    Add,
    Upgrade,
}
public class AddonBtns : MonoBehaviour
{
    [SerializeField] private AddonType myAddon;
    InstrumentManager _instrumentManager;
    SpriteRenderer _spriteRenderer;
    [SerializeField] GameObject btn;
    [SerializeField] InstrumentBtn _instrumentBtn;


    private void Awake()
    {
        _instrumentManager = GetComponentInParent<InstrumentManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = new Color(1, 1, 1, 0);
    }

    public void OnEnable()
    {

    }

    private void Update()
    {

    }

    public void OnClick()
    {
        switch (myAddon)
        {
            case AddonType.Add:
                if (_instrumentManager.isMaxBtn == false)
                {
                    Btn newbtn = Instantiate(btn).GetComponent<Btn>();
                    _instrumentManager.AddBtn(newbtn);
                    _instrumentBtn.ReFindBtns();

                    newbtn.name = $"Button{_instrumentManager.ButtonIndex++}";
                }
                break;
            case AddonType.Upgrade:
                _instrumentManager._sorting.MaxButton += _instrumentManager._sorting.MaxLine;
                break;
            default:
                break;
        }
    }
}
