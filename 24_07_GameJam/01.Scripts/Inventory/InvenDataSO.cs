using karin;
using System.Collections.Generic;
using UnityEngine;

namespace Karin
{

    [CreateAssetMenu(menuName = "InvenDataSO")]
    public class InvenDataSO : ScriptableObject
    {
        public List<InvenData> list = new();
    }


}
