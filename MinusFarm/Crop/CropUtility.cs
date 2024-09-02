using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CW
{

    public class CropUtility : MonoBehaviour
    {
        [SerializedDictionary("CardSO", "Crop")]
        public SerializedDictionary<CardSO, Crop> cardToCropDataDic = new SerializedDictionary<CardSO, Crop>();



        //public IEnumerator GrowCoroutine(int count)
        //{
        //    for (int i = 0; i < count; ++i)
        //    {
        //        yield return new WaitUntil();
        //    }

        //    Debug.Log("123");
        //}
    }

}