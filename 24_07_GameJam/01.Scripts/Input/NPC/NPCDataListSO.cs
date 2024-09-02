using System.Collections.Generic;
using UnityEngine;

namespace Karin
{

    [CreateAssetMenu(menuName = "SO/NPCDataListSO")]
    public class NPCDataListSO : ScriptableObject
    {
        public List<NPCdataSO> list = new List<NPCdataSO>();

        public NPCdataSO GetRandomData()
        {
            int idx = Random.Range(0, list.Count);
            return list[idx];
        }

    }

}