using Karin;
using System.Collections;
using System.Collections.Generic;
using Tkfkadlsi;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    public void ResCheat()
    {
        ResourceManager.Instance.BoneCount = 9999;
        ResourceManager.Instance.BranchCount = 9999;
        ResourceManager.Instance.RockCount = 9999;
    }

    public void NextDay()
    {
        var obj = FindObjectOfType<DayAndNight>();
        obj.CurrentTime = 1439;
    }
}
