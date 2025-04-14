using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BottomBar : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] TextMeshProUGUI Atk;
    [SerializeField] TextMeshProUGUI AtkSpeed;
    [SerializeField] TextMeshProUGUI HP;
    [SerializeField] TextMeshProUGUI Range;

    public void Setting(Sprite img, string name, float atk, float atkSpeed, float currentHp, float maxHp, float range)
    {
        image.sprite = img;

        Name.text = name;
        Atk.text = $"Atk : {atk.ToString()}";
        AtkSpeed.text = $"AtkSpeed : {atkSpeed.ToString()}";
        HP.text = $"HP : {currentHp.ToString()}";
        Range.text = $"Range : {range.ToString()}";
    }
}
